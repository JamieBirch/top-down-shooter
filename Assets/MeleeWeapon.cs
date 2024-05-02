using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Transform attackPoint;
    public float meleeAttackRange;
    
    public LayerMask enemyLayer;

    public override void Attack()
    {
        Hit();
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
}