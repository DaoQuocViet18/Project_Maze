using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject ShopMenuPanel;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private TextMeshProUGUI numberOfShieldsText;
    [SerializeField] private Button buyShieldBtn;

    private int numberOfShields = 0; // Biến lưu số khiên hiện có
    [SerializeField] private int costOfShields = 2; // Biến lưu số khiên hiện có

    private void Start()
    {
        shopBtn.onClick.AddListener(OnShop);
        backBtn.onClick.AddListener(OnBack);
        buyShieldBtn.onClick.AddListener(BuyShield);

        ShopMenuPanel.SetActive(false);
    }

    private void OnShop()
    {
        numberOfShields = Player.Instance.shield;
        ShopMenuPanel.SetActive(true);
        UpdateShieldText();
    }

    private void OnBack()
    {
        ShopMenuPanel.SetActive(false);
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
