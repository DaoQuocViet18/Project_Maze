using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    [SerializeField] private Button shieldButton;

    private void Start()
    {
        shieldButton.onClick.AddListener(ActiveShield);
    }

    public void ActiveShield()
    {
        EventDispatcher.Dispatch(new EventDefine.OnActiveShield());
    }
}
