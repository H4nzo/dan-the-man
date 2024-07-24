using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(GameData gameData, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData LoadGame(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData gameData = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return gameData;
        }
        else
        {
            return null;
        }
    }

    public static void DeleteSave(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool HasSave(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".save";
        return File.Exists(path);
    }
}
