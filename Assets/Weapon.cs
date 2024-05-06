using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Rigidbody2D rb;
    public float dropForce;

    private bool isHeld = false;
    
    public abstract void Attack();

    public abstract void HitEnemyWhenThrown(Enemy enemyComponent);

    public void Throw()
    {
        rb.simulated = true;
        rb.AddForce(transform.up * dropForce, ForceMode2D.Impulse);
        isHeld = false;
    }
    
    public void Drop()
    {
        rb.simulated = true;
        isHeld = false;
    }

    public void PickUp()
    {
        isHeld = true;
    }
    
    public bool IsHeld()
    {
        return isHeld;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        
        GameObject colGameObject = col.gameObject;
        if (colGameObject.TryGetComponent<Player>(out _) || colGameObject.TryGetComponent<Bullet>(out _))
        {
            return;
        }

        if (colGameObject.TryGetComponent<Enemy>(out var enemyComponent))
        {
            if (enemyComponent.weapon == this)
            {
                return;
            }
            else
            {
                HitEnemyWhenThrown(enemyComponent);
            }
        }
    }
    
}