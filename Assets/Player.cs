using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isAlive = true;
    public GameObject spriteAlive;
    public GameObject spriteDead;

    public float weaponPickupRange;
    public GameObject weaponSlot;

    public Weapon heldWeapon;
    
    public float fistsAttackRange;
    public LayerMask enemyLayer;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (heldWeapon != null)
            {
                ThrowWeapon();
            }
            else
            {
                if (StandingNearWeapon(out var weapon))
                {
                    PickupWeapon(weapon);
                }
            }
        }
    }

    private void PickupWeapon(Weapon weapon)
    {
        if (weapon.IsHeld())
        {
            return;
        }
        heldWeapon = weapon;
        weapon.transform.position = weaponSlot.transform.position;
        weapon.gameObject.transform.SetParent(weaponSlot.transform);
        weapon.rb.simulated = false;
        weapon.transform.rotation = new Quaternion(0,0,0, 0);
        heldWeapon.PickUp();
    }

    private bool StandingNearWeapon(out Weapon weaponComponent)
    {
        weaponComponent = null;
        
        //TODO test if weapon is held by someone else
        Collider2D[] nearbyItems = Physics2D.OverlapCircleAll(transform.position, weaponPickupRange);

        
        foreach (Collider2D nearbyItem in nearbyItems)
        {
            Weapon wC;
            if (nearbyItem.TryGetComponent(out wC))
            {
                weaponComponent = wC;
            }
        }
        return weaponComponent != null;
    }

    private void ThrowWeapon()
    {
        //TODO
        heldWeapon.gameObject.transform.SetParent(null);
        heldWeapon.Throw();
        heldWeapon = null;
    }

    public void Attack()
    {
        if (heldWeapon != null)
        {
            heldWeapon.Attack();
        }
        else
        {
            AttackWithFists();
        }
    }

    private void AttackWithFists()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, fistsAttackRange, enemyLayer);
        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Enemy>().GetHit();
            Debug.Log("we hit " + hitEnemy.name + " with fists");
        }
    }

    public void TakeBullet()
    {
        Die();
    }
    
    public void GetHit()
    {
        Die();
    }
    
    private void Die()
    {
        isAlive = false;
        Debug.Log("Player is Dead");
        spriteAlive.SetActive(false);
        spriteDead.SetActive(true);
    }
}
