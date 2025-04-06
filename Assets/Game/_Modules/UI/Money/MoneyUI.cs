using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EventDefine;

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
        moneyText.text = Player.Instance.money.ToString(); // Cập nhật UI
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnIncreaseMoney>(OnIncreaseMoney);
        EventDispatcher.Add<EventDefine.OnDecreaseMoney>(OnDecreaseMoney);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnIncreaseMoney>(OnIncreaseMoney);
        EventDispatcher.Remove<EventDefine.OnDecreaseMoney>(OnDecreaseMoney);
    }

    private void OnIncreaseMoney(IEventParam param)
    {
        if (param is OnIncreaseMoney increaseMoneyEvent)
        {
            Player.Instance.money += increaseMoneyEvent.money; // ✅ Tăng số tiền theo giá trị truyền vào
            UpdateMoneyUI();
        }
    }

    private void OnDecreaseMoney(IEventParam param)
    {
        if (param is OnDecreaseMoney decreaseMoneyEvent)
        {
            Player.Instance.money -= decreaseMoneyEvent.money; // ✅ Giảm số tiền theo giá trị truyền vào
            UpdateMoneyUI();
        }
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = Player.Instance.money.ToString(); // Cập nhật UI
        Player.Instance.SavePlayer();
    }
}
