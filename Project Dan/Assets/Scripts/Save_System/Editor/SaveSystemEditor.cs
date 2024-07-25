using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemEditor : EditorWindow
{
    private string levelName = "";
    private string message = "";

    [MenuItem("Hanzo/Save System")]
    public static void ShowWindow()
    {
        GetWindow<SaveSystemEditor>("Save System");
    }

    private void OnEnable()
    {
        // Get the current scene name when the editor window is opened
        levelName = SceneManager.GetActiveScene().name;
    }

    private void OnGUI()
    {
        GUILayout.Label("Save System", EditorStyles.boldLabel);

        // Display current scene name
        GUILayout.Label("Current Scene: " + levelName, EditorStyles.label);

        if (GUILayout.Button("Delete Save Data"))
        {
            DeleteSaveData();
        }

        GUILayout.Label(message, EditorStyles.wordWrappedLabel);
    }

    private void DeleteSaveData()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            message = "Level name cannot be empty.";
            Repaint();
            return;
        }

        SaveSystem.DeleteSave(levelName);
        message = $"Save data for level '{levelName}' deleted.";
        Repaint();
    }
}
