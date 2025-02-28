using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerData
{
    public int maxCurrentLevel = 0;

    public PlayerData(Player player)
    {
        maxCurrentLevel = player.maxCurrentLevel;
    }
}

