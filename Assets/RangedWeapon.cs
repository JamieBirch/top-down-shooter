using UnityEngine;

public class RangedWeapon : Weapon
{
    public LayerMask enemyLayer;
    
    public Transform shootingPoint;
    // public GameObject bulletPrefab;
    public GameObject shootEffectPrefab;
    
    public int bulletsMax;
    public int bulletsCurrent;
    // public float bulletForce;
    public float rangedAttackAngle;
    
    private void Start()
    {
        bulletsCurrent = bulletsMax;
    }

    public override void Attack()
    {
        if (bulletsCurrent > 0)
        {
            Shoot();
            bulletsCurrent--;
        }
        else
        {
            //TODO
        }
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        enemyComponent.beStunned();
    }

    public override int GetBulletCount()
    {
        return bulletsCurrent;
    }

    private void Shoot()
    {
        SoundManager.PlaySound(SoundManager.Sound.shoot);
        
        GameObject shootEffect = Instantiate(shootEffectPrefab, shootingPoint.position, shootingPoint.rotation);
        Destroy(shootEffect, 5f);
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                Transform target = hitEnemy.transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;

                if (Vector2.Angle(transform.up, directionToTarget) < rangedAttackAngle / 2)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, target.position);

                    if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallLayer))
                    {
                        ShootEnemy(hitEnemy);
                        // canSeePlayer = true;
                    }
                    /*else
                    {
                        HitMiss();
                    }*/
                
                }
                /*else
                {
                    HitMiss();
                }*/
            }
        }
        /*else
        {
            HitMiss();
        }*/
        
        
        // GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // rb.AddForce(shootingPoint.up * bulletForce, ForceMode2D.Impulse);
    }

    private void ShootEnemy(Collider2D hitEnemy)
    {
        hitEnemy.GetComponent<Enemy>().ReceiveDamage(5);
        // Debug.Log("we hit " + hitEnemy.name);
            
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
    }
}