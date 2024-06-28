using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;

    public GameObject hitEffect;
    public LayerMask wallsLayer;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject colGameObject = col.gameObject;
        if (colGameObject.TryGetComponent<Weapon>(out _) || colGameObject.TryGetComponent<Bullet>(out _))
        {
            return;
        }

        //TODO figure out bullets movement
        if (colGameObject.CompareTag("Player") || colGameObject.CompareTag("enemy"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

            if (colGameObject.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.ReceiveDamage(5, rb.rotation);
                // enemyComponent.TakeBullet();
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
