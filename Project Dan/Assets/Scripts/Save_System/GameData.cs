using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemies = new List<EnemyData>();
    public List<CheckpointData> checkpointDatas = new List<CheckpointData>();
    public List<PickUpData> pickupDatas = new List<PickUpData>();

}

