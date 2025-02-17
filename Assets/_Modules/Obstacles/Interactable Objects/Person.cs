using UnityEngine;

public class Person : MonoBehaviour, IInteractableObject
{
    public void Interact(GameObject targetObject)
    {
        Debug.Log("targetObject: " + targetObject);
        EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
        Time.timeScale = 0;
    }
}
