using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform TransPlayerImage;
    [SerializeField] private ParticleSystem groundingParticle;
    [SerializeField] private GameObject ObjectTrailRenderer;

    private float eulerAnglesY = -180;

    public void SpriteFlipX(Vector2 swipeDelta)
    {
        spriteRenderer.flipX = swipeDelta.x < 0;
    }

    public void AnimaRolling()
    {
        groundingParticle.Stop(true);   // Dừng hệ thống hạt
        groundingParticle.Clear();      // Xóa toàn bộ hạt đang tồn tại ngay lập tức
        animator.CrossFade("Player_Image_Rolling", 0.1f);
    }

    public void AnimaIdle()
    {
        groundingParticle.Play();  
        animator.CrossFade("Player_Image_Idle", 0.1f);
    }

    public void RotateOnMove(Vector2 direction)
    {
        ObjectTrailRenderer.SetActive(false);

        float rotationX = 0, rotationY = 0, rotationZ = 0;
        float trailRotationZ = 0;

        if (direction == Vector2.right)
        {
            rotationY = eulerAnglesY - 180;
            rotationZ = 90;
        }
        else if (direction == Vector2.left)
        {
            rotationY = eulerAnglesY - 180;
            rotationZ = -90;
        }
        else if (direction == Vector2.up)
        {
            rotationZ = 180;
            trailRotationZ = 90;
            eulerAnglesY = 0;
        }
        else if (direction == Vector2.down)
        {
            rotationY = 180;
            trailRotationZ = 90;
            eulerAnglesY = 180;
        }

        TransPlayerImage.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
        ObjectTrailRenderer.transform.rotation = Quaternion.Euler(0, 0, trailRotationZ);

        ObjectTrailRenderer.SetActive(true);
    }

    public void RotateOnCollision(Vector2 direction)
    {
        float rotationZ = 0;

        if (direction == Vector2.right)
        {
            rotationZ = 90;
            TransPlayerImage.eulerAngles = new Vector3(0, eulerAnglesY, (eulerAnglesY == 0) ? rotationZ : -rotationZ);
        }
        else if (direction == Vector2.left)
        {
            rotationZ = -90;
            TransPlayerImage.eulerAngles = new Vector3(0, eulerAnglesY, (eulerAnglesY == 0) ? rotationZ : -rotationZ);

        }

    }
}
