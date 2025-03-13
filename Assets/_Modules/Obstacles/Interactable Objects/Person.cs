using UnityEngine;

public class Person : MonoBehaviour, IInteractableObject
{
    public void InteractGoal(GameObject targetObject)
    {
        GameManager.Instance.activeWinGame();
    }

    public void InteractGameOver(GameObject targetObject)
    {
        Debug.Log("targetObject: " + targetObject);
        EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
        GameManager.Instance.TimeStop();
    }
}
