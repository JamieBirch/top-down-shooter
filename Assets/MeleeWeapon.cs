using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float meleeAttackRange;
    
    public LayerMask enemyLayer;

    public override void Attack()
    {
        Hit();
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        //TODO:
        throw new System.NotImplementedException();
    }

    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange, enemyLayer);

        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Enemy>().GetHit();
            Debug.Log("we hit " + hitEnemy.name);
        }
    }
}