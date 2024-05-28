using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    
    public int bulletsMax;
    public int bulletsCurrent;
    public float bulletForce;
    
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
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootingPoint.up * bulletForce, ForceMode2D.Impulse);
    }
}