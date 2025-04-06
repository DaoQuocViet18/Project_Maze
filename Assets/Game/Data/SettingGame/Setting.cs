using System.IO;
using UnityEngine;

public class Setting : Singleton<Setting>
{
    public bool isSoundOn = true;
    public bool isMusicOn = true;

    public void SaveSetting()
    {
        SaveSystem.SaveSetting(this);
    }

    public void LoadSetting()
    {
        string path = Application.persistentDataPath + "/setting.dat";
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        if (File.Exists(path))
        {
            SettingData date = SaveSystem.LoadSetting();
            isSoundOn = date.isSoundOn;
            isMusicOn = date.isMusicOn;

            Debug.Log("Save file found! Loading player data...");
        }
        else
        {
            Debug.LogWarning("No save file found! Starting new game.");
        }
    }
}
