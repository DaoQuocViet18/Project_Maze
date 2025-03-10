﻿using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class SwipeMovement : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask obstacleMask;

    [SerializeField] private PlayerAnimation _playerAnimation;
    private Rigidbody2D _rb;

    private Vector2 swipeStart;
    private Vector2 direction;
    [SerializeField] private bool grounded = true;

    private float swipeBufferTime = 2f;
    private float bufferedSwipeTime = -1f;
    private bool HasBufferedSwipe => bufferedSwipeTime > 0 && Time.time < bufferedSwipeTime + swipeBufferTime;

    private void Reset() => SetUp();
    private void Awake() => SetUp();

    private void SetUp()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _rb = GetComponent<Rigidbody2D>();

        // 🔹 Cài đặt Rigidbody2D để kiểm tra va chạm tốt hơn
        _rb.gravityScale = 0; // Không dùng trọng lực
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Không cho xoay
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 🔥 Tránh xuyên vật cản
    }

    private void Update()
    {
        HandleSwipe();

        if (grounded && HasBufferedSwipe)
        {
            bufferedSwipeTime = -1f;
            StartMoving().Forget();
        }
    }

    private void HandleSwipe()
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
        if (!grounded) return;
        grounded = false;
        _playerAnimation.RotateOnMove(direction);

        // 🔹 Nếu gần tường, trượt nhẹ về phía trước rồi quay lại
        if (CheckCollision(transform.position + (Vector3)direction * 0.5f))
        {
            Vector3 initialPosition = transform.position;
            Vector3 bumpPosition = transform.position + (Vector3)direction * 0.2f; // Trượt nhẹ lên một chút

            transform.position = bumpPosition;
            await UniTask.Delay(100);
            transform.position = initialPosition; // Đảm bảo nhân vật trở về đúng vị trí cũ

            grounded = true;
            _playerAnimation.RotateOnCollision(direction);
            _playerAnimation.AnimaIdle();
            return;
        }

        _playerAnimation.AnimaRolling();

        // 🔹 Chuyển động bằng Rigidbody2D
        _rb.linearVelocity = direction * speed;

        // 🔹 Kiểm tra va chạm liên tục
        await UniTask.WaitUntil(() => CheckCollision(transform.position + (Vector3)direction * 0.5f));

        // 🔹 Dừng khi va chạm
        _rb.linearVelocity = Vector2.zero;

        transform.position = GetAdjustedPosition();

        grounded = true;
        _playerAnimation.RotateOnCollision(direction);
        _playerAnimation.AnimaIdle();
    }

    private void FixedUpdate()
    {
        if (!grounded && CheckCollision(transform.position + (Vector3)direction * 0.5f))
        {
            _rb.linearVelocity = Vector2.zero; // 🔥 Dừng lại ngay khi chạm vật cản
            grounded = true;
        }
    }

    private bool CheckCollision(Vector3 targetPosition)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPosition, 0.1f, obstacleMask);

        // 🔥 Kiểm tra nếu collider tồn tại và không phải là Trigger
        return collider != null && !collider.isTrigger;
    }

    private Vector3 GetAdjustedPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, obstacleMask);
        if (hit.collider != null)
        {
            float adjustedDistance = Mathf.Max(hit.distance - 0.05f, 0f); // Đảm bảo không lùi vào trong vật cản
            return transform.position + (Vector3)direction * adjustedDistance;
        }
        return transform.position;
    }
}
