using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject colGameObject = col.gameObject;
        Player player;
        if (colGameObject.TryGetComponent<Player>(out player))
        {
            return;
        }
        
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        Enemy enemyComponent;
        if (colGameObject.TryGetComponent<Enemy>(out enemyComponent))
        {
            enemyComponent.TakeBullet();
        }

        Destroy(effect, 5f);
        Destroy(gameObject);
    }
}
