using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int maxCurrentLevel = 0;
    public int[] starsPerLevel; // 🔹 Mảng chứa số sao thu thập được ở từng level

    public PlayerData(Player player)
    {
        maxCurrentLevel = player.maxCurrentLevel;
        starsPerLevel = player.starsPerLevel ?? new int[10];
    }
}
