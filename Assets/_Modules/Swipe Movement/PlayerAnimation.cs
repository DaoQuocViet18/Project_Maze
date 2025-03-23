using Cysharp.Threading.Tasks;
using UnityEngine;
using static EventDefine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator shieldAnim;
    [SerializeField] private SpriteRenderer playerImage;
    [SerializeField] private SpriteRenderer shieldImage;
    [SerializeField] private Transform playerImageTrans;
    [SerializeField] private ParticleSystem bouncingParticles;
    [SerializeField] private GameObject objectTrailRenderer;

    private float eulerAnglesY = -180;

    private void Start()
    {
        shieldImage.enabled = false;
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnActiveShield>(onActiveShield);
        EventDispatcher.Add<EventDefine.OnDisableShield>(onDisableShield);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnActiveShield>(onActiveShield);
        EventDispatcher.Remove<EventDefine.OnDisableShield>(onDisableShield);
    }

    private void onActiveShield(IEventParam param)
    {
        shieldImage.enabled = true;
        shieldAnim.CrossFade("Shield_TurnOn", 0.1f);
    }

    private void onDisableShield(IEventParam param)
    {
        shieldAnim.CrossFade("Shield_Idle", 0.1f);
        shieldImage.enabled = false;
    }

    public void SpriteFlipX(Vector2 swipeDelta)
    {
        playerImage.flipX = swipeDelta.x < 0;
    }

    public void AnimaRolling()
    {
        bouncingParticles.Stop(true);   // Dừng hệ thống hạt
        bouncingParticles.Clear();      // Xóa toàn bộ hạt đang tồn tại ngay lập tức
        playerAnim.CrossFade("Player_Image_Rolling", 0.1f);
    }

    public void AnimaIdle()
    {
        bouncingParticles.Play();
        playerAnim.CrossFade("Player_Image_Idle", 0.1f);
    }

    public void RotateOnMove(Vector2 direction)
    {
        objectTrailRenderer.SetActive(false);

        float rotationX = 0, rotationY = 0, rotationZ = 0;
        float trailRotationZ = 0;

        if (direction == Vector2.right)
        {
            trailRotationZ = 0;
            rotationY = eulerAnglesY - 180;
            rotationZ = 90;
        }
        else if (direction == Vector2.left)
        {
            trailRotationZ = 180;
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
            trailRotationZ = -90;
            eulerAnglesY = 180;
        }

        playerImageTrans.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
        objectTrailRenderer.transform.rotation = Quaternion.Euler(0, 0, trailRotationZ);

        objectTrailRenderer.SetActive(true);
    }

    public void RotateOnCollision(Vector2 direction)
    {
        float rotationZ = 0;

        if (direction == Vector2.right)
        {
            rotationZ = 90;
            playerImageTrans.eulerAngles = new Vector3(0, eulerAnglesY, (eulerAnglesY == 0) ? rotationZ : -rotationZ);
        }
        else if (direction == Vector2.left)
        {
            rotationZ = -90;
            playerImageTrans.eulerAngles = new Vector3(0, eulerAnglesY, (eulerAnglesY == 0) ? rotationZ : -rotationZ);
        }
    }
}
