using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    // public float meleeAttackRange;
    
    public LayerMask enemyLayer;
    public GameObject hitEffect;
    public bool killsWhenThrown = false;

    public override void Attack()
    {
        Hit();
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        //TODO: different effect for different weapons
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        if (killsWhenThrown)
        {
            enemyComponent.GETKNIFED();
            Destroy(effect, 5f);
        }
        else
        {
            enemyComponent.beStunned();
        }
        
        // throw new System.NotImplementedException();
    }

    public override int GetBulletCount()
    {
        return Int32.MaxValue;
    }

    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Enemy>().ReceiveDamage(5);
            Debug.Log("we hit " + hitEnemy.name);
            
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }
    }
}