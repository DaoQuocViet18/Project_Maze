using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab viên đạn
    public Vector3 direction = Vector3.right; // Hướng bắn
    public float bulletSpeed = 10f; // Tốc độ viên đạn

    private Vector3 firePosition; // Vị trí bắn
    private GameObject currentBullet; // Viên đạn hiện tại
    private bool isBulletDisabled = false;

    void Start()
    {
        firePosition = transform.position + direction;
        Shoot(); // Bắt đầu bắn ngay khi game chạy
    }

    private void Update()
    {
        if (currentBullet != null && !currentBullet.activeSelf && !isBulletDisabled)
        {
            isBulletDisabled = true; // Đánh dấu đã gọi BulletDisabled()
            BulletDisabled();
        }
    }

    void Shoot()
    {
        if (currentBullet == null) // Nếu chưa có viên đạn nào, tạo mới
        {
            currentBullet = Instantiate(bulletPrefab, firePosition, Quaternion.identity);
        }
        else
        {
            currentBullet.SetActive(true);
            currentBullet.transform.position = firePosition;
        }
        ApplyForce(currentBullet);
    }

    void ApplyForce(GameObject bullet)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * bulletSpeed; // Đặt vận tốc theo hướng chỉ định
        }
        isBulletDisabled = false;
    }

    public void BulletDisabled() // Gọi khi đạn bị vô hiệu hóa
    {
        StartCoroutine(ReactivateBullet());
    }

    IEnumerator ReactivateBullet()
    {
        yield return new WaitForSeconds(2f); // Đợi 2 giây
        Shoot(); // Kích hoạt lại viên đạn
    }
}
