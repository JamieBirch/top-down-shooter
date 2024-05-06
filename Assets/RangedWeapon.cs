using System;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    
    public int bulletsMax;
    public int bulletsCurrent;
    public float bulletForce = 20f;

    private void Start()
    {
        bulletsCurrent = bulletsMax;
    }

    public override void Attack()
    {
        Shoot();
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        enemyComponent.beStunned();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootingPoint.up * bulletForce, ForceMode2D.Impulse);
    }
}