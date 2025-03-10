using UnityEngine;

public class Person : MonoBehaviour, IInteractableObject
{
    public void InteractGoal(GameObject targetObject)
    {
        GameManager.Instance.ActiveWinGame();
    }

    public void InteractGameOver(GameObject targetObject)
    {
        EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
        GameManager.Instance.TimeStop();
    }

    public void InteractStar(GameObject targetObject)
    {
        EventDispatcher.Dispatch(new EventDefine.OnIncreaseStar());
    }
}
