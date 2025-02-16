using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Animator))]
public class SwipeMovement : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    public LayerMask obstacleMask; // Layer dùng để phát hiện tường
    [SerializeField] private PlayerAnimation _playerAnimation;

    private Vector2 swipeStart;
    private Vector2 direction;
    private bool grounded = true;

    private float swipeBufferTime = 2f;
    private float bufferedSwipeTime = -1f;
    private bool HasBufferedSwipe => bufferedSwipeTime > 0 && Time.time < bufferedSwipeTime + swipeBufferTime;

    private void Reset() => SetUp();
    private void Awake() => SetUp();

    void SetUp() => _playerAnimation = GetComponent<PlayerAnimation>();

    void Update()
    {
        HandleSwipe();
        // 🔹 Chỉ thực hiện di chuyển nếu nhân vật không đang di chuyển
        if (grounded && HasBufferedSwipe)
        {
            Debug.Log("StartMoving_Update");
            bufferedSwipeTime = -1f;
            StartMoving().Forget();
        }
    }

    void HandleSwipe()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeEnd = Input.mousePosition;
            Vector2 swipeDelta = swipeEnd - swipeStart;

           
            if (swipeDelta.magnitude > 30)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    direction = swipeDelta.x > 0 ? Vector2.right : Vector2.left;
                    _playerAnimation.SpriteFlipX(swipeDelta);
                }
                else
                {
                    direction = swipeDelta.y > 0 ? Vector2.up : Vector2.down;
                }

                if (grounded)
                {
                    Debug.Log("StartMoving_HandleSwipe");
                    StartMoving().Forget();
                }
                else
                {
                    bufferedSwipeTime = Time.time;
                }
            }
        }
    }

    private async UniTaskVoid StartMoving()
    {
        if (!grounded) return; // Ngăn gọi liên tục
        grounded = false;
        _playerAnimation.AnimaRolling();

        await MoveUntilObstacle(); // Di chuyển đến khi gặp vật cản

        grounded = true;
        if (bufferedSwipeTime != -1f) return;
        // Khi dừng lại, tính toán vị trí chính xác cách vật cản 0.1f và đặt lại vị trí
        transform.position = GetAdjustedPosition();

        _playerAnimation.RotateOnCollision(direction);
        _playerAnimation.AnimaIdle();
    }

    private async UniTask MoveUntilObstacle()
    {
        Vector2 direction = this.direction;
        _playerAnimation.RotateOnMove(direction);

        // Di chuyển đến khi gặp vật cản
        while (!CheckCollision(transform.position + (Vector3)direction * 0.58f))
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        await UniTask.NextFrame();
    }

    // Trả về vị trí chính xác cách vật cản 0.1f
    private Vector3 GetAdjustedPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, obstacleMask);
        return hit.collider ? (Vector3)hit.point - (Vector3)(direction * 0.1f) : transform.position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)direction * 0.53f, 0.01f);
        }
    }

    private bool CheckCollision(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.01f, obstacleMask) != null;
    }

}
