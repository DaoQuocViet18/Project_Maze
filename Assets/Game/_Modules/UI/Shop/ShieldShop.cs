using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldShop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberOfShieldsText;
    [SerializeField] private Button buyShieldBtn;

    private int numberOfShields = 0; // Biến lưu số khiên hiện có
    [SerializeField] private int costOfShields = 2; // Biến lưu số khiên hiện có

    void Start()
    {
        buyShieldBtn.onClick.AddListener(BuyShield);
    }

    private void OnEnable()
    {
        numberOfShields = Player.Instance.shield;
        UpdateShieldText();
    }

    private void BuyShield()
    {
        if (Player.Instance.money < costOfShields) return;

        numberOfShields = ++Player.Instance.shield; // Tăng số khiên lên 1
        UpdateShieldText(); // Cập nhật UI
        EventDispatcher.Dispatch(new EventDefine.OnDecreaseMoney { money = costOfShields });
    }

    private void UpdateShieldText()
    {
        numberOfShieldsText.text = numberOfShields.ToString(); // Cập nhật hiển thị số khiên
    }
}
