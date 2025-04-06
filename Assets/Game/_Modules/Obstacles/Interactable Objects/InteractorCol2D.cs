using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractorCol2D : MonoBehaviour
{
    public enum InteractionTypeCol2D
    {
        Goal,
        GameOver
    }

    public InteractionTypeCol2D interactionType; // Biến enum để xác định loại tương tác
    private bool CanBeShielded = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out IInteractableObject interactableObject))
        {
            switch (interactionType)
            {
                case InteractionTypeCol2D.Goal:
                    interactableObject.InteractGoal(gameObject);
                    break;
                case InteractionTypeCol2D.GameOver:
                    interactableObject.InteractGameOver(gameObject, CanBeShielded);
                    break;
            }
        }
    }
}
