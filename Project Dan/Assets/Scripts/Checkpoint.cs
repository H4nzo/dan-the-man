using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameController gameController;
    public bool IsTriggered { get; private set; } = false;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsTriggered)
        {
            IsTriggered = true;
            gameController.SaveGame();
            Debug.Log("Game Saved!");
            DisableTrigger();
        }
    }

    public void DisableTrigger()
    {
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponent<SpriteRenderer>().enabled = false; // Assuming you want to hide the trigger visually as well
    }
}