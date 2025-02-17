using DG.Tweening;
using UnityEngine;

public class LooperMovement : MonoBehaviour
{
    public float moveDistance = 5f; // Khoảng cách di chuyển qua lại
    public float moveDuration = 2f; // Thời gian di chuyển qua lại
    public Vector3 moveDirection = Vector3.right; // Hướng di chuyển

    private void Start()
    {
        // Bắt đầu di chuyển đối tượng qua lại
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // Sử dụng DOTween để di chuyển đối tượng qua lại
        transform.DOMove(transform.position + moveDirection * moveDistance, moveDuration)
            .SetLoops(-1, LoopType.Yoyo) // Lặp lại vô hạn theo kiểu "Yoyo" (đi qua lại)
            .SetEase(Ease.Linear); // Thêm easing nếu muốn (Linear cho chuyển động đều)
    }
}
