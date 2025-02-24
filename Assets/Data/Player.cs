using UnityEngine;

public class Player : Singleton<Player>
{
    private int maxCurrentLevel = 0;
    private int currentLevel = 0;

    public int MaxCurrentLevel { get => maxCurrentLevel; set => maxCurrentLevel = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData date = SaveSystem.LoadPlayer();

        this.maxCurrentLevel = date.maxCurrentLevel;
        Debug.Log("maxCurrentLevel: " + maxCurrentLevel);
    }
}
