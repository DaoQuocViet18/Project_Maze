using DG.Tweening;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject damage;

    private static readonly string AttackAnim = "Plant_Collided";
    private static readonly string WithdrawAnim = "Plant_Withdrew";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractableObject _))
        {
            DOVirtual.DelayedCall(0.1f, PlayAttackAnimation);
        }
    }

    private void PlayAttackAnimation()
    {
        float animDuration = PlayAnimation(AttackAnim);
        MoveSpike(1f, animDuration);

        DOVirtual.DelayedCall(1f, PlayWithdrewAnimation);
    }

    private void PlayWithdrewAnimation()
    {
        float animDuration = PlayAnimation(WithdrawAnim);
        MoveSpike(-1f, animDuration);
    }

    private float PlayAnimation(string animationName)
    {
        anim.CrossFade(animationName, 0.1f);
        return anim.GetCurrentAnimatorStateInfo(0).length;
    }

    private void MoveSpike(float distance, float duration)
    {
        damage.transform.DOMoveY(damage.transform.position.y + distance, duration)
                         .SetEase(Ease.Linear);
    }
}
