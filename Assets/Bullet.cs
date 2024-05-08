using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public LayerMask wallsLayer;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject colGameObject = col.gameObject;
        if (colGameObject.TryGetComponent<Weapon>(out _))
        {
            return;
        }

        if (colGameObject.CompareTag("Player") || colGameObject.CompareTag("enemy"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

            if (colGameObject.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.TakeBullet();
            }
        
            if (colGameObject.TryGetComponent<Player>(out var playerComponent))
            {
                playerComponent.TakeBullet();
            }
            Destroy(effect, 5f);
        }
        
        Destroy(gameObject);
    }
}
