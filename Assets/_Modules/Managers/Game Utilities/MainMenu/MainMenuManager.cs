using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        Player.Instance.LoadPlayer();
    }
}
