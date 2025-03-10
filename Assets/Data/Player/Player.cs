using System.IO;
using UnityEngine;

public class Player : Singleton<Player>
{
    public int maxCurrentLevel = 0;
    public int currentLevel = 0;
    public int[] starsPerLevel;

    private void Awake()
    {
        starsPerLevel = new int[10]; // Giả sử có tối đa 100 level
    }

    public void UpdateStarsForLevel(int levelIndex, int newStars)
    {
        if (levelIndex >= 0 && levelIndex < starsPerLevel.Length)
        {
            starsPerLevel[levelIndex] = Mathf.Max(starsPerLevel[levelIndex], newStars);
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dat";
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        if (File.Exists(path))
        {
            PlayerData data = SaveSystem.LoadPlayer();
            this.maxCurrentLevel = data.maxCurrentLevel;
            this.starsPerLevel = data.starsPerLevel ?? new int[10];

            Debug.Log("Save file found! Loading player data...");
        }
        else
        {
            Debug.LogWarning("No save file found! Starting new game.");
        }
    }
}
