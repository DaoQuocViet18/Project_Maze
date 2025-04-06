using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject ShopMenuPanel;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button backBtn;


    private void Start()
    {
        shopBtn.onClick.AddListener(OnShop);
        backBtn.onClick.AddListener(OnBack);
       

        ShopMenuPanel.SetActive(false);
    }

    private void OnShop()
    {
        ShopMenuPanel.SetActive(true);
    }

    private void OnBack()
    {
        ShopMenuPanel.SetActive(false);
    }

}
