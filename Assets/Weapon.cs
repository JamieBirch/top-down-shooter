using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float attackRange;
    public Rigidbody2D rb;
    public float throwForce;
    public GameObject sprite;

    // public string finisherName;

    private bool isHeld = false;
    public bool OnGround = true;
    public float weaponThrowTimeout = 3f;
    public float weaponThrowCountdown = 0f;
    
    public LayerMask wallLayer;
    
    public GameObject hitEffect;

    public Vector2 colliderOffset;
    public Vector2 colliderSize;
    
    // public float weaponKickback;
    
    public abstract void Attack(float rotationZ);

    public abstract void HitEnemyWhenThrown(Enemy enemyComponent);
    
    public abstract int GetBulletCount();

    private void Update()
    {
        if (weaponThrowCountdown > 0f)
        {
            weaponThrowCountdown -= Time.deltaTime;
        }
        else if (!OnGround && !isHeld)
        {
            OnGround = true;
        }
    }

    public void Throw()
    {
        sprite.SetActive(true);

        rb.simulated = true;
        rb.AddForce(transform.up * throwForce, ForceMode2D.Impulse);
        isHeld = false;
        weaponThrowCountdown = weaponThrowTimeout;
    }
    
    public void Drop()
    {
        sprite.SetActive(true);

        isHeld = false;
        rb.simulated = true;
        weaponThrowCountdown = weaponThrowTimeout;
    }

    public void PickUp()
    {
        SoundManager.PlaySound(SoundManager.Sound.pickup_weapon);
        
        isHeld = true;
        OnGround = false;
        sprite.SetActive(false);
        
        
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
            if (enemyComponent.weapon == this || OnGround)
            {
                return;
            }
            else
            {
                HitEnemyWhenThrown(enemyComponent);
            }
        }
    }

    /*public string GetFinisherName()
    {
        return finisherName;
    }*/
    

}