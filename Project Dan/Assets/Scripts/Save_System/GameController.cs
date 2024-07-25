using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public string currentLevelName;
    public List<GameObject> enemyPrefabs;


    private void Start()
    {
        // if (SaveSystem.HasSave("gameSave"))
        // {
        //     LoadGame();
        // }
        // Load the level-specific data
        currentLevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        GameData gameData = SaveSystem.LoadGame(currentLevelName);

        if (gameData != null)
        {
            LoadGame();
        }
        else
        {
            return;
        }
    }

    public void SaveGame()
    {
        GameData gameData = new GameData();

        // Save enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            EnemyData enemyData = new EnemyData
            {
                enemyType = enemyBase.enemyType.ToString(),
                position = new float[3] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z }
            };
            gameData.enemies.Add(enemyData);
        }

        // Save player
        PlayerController player = GameObject.FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            PlayerData playerData = new PlayerData
            {
                position = new float[3] { player.transform.position.x, player.transform.position.y, player.transform.position.z },
                health = player.Health,
                coins = player.inventoryManager.Coins // Use the public property here
            };
            gameData.playerData = playerData;
        }

        // Save used Checkpoints
        Checkpoint[] Checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (var checkpoint in Checkpoints)
        {
            if (checkpoint.IsTriggered)
            {
                CheckpointData checkpointData = new CheckpointData
                {
                    position = new float[3] { checkpoint.transform.position.x, checkpoint.transform.position.y, checkpoint.transform.position.z }
                };
                gameData.checkpointDatas.Add(checkpointData);
            }
        }

        // Save pickups
        Pickup[] pickups = FindObjectsOfType<Pickup>();
        foreach (var pickup in pickups)
        {
            PickUpData pickUpData = new PickUpData
            {
                position = new float[3] { pickup.transform.position.x, pickup.transform.position.y, pickup.transform.position.z },
                type = pickup.type
            };
            gameData.pickupDatas.Add(pickUpData);

            // Save the game and delete previous save if it exists
            SaveSystem.SaveGame(gameData, currentLevelName);
        }
    }

    public void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGame(currentLevelName);
        if (gameData != null)
        {
            // Clear existing enemies
            GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in existingEnemies)
            {
                Destroy(enemy);
            }

            // Instantiate saved enemies
            foreach (var enemyData in gameData.enemies)
            {
                Vector3 position = new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]);
                GameObject prefab = enemyPrefabs.Find(p => p.GetComponent<EnemyBase>().enemyType.ToString() == enemyData.enemyType);
                if (prefab != null)
                {
                    Instantiate(prefab, position, Quaternion.identity);
                }
            }

            // Load player data
            PlayerController player = GameObject.FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.transform.position = new Vector3(gameData.playerData.position[0], gameData.playerData.position[1], gameData.playerData.position[2]);
                player.Health = gameData.playerData.health;
                player.inventoryManager.AddCoin(gameData.playerData.coins); // Use the public property here
            }

            // Disable used Checkpoint
            Checkpoint[] Checkpoints = FindObjectsOfType<Checkpoint>();
            foreach (var checkpoint in Checkpoints)
            {
                foreach (var checkpointData in gameData.checkpointDatas)
                {
                    if (Vector3.Distance(checkpoint.transform.position, new Vector3(checkpointData.position[0], checkpointData.position[1], checkpointData.position[2])) < 0.1f)
                    {
                        checkpoint.DisableTrigger();
                    }
                }
            }

            // Remove picked up items
            // Handle pickups
            Pickup[] pickupsInScene = FindObjectsOfType<Pickup>();
            List<Pickup> pickupsToKeep = new List<Pickup>();

            foreach (var pickup in pickupsInScene)
            {
                bool found = false;
                foreach (var pickUpData in gameData.pickupDatas)
                {
                    if (Vector3.Distance(pickup.transform.position, new Vector3(pickUpData.position[0], pickUpData.position[1], pickUpData.position[2])) < 0.1f &&
                        pickup.type == pickUpData.type)
                    {
                        found = true;
                        pickupsToKeep.Add(pickup);
                        break;
                    }
                }

                if (!found)
                {
                    Destroy(pickup.gameObject);
                }
            }

            // Destroy pickups in the scene that are not in the saved data
            foreach (var pickup in pickupsInScene)
            {
                if (!pickupsToKeep.Contains(pickup))
                {
                    Destroy(pickup.gameObject);
                }
            }
        }
    }


public void ClearSavedData()
{
    SaveSystem.DeleteSave(currentLevelName);
}
}
