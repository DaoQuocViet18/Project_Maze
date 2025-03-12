using UnityEngine;

public interface IInteractableObject
{
    public void InteractGoal(GameObject targetObject);
    public void InteractGameOver(GameObject targetObject);
    public void InteractStar(GameObject targetObject);

    public void InteractMoney(GameObject targetObject);
}
