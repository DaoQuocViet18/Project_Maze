using UnityEngine;
using System;
using System.Collections;

public class BulletTri2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BulletTri2D>(out _) || collision.TryGetComponent<InteractorTri2D>(out _))
            return;

        if (TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = Vector2.zero; 

        gameObject.SetActive(false);
    }
}
