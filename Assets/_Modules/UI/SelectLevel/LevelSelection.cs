using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button[] levelButtons; // Mảng các nút level

    private void Awake()
    {
        quitButton.onClick.AddListener(() => {
            Loader.Instance.LoadWithFade(SceneName.MainMenuScene);
        });

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i; // Lưu trữ chỉ số level
            levelButtons[i].onClick.AddListener(() => {
                Loader.Instance.LoadWithFade(SceneName.GameScene);
                GameManager.CurrentLevel = levelIndex;
                Debug.Log("GameManager.CurrentLevel: " + GameManager.CurrentLevel);
                //Loader.Instance.LoadWithFade((SceneName)System.Enum.Parse(typeof(SceneName), "Level" + levelIndex));
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int levelAt = GameManager.MaxCurrentLevel;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > levelAt)
                levelButtons[i].interactable = false;
        }
    }
}