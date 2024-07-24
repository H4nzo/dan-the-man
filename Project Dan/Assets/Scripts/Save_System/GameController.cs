using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // Assign your enemy prefabs in the inspector

    private void Start()
    {
        if (SaveSystem.HasSave("gameSave"))
        {
            LoadGame();
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

        // Save the game and delete previous save if it exists
        SaveSystem.SaveGame(gameData, "gameSave");
    }

    public void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGame("gameSave");
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
                Debug.Log("Coins = " + gameData.playerData.coins);
                player.inventoryManager.AddCoin(gameData.playerData.coins); // Use the public property here
                // GameObject.FindAnyObjectByType<PlayerController>().inventoryManager.AddCoin(gameData.playerData.coins);
            }
        }
    }

    public void ClearSavedData()
    {
        SaveSystem.DeleteSave("gameSave");
    }
}
