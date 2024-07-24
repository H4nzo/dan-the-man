using UnityEditor;
using UnityEngine;

public class SaveSystemEditor : EditorWindow
{
    private string message = "";

    [MenuItem("Hanzo/Save System")]
    public static void ShowWindow()
    {
        GetWindow<SaveSystemEditor>("Delete Save");
    }

    private void OnGUI()
    {
        GUILayout.Label("Save System", EditorStyles.boldLabel);

        if (GUILayout.Button("Delete Save Data"))
        {
            DeleteSaveData();
        }

        GUILayout.Label(message, EditorStyles.wordWrappedLabel);
    }

    private void DeleteSaveData()
    {
        SaveSystem.DeleteSave("gameSave");
        message = "Save data deletion executed.";
        Repaint();
    }
}
