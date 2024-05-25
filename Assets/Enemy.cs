using UnityEngine;

public class Enemy : MonoBehaviour
{
    public FieldOfView fov;
    public Rigidbody2D rb;
    
    public float speed;
    public float fistAttackRange;

    public int HP;
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
    //TODO: does not work = should be V3/V2
    public Transform defaultPosition;
    private int waypointIndex = 0;

    private Transform target;

    private void Start()
    {
        SetSprite(EnemyState.alive);
        defaultPosition = transform;
        
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
        if (HP <= 0)
        {
            Die();
        }
        
        if (!isAlive)
        {
            return;
        }
        
        if (isStunned && stunCountdown <= 0)
        {
            isStunned = false;
            SetSprite(EnemyState.alive);
        }
        if (isStunned)
        {
            stunCountdown -= Time.deltaTime;
            return;
        }

        if (fov.canSeePlayer && !fov.player.GetComponent<Player>().isDead())
        {
            AttackPlayer();
        } else if (patrolWaypoints.Length != 0)
        {
            Patrol();
        }
        else
        {
            target = defaultPosition;
        }

        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    public virtual void AttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, fov.player.transform.position);
        if (weapon != null)
        {
            if (weapon.attackRange > distanceToPlayer)
            {
                //attack with ranged weapon
                Vector2 lookDir = (Vector2)fov.player.transform.position - rb.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = angle;
                weapon.Attack();
            }
            else
            {
                //get closer
                target = fov.player.transform;
            }
        }
        else
        {
            //attack with fists
            if (distanceToPlayer <= fistAttackRange)
            {
                FistAttack();
            }
            else
            {
                //get closer
                target = fov.player.transform;
            }
        }
    }

    private void FistAttack()
    {
        //TODO
        Debug.Log("enemy attacks with fists");
        fov.player.GetComponent<Player>().GetHit();
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

    /*public void TakeBullet()
    {
        Die();
    }
    
    public void GetHit()
    {
        Die();
    }*/
    
    public void ReceiveDamage(int damage)
    {
        HP -= damage;
    }
    
    public void GetHitByFist()
    {
        if (!isStunned)
        {
            beStunned();
            ReceiveDamage(1);
        }
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
        // Debug.Log("Enemy is Dead");
        SetSprite(EnemyState.dead);
        collider.enabled = false;
        if (weapon != null)
        {
            DropWeapon();
        }
    }
    
    public bool IsAlive()
    {
        return isAlive;
    }
    
    public bool IsStunned()
    {
        return isStunned;
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

    public void GETKNIFED()
    {
        Die();
    }

    public void GetFinished()
    {
        Die();
    }
}
