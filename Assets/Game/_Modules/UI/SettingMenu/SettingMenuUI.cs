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
    [SerializeField] private Button soundToggleBtn;
    public AudioMixer audioMixer;

    private void Start()
    {
        settingBtn.onClick.AddListener(OnSetting);
        backBtn.onClick.AddListener(OnBack);
        soundToggleBtn.onClick.AddListener(ToggleSound);
        musicToggleBtn.onClick.AddListener(ToggleMusic);
        SettingMenuPanel.SetActive(false);
        
        Setting.Instance.LoadSetting();
        audioMixer.SetFloat("sound", Setting.Instance.isSoundOn ? 0 : -80);
        audioMixer.SetFloat("music", Setting.Instance.isMusicOn ? 0 : -80);
        UpdateButtonText(soundToggleBtn, Setting.Instance.isSoundOn);
        UpdateButtonText(musicToggleBtn, Setting.Instance.isMusicOn);
    }

    private void OnSetting()
    {
        SettingMenuPanel.SetActive(true);
    }

    private void OnBack()
    {
        Setting.Instance.SaveSetting();
        SettingMenuPanel.SetActive(false);
    }

    public void ToggleSound()
    {
        Setting.Instance.isSoundOn = !Setting.Instance.isSoundOn;
        audioMixer.SetFloat("sound", Setting.Instance.isSoundOn ? 0 : -80);
        UpdateButtonText(soundToggleBtn, Setting.Instance.isSoundOn);
    }

    public void ToggleMusic()
    {
        Setting.Instance.isMusicOn = !Setting.Instance.isMusicOn;
        audioMixer.SetFloat("music", Setting.Instance.isMusicOn ? 0 : -80);
        UpdateButtonText(musicToggleBtn, Setting.Instance.isMusicOn);
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
