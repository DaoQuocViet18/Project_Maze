using UnityEngine;

public class InteractorTri2D : MonoBehaviour
{
    public enum InteractionTypeTri2D
    {
        Star,
        Money,
        Point
    }

    public InteractionTypeTri2D interactionType; // Biến enum để xác định loại tương tác
    [SerializeField] private AnimationTri2D animationTri2D;

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
            }

            if (!animationTri2D) return;
            animationTri2D.PlayTakeAnimation();
        }
    }
}
