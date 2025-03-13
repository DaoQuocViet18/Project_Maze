using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText; // Text hiển thị số tiền
    [SerializeField] private GameObject MoneyPanel;

    private void Awake()
    {
        MoneyPanel.SetActive(true);
    }

    private void Start()
    {
        moneyText.text = Player.Instance.money.ToString();
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnIncreaseMoney>(OnIncreaseMoney);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnIncreaseMoney>(OnIncreaseMoney);
    }

    private void OnIncreaseMoney(IEventParam param)
    {
        int moneyAmount = ++Player.Instance.money; 
        moneyText.text = moneyAmount.ToString(); // ✅ Cập nhật UI

        AudioManager.Instance.PlaySound(GameAudioClip.COLLECT);
        Player.Instance.SavePlayer();
    }
}
