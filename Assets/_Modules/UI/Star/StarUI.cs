using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    [SerializeField] private GameObject[] stars; // Mảng chứa các sao chưa kích hoạt
    [SerializeField] private GameObject StarPanel;
    [SerializeField] private Image[] StarActivated; // Mảng chứa các image sao đã kích hoạt

    private void Awake()
    {
        StarPanel.SetActive(true);

        // Kiểm tra số lượng sao trong hai mảng
        if (stars.Length != StarActivated.Length)
        {
            Debug.LogError($"Số lượng sao không khớp! stars: {stars.Length}, StarActivated: {StarActivated.Length}");
            return;
        }

        // Kiểm tra các phần tử null
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] == null)
            {
                Debug.LogError($"Phần tử stars[{i}] là null!");
                return;
            }
        }

        for (int i = 0; i < StarActivated.Length; i++)
        {
            if (StarActivated[i] == null)
            {
                Debug.LogError($"Phần tử StarActivated[{i}] là null!");
                return;
            }
        }

        // Ẩn tất cả các sao đã kích hoạt khi bắt đầu
        foreach (var star in StarActivated)
        {
            star.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnIncreaseStar>(OnIncreaseStar);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnIncreaseStar>(OnIncreaseStar);
    }

    private void OnIncreaseStar(IEventParam param)
    {
        // Kiểm tra mảng có hợp lệ không
        if (stars == null || StarActivated == null || stars.Length == 0 || StarActivated.Length == 0)
        {
            Debug.LogError("Mảng stars hoặc StarActivated không hợp lệ!");
            return;
        }

        // 🔹 Lấy số sao đã kích hoạt hiện tại
        int activeStars = StarActivated.Count(star => star.gameObject.activeSelf);

        // 🔹 Nếu đã kích hoạt hết sao thì không làm gì
        if (activeStars >= stars.Length) return;

        // Kiểm tra chỉ số mảng trước khi truy cập
        if (activeStars < 0 || activeStars >= stars.Length || activeStars >= StarActivated.Length)
        {
            Debug.LogError($"Chỉ số không hợp lệ! activeStars: {activeStars}, stars.Length: {stars.Length}, StarActivated.Length: {StarActivated.Length}");
            return;
        }

        // 🔹 Ẩn sao chưa kích hoạt
        if (stars[activeStars].TryGetComponent<Image>(out Image originalImage))
        {
            originalImage.enabled = false;
        }

        // 🔹 Hiện sao đã kích hoạt
        StarActivated[activeStars].gameObject.SetActive(true);

        // 🔹 Phát âm thanh nếu đạt max sao
        if (activeStars + 1 == stars.Length)
        {
            Debug.Log("All stars activated!");
            AudioManager.Instance.PlaySound(GameAudioClip.WOA);
        }
    }
}
