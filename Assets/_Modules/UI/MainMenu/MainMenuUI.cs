using Unity.VisualScripting;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        playButton.onClick.AddListener(() => {
            Loader.Instance.LoadWithFade(SceneName.SelectLevelScene);
            //Loader.Instance.LoadWithFade(SceneName.GameScene);
        });
        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        string path = Application.persistentDataPath + "/player.dat";
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        if (File.Exists(path))
        {
            Debug.Log("Save file found! Loading player data...");
            Player.Instance.LoadPlayer();
        }
        else
        {
            Debug.LogWarning("No save file found! Starting new game.");
        }
    }
}