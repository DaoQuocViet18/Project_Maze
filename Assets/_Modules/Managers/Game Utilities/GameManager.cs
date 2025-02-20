using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float levelTimeMax;

    [SerializeField] private int MaxPoint = 4;
    [SerializeField] private int currentPoint = 0;

    [SerializeField] GameObject[] levelPrefabs; // Prefab của Level
    private GameObject currentLevelInstance; // Instance của Level hiện tại
    private Level currentLevelComponent; // Component Level của Level hiện tại

    [SerializeField] static int maxCurrentLevel = 0;
    [SerializeField] static int currentLevel = 0;

    public static int MaxCurrentLevel { get => maxCurrentLevel; }
    public static int CurrentLevel { get => currentLevel; set => currentLevel = value; }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnIncreasePoint>(onIncreasePoint);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnIncreasePoint>(onIncreasePoint);
    }

    private void Start()
    {
        onLoadLevel(currentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySound(GameAudioClip.POP_SOUND_EFFECT);
        }
    }

    public void onLoadLevel(int levelIndex)
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance); // Xóa Level cũ nếu có
        }

        if (levelIndex < 0 || levelIndex >= levelPrefabs.Length)
        {
            Debug.LogError("Level index out of range: " + levelIndex);
            return;
        }

        Debug.Log("Loading level: " + levelIndex);
        currentLevel = levelIndex;

        // Tạo level mới từ Prefab
        currentLevelInstance = Instantiate(levelPrefabs[currentLevel], transform.position, Quaternion.identity);

        // Lấy component Level từ GameObject
        currentLevelComponent = currentLevelInstance.GetComponent<Level>();

        if (currentLevelComponent == null)
        {
            Debug.LogError("GameObject không có component Level: " + currentLevelInstance.name);
            return;
        }

        // Cập nhật điểm số từ Level
        MaxPoint = currentLevelComponent.MaxPoint;
        currentPoint = 0;

        // Gửi sự kiện cập nhật UI
        EventDispatcher.Dispatch(new EventDefine.OnUpdateProgressBar());
    }

    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public float getCurrenProgress()
    {
        return (float)currentPoint / MaxPoint;
    }

    private void onIncreasePoint(IEventParam param)
    {
        currentPoint++;
        if (currentPoint == MaxPoint)
        {
            activeWinGame();
        }

        EventDispatcher.Dispatch(new EventDefine.OnUpdateProgressBar());
    }

    void activeWinGame()
    {
        if (currentLevel == maxCurrentLevel && maxCurrentLevel < levelPrefabs.Length - 1)
        {
            maxCurrentLevel++;
        }

        Debug.Log("MaxCurrentLevel: " + maxCurrentLevel);
        Debug.Log("levels.Length: " + levelPrefabs.Length);

        // Kiểm tra và gọi onWinGame từ Level component thay vì GameObject
        if (currentLevelComponent != null)
        {
            currentLevelComponent.onWinGame(() =>
            {
                if (currentLevel + 1 == levelPrefabs.Length)
                {
                    Loader.Instance.LoadWithFade(SceneName.EndingScene);
                }
                else
                {
                    EventDispatcher.Dispatch(new EventDefine.OnWinGame());
                }
            });
        }
        else
        {
            Debug.LogError("Không tìm thấy Level component trên currentLevelInstance.");
        }
    }

    void setDefualtPoint()
    {
        currentPoint = 0;
    }
}
