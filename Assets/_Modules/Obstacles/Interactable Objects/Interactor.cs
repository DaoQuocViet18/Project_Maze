using System.Threading.Tasks;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public enum InteractionType
    {
        Goal,
        GameOver
    }

    public InteractionType interactionType; // Biến enum để xác định loại tương tác

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter2D");

        if (other.collider.TryGetComponent(out IInteractableObject interactableObject))
        {
            switch (interactionType)
            {
                case InteractionType.Goal:
                    interactableObject.InteractGoal(gameObject);
                    break;
                case InteractionType.GameOver:
                    interactableObject.InteractGameOver(gameObject);
                    break;
            }
        }
    }
}
