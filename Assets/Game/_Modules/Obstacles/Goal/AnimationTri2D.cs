using UnityEngine;

public class AnimationTri2D : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int TakeHash = Animator.StringToHash("Take"); // Tạo Hash để tối ưu hiệu suất

    private void Awake()
    {
        if (!animator)
            animator = GetComponent<Animator>();
    }

    public void PlayTakeAnimation()
    {
        if (!animator || !animator.HasState(0, TakeHash))
        {
            DeactivateObject(); // Nếu không có animator, tắt luôn
            return;
        }

        animator.updateMode = AnimatorUpdateMode.UnscaledTime; // Không bị ảnh hưởng bởi timeScale
        animator.CrossFade(TakeHash, 0.1f); // Chuyển animation mượt trong 0.1s

        StartCoroutine(WaitForAnimationToEnd());
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd()
    {
        yield return null; // Đợi 1 frame để đảm bảo Animator cập nhật trạng thái

        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        if (animationLength > 0)
        {
            yield return new WaitForSecondsRealtime(animationLength);
        }

        DeactivateObject();
    }

    private void DeactivateObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
