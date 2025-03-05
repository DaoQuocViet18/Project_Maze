using UnityEngine;

[System.Serializable]
public class SettingData 
{
    public bool isSoundOn = true;
    public bool isMusicOn = true;

    public SettingData(Setting setting)
    {
        isSoundOn = setting.isSoundOn;
        isMusicOn = setting.isMusicOn;
    }
}
