using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int maxCurrentLevel = 0;
    public int money = 0;
    public int[] starsPerLevel = new int[10]; // 🔹 Mảng chứa số sao thu thập được ở từng level

    public PlayerData(Player player)
    {
        maxCurrentLevel = player.maxCurrentLevel;
        money = player.money;
        starsPerLevel = player.starsPerLevel ?? new int[10];
    }
}
