using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    [SerializeField] private GameObject[] stars; // Mảng chứa các sao (GameObject)
    [SerializeField] private GameObject StarPanel;

    private void Awake()
    {
        StarPanel.SetActive(true);
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
        // 🔹 Lấy số sao đã kích hoạt hiện tại
        int activeStars = stars.Count(star => star.GetComponent<Image>().color == Color.yellow);

        // 🔹 Nếu đã kích hoạt hết sao thì không làm gì
        if (activeStars >= stars.Length) return;

        // 🔹 Đổi màu sao tiếp theo
        if (stars[activeStars].TryGetComponent<Image>(out Image starImage))
        {
            starImage.color = Color.yellow; // ✅ Đổi màu sao tiếp theo
        }
        else
        {
            Debug.LogError($"GameObject {stars[activeStars].name} does not have an Image component!");
        }

        // 🔹 Phát âm thanh nếu đạt max sao
        if (activeStars + 1 == stars.Length)
        {
            Debug.Log("All stars activated!");
            AudioManager.Instance.PlaySound(GameAudioClip.WOA);
        }
    }

}
