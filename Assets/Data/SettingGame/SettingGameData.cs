using UnityEngine;

[System.Serializable]
public class SettingGameData 
{
    public bool isSFXOn = true;
    public bool isMusicOn = true;

    public SettingGameData(SettingGame settingGame)
    {
        isSFXOn = settingGame.isSFXOn;
        isMusicOn = settingGame.isMusicOn;
    }
}
