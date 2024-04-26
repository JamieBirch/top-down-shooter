using System;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public bool holdingGun;
    
    public Transform attackPoint;
    public float meleeAttackRange;

    public LayerMask enemyLayer;

    public float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (holdingGun)
            {
                Shoot();
            }
            else
            {
                Hit();
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootingPoint.up * bulletForce, ForceMode2D.Impulse);
    }

    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackRange, enemyLayer);

        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Enemy>().GetHit();
            Debug.Log("we hit " + hitEnemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRange);
    }
}
