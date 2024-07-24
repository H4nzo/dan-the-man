using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SaveGame();
            Debug.Log("Game Saved!");
        }
    }
}
