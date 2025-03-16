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
    private Animator animator;
    private bool isAnimating = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAnimating) return; // Nếu đang chạy animation thì không xử lý va chạm nữa

        if (collision.TryGetComponent(out IInteractableObject interactableObject))
        {
            switch (interactionType)
            {
                case InteractionTypeTri2D.Star:
                    interactableObject.InteractStar(gameObject);
                    PlayTakeAnimation();
                    break;
                case InteractionTypeTri2D.Money:
                    interactableObject.InteractMoney(gameObject);
                    PlayTakeAnimation();
                    break;
                case InteractionTypeTri2D.Point:
                    interactableObject.InteractPoint(gameObject);
                    PlayTakeAnimation();
                    break;
            }
        }
    }

    private void PlayTakeAnimation()
    {
        if (animator != null && !isAnimating)
        {
            isAnimating = true;
            // Đảm bảo animation không lặp lại
            animator.updateMode = AnimatorUpdateMode.UnscaledTime; // Để animation không bị ảnh hưởng bởi timeScale
            animator.SetTrigger("Take");

            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("DeactivateObject", animationLength);
        }
        else if (!isAnimating)
        {
            DeactivateObject();
        }
    }

    private void DeactivateObject()
    {
        gameObject.SetActive(false);
        isAnimating = false;
    }
}
