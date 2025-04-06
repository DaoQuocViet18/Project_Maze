using UnityEngine;
using static InteractorCol2D;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class InteractorTri2D : MonoBehaviour
{
    public enum InteractionTypeTri2D
    {
        Star,
        Money,
        Point,
        GameOver
    }

    public InteractionTypeTri2D interactionType; // Biến enum để xác định loại tương tác
    [SerializeField] private AnimationTri2D animationTri2D;
    private bool CanBeShielded = true;

    private void Awake()
    {
        // Đảm bảo Collider2D có isTrigger = true
        if (TryGetComponent<Collider2D>(out var col))
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractableObject interactableObject))
        {
            switch (interactionType)
            {
                case InteractionTypeTri2D.Star:
                    interactableObject.InteractStar(gameObject);
                    break;
                case InteractionTypeTri2D.Money:
                    interactableObject.InteractMoney(gameObject);
                    break;
                case InteractionTypeTri2D.Point:
                    interactableObject.InteractPoint(gameObject);
                    break;
                case InteractionTypeTri2D.GameOver:
                    interactableObject.InteractGameOver(gameObject, CanBeShielded);
                    break;
            }

            if (!animationTri2D) return;
            animationTri2D.PlayTakeAnimation();
        }
    }
}
