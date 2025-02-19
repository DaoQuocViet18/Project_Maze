using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinGameUI : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject WinPanel;

    private ConfirmDialogue confirmDialog;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextLevelBtnClick);
        replayButton.onClick.AddListener(OnReplayBtnClick);
        homeButton.onClick.AddListener(OnHomeBtnClick);

        WinPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnWinGame>(OnWinGame);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnWinGame>(OnWinGame);
    }

    private void OnNextLevelBtnClick()
    {
        // // Move to next level
        // SceneManager.LoadScene(nextSceneLoad);

        // // Setting the next level to be unlocked
        // if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        // {
        //     PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        // }

        //int indexLevel = GameManager.Instance.getCurrentLevel();
        ////GameManager.Instance.onLoadLevel(++indexLevel);
        //GameManager.Instance.onLoadLevelAndDisableLevels(++indexLevel);
        //Loader.Instance.LoadWithFade(SceneName.GameScene);
    }

    private void OnReplayBtnClick()
    {
        // Replay current level
        Loader.Instance.LoadWithFade(SceneName.GameScene);
    }

    private void OnHomeBtnClick()
    {
        // Go to home scene
        Loader.Instance.LoadWithFade(SceneName.MainMenuScene);
    }

    public void ShowWinPanel()
    {
        WinPanel.SetActive(true);
    }

    public void HideWinPanel()
    {
        WinPanel.SetActive(false);
    }

    public void ShowConfirmDialogue(ConfirmDialogue confirmDialogue)
    {
        confirmDialog = confirmDialogue;
    }

    public int nextSceneLoad;

    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnWinGame(IEventParam param)
    {
        Debug.Log("WIN");
        AudioManager.Instance.PlaySound(GameAudioClip.REWARD_SOUND);
        ShowWinPanel();
    }
}