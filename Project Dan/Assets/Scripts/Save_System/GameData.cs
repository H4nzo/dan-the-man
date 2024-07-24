using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<EnemyData> enemies = new List<EnemyData>();
    public PlayerData playerData;
}

