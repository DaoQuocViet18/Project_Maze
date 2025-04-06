using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdsShop : MonoBehaviour
{
    [SerializeField] private Button AdsBtn;
    [SerializeField] private int costOfAds = 2; 

    void Start()
    {
        AdsBtn.onClick.AddListener(AdsShow);
    }

    private void AdsShow()
    {
        EventDispatcher.Dispatch(new EventDefine.OnAdsGame());
        EventDispatcher.Dispatch(new EventDefine.OnIncreaseMoney { money = costOfAds });
    }
}
