using System.IO;
using UnityEngine;

public class Player : Singleton<Player>
{
    public int maxCurrentLevel = 0;
    public int currentLevel = 0;

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
            PlayerData date = SaveSystem.LoadPlayer();
            this.maxCurrentLevel = date.maxCurrentLevel;

            Debug.Log("Save file found! Loading player data...");
        }
        else
        {
            Debug.LogWarning("No save file found! Starting new game.");
        }
    }
}
