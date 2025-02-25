using UnityEngine;

public class AutoCameraSize : MonoBehaviour
{
    void Start()
    {
        Camera.main.orthographicSize = 1920f / (Screen.width / (float)Screen.height) / 200f;
    }

}
