using UnityEngine;

public class RangedWeapon : Weapon
{
    public LayerMask enemyLayer;
    
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    // public GameObject shootEffectPrefab;

    public int partsShotAtOnce;
    
    public int bulletsMax;
    public int bulletsCurrent;
    public float bulletForce;
    public float rangedAttackAngle;

    
    private void Start()
    {
        bulletsCurrent = bulletsMax;
    }

    public override void Attack(float rotationZ, bool spendBullets)
    {
        if (bulletsCurrent > 0)
        {
            Shoot(rotationZ);
            if (spendBullets)
            {
                bulletsCurrent--;
            }
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.no_ammo);
        }
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        enemyComponent.beStunned(transform.rotation.z);
    }

    public override int GetBulletCount()
    {
        return bulletsCurrent;
    }

    public override void Reload()
    {
        bulletsCurrent = bulletsMax;
    }

    private void Shoot(float rotationZ)
    {
        SoundManager.PlaySound(SoundManager.Sound.shoot);
        
        // GameObject shootEffect = Instantiate(shootEffectPrefab, shootingPoint.position, shootingPoint.rotation);
        // Destroy(shootEffect, 5f);
        
        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        /*if (hitEnemies.Length > 0)
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
                        ShootEnemy(hitEnemy, rotationZ);
                        // canSeePlayer = true;
                    }
                    /*else
                    {
                        HitMiss();
                    }#1#
                
                }
                /*else
                {
                    HitMiss();
                }#1#
            }
        }*/
        /*else
        {
            HitMiss();
        }*/


        for (int i = 0; i < partsShotAtOnce; i++)
        {
            Vector3 spread = new Vector3(Random.Range(-rangedAttackAngle/2, rangedAttackAngle/2), 0,0);
            GameObject projectile = Instantiate(bulletPrefab, shootingPoint.position + (spread * 0.01f), shootingPoint.rotation);
            projectile.GetComponent<Rigidbody2D>()
                // .AddForce(bulletForce * (shootingPoint.up + Random.Range(-rangedAttackAngle/2, rangedAttackAngle/2)) /*+ (100 * shootingPoint.right * spread.x)*/, ForceMode2D.Impulse);
                .AddForce(bulletForce * (shootingPoint.up + shootingPoint.transform.right * Random.Range(-rangedAttackAngle/8, rangedAttackAngle/8)) /*+ (100 * shootingPoint.right * spread.x)*/, ForceMode2D.Impulse);
            // projectile.GetComponent<Rigidbody>().AddForce((100 * shootingPoint.up * (bulletForce + Random.Range(0, spread.x))) + (100 * shootingPoint.transform.right * spread.x));
            Destroy(projectile, 1f);
        }
        // GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // rb.AddForce(shootingPoint.up * bulletForce, ForceMode2D.Impulse);
    }

    private void ShootEnemy(Collider2D hitEnemy, float rotationZ)
    {
        hitEnemy.GetComponent<Enemy>().ReceiveDamage(5, rotationZ);
        // Debug.Log("we hit " + hitEnemy.name);
            
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
    }
}