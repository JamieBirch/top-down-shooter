using UnityEngine;

public class MeleeWeapon : Weapon
{
    // public float meleeAttackRange;
    
    public LayerMask enemyLayer;
    public GameObject hitEffect;

    public override void Attack()
    {
        Hit();
    }

    public override void HitEnemyWhenThrown(Enemy enemyComponent)
    {
        //TODO: different effect for different weapons
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        
        enemyComponent.GETKNIFED();
        Destroy(effect, 5f);
        
        // throw new System.NotImplementedException();
    }

    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Enemy>().GetHit();
            Debug.Log("we hit " + hitEnemy.name);
            
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }
    }
}