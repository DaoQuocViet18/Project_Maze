using UnityEngine;
using System;
using System.Collections;

public class BulletCol2D : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.TryGetComponent(out IInteractableObject interactableObject))
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Đặt vận tốc về 0
            }

            gameObject.SetActive(false);
        }
    }
}
