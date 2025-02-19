using UnityEngine;

public interface IInteractableObject
{
    public void InteractGoal(GameObject targetObject);
    public void InteractGameOver(GameObject targetObject);
}
