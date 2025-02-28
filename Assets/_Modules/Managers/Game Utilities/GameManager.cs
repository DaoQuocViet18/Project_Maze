using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float levelTimeMax;

    [SerializeField] private int MaxPoint = 4;
    [SerializeField] private int currentPoint = 0;

    public GameObject[] levelPrefabs; // Prefab của Level
    private GameObject currentLevelInstance; // Instance của Level hiện tại
    private Level currentLevelComponent; // Component Level của Level hiện tại

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
        
        onLoadLevel(Player.Instance.currentLevel);
        TimeRun();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySound(GameAudioClip.POP_SOUND_EFFECT);
        }
    }

    public void TimeRun()
    {
        Time.timeScale = 1;
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
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
        Player.Instance.currentLevel = levelIndex;

        // Tạo level mới từ Prefab
        currentLevelInstance = Instantiate(levelPrefabs[Player.Instance.currentLevel], transform.position, Quaternion.identity);
        currentLevelInstance.transform.parent = GameObject.Find("Environment").transform;
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
    }

    public void activeWinGame()
    {
        if (Player.Instance.currentLevel == Player.Instance.maxCurrentLevel && Player.Instance.maxCurrentLevel < levelPrefabs.Length - 1)
        {
            Player.Instance.maxCurrentLevel++;
        }

        Debug.Log("MaxCurrentLevel: " + Player.Instance.maxCurrentLevel);
        Debug.Log("currentLevel: " + Player.Instance.currentLevel);
        Debug.Log("levels.Length: " + levelPrefabs.Length);

        // Kiểm tra và gọi onWinGame từ Level component thay vì GameObject
        if (currentLevelComponent != null)
        {
            currentLevelComponent.onWinGame(() =>
            {
                EventDispatcher.Dispatch(new EventDefine.OnWinGame());
                Player.Instance.SavePlayer();
                TimeStop();
            });
        }
        else
        {
            Debug.LogError("Không tìm thấy Level component trên currentLevelInstance.");
        }
    }

    public void activeLostGame()
    {
        // Kiểm tra và gọi onLostGame từ Level component thay vì GameObject
        if (currentLevelComponent != null)
        {
            currentLevelComponent.onLostGame(() =>
            {
                EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
                TimeStop();
            });
        }
        else
        {
            Debug.LogError("Không tìm thấy Level component trên currentLevelInstance.");
        }
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
            //activeWinGame();
        }

        EventDispatcher.Dispatch(new EventDefine.OnUpdateProgressBar());
    }
}
