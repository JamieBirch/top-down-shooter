using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isAlive = true;
    public GameObject spriteAlive;
    public GameObject spriteDead;

    public float weaponPickupRange;
    public GameObject weaponSlot;

    public Weapon heldWeapon;
    
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
                DropWeapon();
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
        heldWeapon = weapon;
        weapon.transform.position = weaponSlot.transform.position;
        weapon.GetComponent<Rigidbody2D>().rotation = 0f;
        weapon.gameObject.transform.SetParent(weaponSlot.transform);

        weapon.PickUp();
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

    private void DropWeapon()
    {
        //TODO
        heldWeapon.gameObject.transform.SetParent(null);
        heldWeapon.Drop();
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
        //TODO implement fists as melee weapon 
        throw new System.NotImplementedException();
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
