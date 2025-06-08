using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int record;

    public PlayerData(int bestScore)
    {
        record = bestScore;
    }
}
