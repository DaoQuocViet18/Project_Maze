using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    [SerializeField] private Button shieldButton;
    [SerializeField] private TextMeshProUGUI shieldText; // Thêm tham chiếu đến Text để hiển thị số khiên

    private void Start()
    {
        UpdateShieldText(); // Cập nhật text khi bắt đầu
        shieldButton.onClick.AddListener(ActiveShield);
    }

    public void ActiveShield()
    {
        if (Player.Instance.shield <= 0) return;

        Player.Instance.shield--;
        Player.Instance.SavePlayer();
        UpdateShieldText(); // Cập nhật lại text sau khi sử dụng khiên

        EventDispatcher.Dispatch(new EventDefine.OnActiveShield());
    }

    private void UpdateShieldText()
    {
        shieldText.text = $"{Player.Instance.shield}"; // Hiển thị số khiên
    }
}
