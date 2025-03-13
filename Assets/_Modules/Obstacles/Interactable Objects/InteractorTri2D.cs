using UnityEngine;

public class InteractorTri2D : MonoBehaviour
{
    public enum InteractionTypeTri2D
    {
        Star,
        Money
    }

    public InteractionTypeTri2D interactionType; // Biến enum để xác định loại tương tác


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractableObject interactableObject))
        {
            switch (interactionType)
            {
                case InteractionTypeTri2D.Star:
                    interactableObject.InteractStar(gameObject);
                    gameObject.SetActive(false);
                    break;
                case InteractionTypeTri2D.Money:
                    interactableObject.InteractMoney(gameObject);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
