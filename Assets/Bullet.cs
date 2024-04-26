using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        GameObject colGameObject = col.gameObject;
        Enemy enemyComponent;
        if (colGameObject.TryGetComponent<Enemy>(out enemyComponent))
        {
            enemyComponent.TakeBullet();
        }

        Destroy(effect, 5f);
        Destroy(gameObject);
    }
}
