using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Player.Instance.LoadPlayer();
    }
}
