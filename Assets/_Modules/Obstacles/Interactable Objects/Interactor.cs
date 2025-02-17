using System.Threading.Tasks;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.TryGetComponent(out IInteractableObject interactableObject))
    //    {
    //        interactableObject.Interact(gameObject);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter2D");
        if (other.collider.TryGetComponent(out IInteractableObject interactableObject))
        {
            interactableObject.Interact(gameObject);
        }
    }
}
