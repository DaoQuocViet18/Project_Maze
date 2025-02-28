using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenuPanel;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button musicToggleBtn;
    [SerializeField] private Button sfxToggleBtn;
    public AudioMixer audioMixer;

    private void Start()
    {
        settingBtn.onClick.AddListener(OnSetting);
        backBtn.onClick.AddListener(OnBack);
        sfxToggleBtn.onClick.AddListener(ToggleSound);
        musicToggleBtn.onClick.AddListener(ToggleMusic);
        SettingMenuPanel.SetActive(false);
        
        SettingGame.Instance.LoadSettingGame();
        audioMixer.SetFloat("sound", SettingGame.Instance.isSFXOn ? 0 : -80);
        audioMixer.SetFloat("music", SettingGame.Instance.isMusicOn ? 0 : -80);
        UpdateButtonText(sfxToggleBtn, SettingGame.Instance.isSFXOn);
        UpdateButtonText(musicToggleBtn, SettingGame.Instance.isMusicOn);
    }

    private void OnSetting()
    {
        SettingMenuPanel.SetActive(true);
    }

    private void OnBack()
    {
        SettingGame.Instance.SaveSettingGame();
        SettingMenuPanel.SetActive(false);
    }

    public void ToggleSound()
    {
        SettingGame.Instance.isSFXOn = !SettingGame.Instance.isSFXOn;
        audioMixer.SetFloat("sound", SettingGame.Instance.isSFXOn ? 0 : -80);
        UpdateButtonText(sfxToggleBtn, SettingGame.Instance.isSFXOn);
    }

    public void ToggleMusic()
    {
        SettingGame.Instance.isMusicOn = !SettingGame.Instance.isMusicOn;
        audioMixer.SetFloat("music", SettingGame.Instance.isMusicOn ? 0 : -80);
        UpdateButtonText(musicToggleBtn, SettingGame.Instance.isMusicOn);
    }

    private void UpdateButtonText(Button button, bool isOn)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            string originalText = buttonText.text.Replace("<s>", "").Replace("</s>", ""); // Xóa gạch ngang cũ trước khi cập nhật
            buttonText.text = isOn ? originalText : $"<s>{originalText}</s>";
        }
    }

}
