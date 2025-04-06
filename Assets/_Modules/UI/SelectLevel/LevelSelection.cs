using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject[] levelObjects; // Mảng các GameObject chứa Button + Stars

    private void Awake()
    {
        quitButton.onClick.AddListener(() => {
            Loader.Instance.LoadWithFade(SceneName.MainMenuScene);
        });

        for (int i = 0; i < levelObjects.Length; i++)
        {
            int levelIndex = i; // Lưu trữ chỉ số level
            Button levelButton = levelObjects[i].GetComponentInChildren<Button>(); // Lấy Button trong GameObject
            if (levelButton != null)
            {
                levelButton.onClick.AddListener(() => {
                    Player.Instance.currentLevel = levelIndex;
                    Loader.Instance.LoadWithFade(SceneName.GameScene);
                });
            }
        }
    }

    void Start()
    {
        Player.Instance.LoadPlayer();
        int levelAt = Player.Instance.maxCurrentLevel;

        for (int i = 0; i < levelObjects.Length; i++)
        {
            Button levelButton = levelObjects[i].GetComponentInChildren<Button>(); // Lấy Button
            Image[] starImages = levelObjects[i].GetComponentsInChildren<Image>(); // Lấy tất cả hình ảnh

            if (levelButton != null)
            {
                levelButton.interactable = i <= levelAt;
            }

            int starsEarned = (i < Player.Instance.starsPerLevel.Length) ? Player.Instance.starsPerLevel[i] : 0;

            // Cập nhật hình ảnh sao
            for (int j = 0; j < 3; j++)
            {
                if (starImages.Length > j + 1) // Đảm bảo không lỗi mảng
                {
                    starImages[j + 1].enabled = j < starsEarned; // Bật/tắt hiển thị sao
                }
            }
        }
    }
}
