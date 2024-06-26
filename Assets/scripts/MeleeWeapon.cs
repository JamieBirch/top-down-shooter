using UnityEngine;

public class MeleeWeapon : Weapon
{
    // public float meleeAttackRange;
    public float meleeAttackAngle;

    public LayerMask enemyLayer;
    // public GameObject hitEffect;
    public bool killsWhenThrown = false;

    public override void Attack(float rotationZ)
    {
        Hit(rotationZ);
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        //TODO: different effect for different weapons
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        if (killsWhenThrown)
        {
            enemyComponent.DeathByThrownWeapon(rb.rotation);
            Destroy(effect, 5f);
        }
        else
        {
            enemyComponent.beStunned(rb.rotation);
        }
        
        // throw new System.NotImplementedException();
    }

    public override int GetBulletCount()
    {
        return -1;
    }

    private void Hit(float rotationZ)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            //currently for only 1 enemy
            Transform target = hitEnemies[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < meleeAttackAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallLayer))
                {
                    HitEnemy(hitEnemies[0], rotationZ);
                    // canSeePlayer = true;
                }
                else
                {
                    HitMiss();
                }
                
            }
            else
            {
                HitMiss();
            }
        }
        else
        {
            HitMiss();
        }
    }

    private void HitMiss()
    {
        // animator.SetTrigger("miss with weapon");
        SoundManager.PlaySound(SoundManager.Sound.fist_miss);
    }

    private void HitEnemy(Collider2D hitEnemy, float rotationZ)
    {
        // animator.SetTrigger("hit with weapon");
        SoundManager.PlaySound(SoundManager.Sound.melee_hit);
        hitEnemy.GetComponent<Enemy>().ReceiveDamage(5, rotationZ);
        // Debug.Log("we hit " + hitEnemy.name);
            
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
    }
}