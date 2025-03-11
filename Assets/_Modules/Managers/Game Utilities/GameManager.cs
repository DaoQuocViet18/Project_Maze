using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] levelPrefabs; // Prefab của Level
    private GameObject currentLevelInstance; // Instance của Level hiện tại
    public Level currentLevelComponent; // Component Level của Level hiện tại

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnIncreaseStar>(onIncreaseStar);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnIncreaseStar>(onIncreaseStar);
    }


    private void Start()
    {
        Player.Instance.LoadPlayer(); 
        LoadLevel(Player.Instance.currentLevel);
        TimeRun();
    }

    public void TimeRun()
    {
        Time.timeScale = 1;
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void LoadLevel(int levelIndex)
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

        // Reset star count khi bắt đầu level mới
        currentLevelComponent.ResetStars();
    }

    public void ActiveWinGame()
    {
        if (Player.Instance.currentLevel == Player.Instance.maxCurrentLevel && Player.Instance.maxCurrentLevel < levelPrefabs.Length - 1)
        {
            Player.Instance.maxCurrentLevel++;
        }

        // Kiểm tra và gọi onWinGame từ Level component
        if (currentLevelComponent != null)
        {
            int starsEarned = currentLevelComponent.currentStars; // Lấy số sao đạt được từ level hiện tại
            Player.Instance.UpdateStarsForLevel(Player.Instance.currentLevel, starsEarned); // Cập nhật số sao cho level

            currentLevelComponent.OnWinGame(() =>
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

    public void ActiveLostGame()
    {
        if (currentLevelComponent != null)
        {
            currentLevelComponent.OnLostGame(() =>
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

    private void onIncreaseStar(IEventParam param)
    {
        if (currentLevelComponent == null)
        {
            Debug.LogError("Không tìm thấy Level component!");
            return;
        }

        if (currentLevelComponent.currentStars < currentLevelComponent.MaxStars)
        {
            currentLevelComponent.currentStars++;
        }

        Debug.Log("currentStars: " + currentLevelComponent.currentStars);

        EventDispatcher.Dispatch(new EventDefine.OnUpdateProgressBar());
    }

}
