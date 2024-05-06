using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    
    private bool isAlive = true;
    private bool isStunned = false;
    public GameObject spriteAlive;
    public GameObject spriteStunned;
    public GameObject spriteDead;
    public BoxCollider2D collider;

    public GameObject weaponSlot;
    public GameObject weaponPrefab;
    public Weapon weapon;

    public float stunTimeout;
    public float stunCountdown;
    
    public Transform[] patrolWaypoints;
    private int waypointIndex = 0;

    private Transform target;

    private void Start()
    {
        SetSprite(EnemyState.alive);
        if (patrolWaypoints.Length != 0)
        {
            target = patrolWaypoints[0];
        }

        if (weaponPrefab != null)
        {
            GameObject instantiatedWeapon = Instantiate(weaponPrefab, weaponSlot.transform.position, new Quaternion(0,0,180, 0), weaponSlot.transform);
            weapon = instantiatedWeapon.GetComponent<Weapon>();
            weapon.PickUp();
            weapon.rb.simulated = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (stunCountdown <= 0)
        {
            isStunned = false;
            SetSprite(EnemyState.alive);
        }
        if (isStunned)
        {
            stunCountdown -= Time.deltaTime;
            return;
        }
        
        if (patrolWaypoints.Length != 0 && isAlive)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.2f)
            {
                GetNextWaypoint();
            }
        }
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= patrolWaypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        else
        {
            waypointIndex++;
        }
        target = patrolWaypoints[waypointIndex];
    }

    public void TakeBullet()
    {
        Die();
    }
    
    public void GetHit()
    {
        Die();
    }

    public void beStunned()
    {
        stunCountdown = stunTimeout;
        
        if (weapon != null)
        {
            DropWeapon();
        }
        
        isStunned = true;
        SetSprite(EnemyState.stunned);
    }
    
    private void DropWeapon()
    {
        //TODO: test
        weapon.gameObject.transform.SetParent(null);
        weapon.Drop();
        weapon = null;
    }
    
    private void PickupWeapon(Weapon foundWeapon)
    {
        weapon = foundWeapon;
        foundWeapon.transform.position = weaponSlot.transform.position;
        foundWeapon.gameObject.transform.SetParent(weaponSlot.transform);
        foundWeapon.rb.simulated = false;
        foundWeapon.transform.rotation = new Quaternion(0,0,180, 0);
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log("Enemy is Dead");
        SetSprite(EnemyState.dead);
        collider.enabled = false;
    }

    private void SetSprite(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.alive:
            {
                spriteAlive.SetActive(true);
                spriteStunned.SetActive(false);
                spriteDead.SetActive(false);
                break;
            }
            case EnemyState.stunned:
            {
                spriteAlive.SetActive(false);
                spriteStunned.SetActive(true);
                spriteDead.SetActive(false);
                break;
            }
            case EnemyState.dead:
            {
                spriteAlive.SetActive(false);
                spriteStunned.SetActive(false);
                spriteDead.SetActive(true);
                break;
            }
        }
    }

    enum EnemyState
    {
        alive,
        stunned,
        dead
    }
}
