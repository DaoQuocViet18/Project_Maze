using System.IO;
using UnityEngine;

public class SettingGame : Singleton<SettingGame>
{
    public bool isSFXOn = true;
    public bool isMusicOn = true;

    public void SaveSettingGame()
    {
        SaveSystem.SaveSettingGame(this);
    }

    public void LoadSettingGame()
    {
        string path = Application.persistentDataPath + "/settingGame.dat";
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        if (File.Exists(path))
        {
            SettingGameData date = SaveSystem.LoadSettingGame();
            isSFXOn = date.isSFXOn;
            isMusicOn = date.isMusicOn;

            Debug.Log("Save file found! Loading player data...");
        }
        else
        {
            Debug.LogWarning("No save file found! Starting new game.");
        }
    }
}
