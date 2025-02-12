using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class SwipeMovement : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    public LayerMask obstacleMask; // Layer dùng để phát hiện tường

    private Vector2 swipeStart;
    private Vector2 direction;
    private bool isMoving = false;

    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (isMoving) return;

        // Bắt đầu vuốt
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }

        // Kết thúc vuốt
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeEnd = Input.mousePosition;
            Vector2 swipeDelta = swipeEnd - swipeStart;

            if (swipeDelta.magnitude > 50) // Chỉ nhận swipe lớn hơn ngưỡng này
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    direction = swipeDelta.x > 0 ? Vector2.right : Vector2.left;
                }
                else
                {
                    direction = swipeDelta.y > 0 ? Vector2.up : Vector2.down;
                }

                StartMoving();
            }
        }
    }

    async void StartMoving()
    {
        isMoving = true;
        await MoveUntilObstacle();
        isMoving = false; // Dừng lại khi gặp vật cản
    }

    async Task MoveUntilObstacle()
    {
        while (!CheckCollision(transform.position + (Vector3)direction * 0.53f))
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            await Task.Yield(); // Thay thế yield return null
        }
    }

    bool CheckCollision(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.01f, obstacleMask);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red; // Màu đỏ để dễ thấy
    //    Vector3 targetPosition = transform.position + (Vector3)direction * 0.53f; // Vị trí kiểm tra

    //    // Vẽ vòng tròn tại vị trí kiểm tra
    //    Gizmos.DrawWireSphere(targetPosition, 0.01f);
    //    Gizmos.DrawLine(transform.position, targetPosition * 1.0f);
    //}
}
