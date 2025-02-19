using UnityEngine;

public class Person : MonoBehaviour, IInteractableObject
{
    public void InteractGoal(GameObject targetObject)
    {
        Debug.Log("targetObject: " + targetObject);
        EventDispatcher.Dispatch(new EventDefine.OnWinGame());
        Time.timeScale = 0;
    }

    public void InteractGameOver(GameObject targetObject)
    {
        Debug.Log("targetObject: " + targetObject);
        EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
        Time.timeScale = 0;
    }

}
