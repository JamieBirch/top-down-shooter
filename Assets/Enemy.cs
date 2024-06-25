using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    public FieldOfView fov;
    public Rigidbody2D rb;

    public AIDestinationSetter destinationSetter;
    
    public bool Alert = false;
    
    public float speed;
    public float fistAttackRange;

    public int HP;
    private bool isAlive = true;
    private bool isStunned = false;
    public GameObject spriteAlive;
    public GameObject spriteStunned;
    public GameObject spriteDead;
    [FormerlySerializedAs("collider")] public BoxCollider2D collider_;

    public GameObject weaponSlot;
    public GameObject weaponPrefab;
    public Weapon weapon;

    public float stunTimeout;
    public float stunCountdown;

    public float alertTimeout;
    public float alertCountdown;
    
    public Transform[] patrolWaypoints;
    //TODO: does not work = should be V3/V2
    // public Vector3 defaultPosition;
    private int waypointIndex = 0;

    // public Vector3 emptyVector = new Vector3();
    // public Vector3 target;

    public GameObject playerLastSeenLocationPrefab;
    public GameObject defaultEnemyLocationPrefab;
    public Transform defaultPosition;

    public GameObject bloodSpotPrefab;
    private int finisherCounterMax = 3;
    private int finisherCounter = 0;
    private bool inFinisher = false;

    
    private void Start()
    {
        SetSprite(EnemyState.alive);
        defaultPosition = Instantiate(defaultEnemyLocationPrefab, transform.position, Quaternion.identity).transform; 
        // defaultPosition = transform.position;
        
        if (patrolWaypoints.Length != 0)
        {
            destinationSetter.target = patrolWaypoints[0];
        }
        else
        {
            destinationSetter.target = null;
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
        /*if (HP <= 0)
        {
            Die();
        }*/
        
        if (!isAlive)
        {
            return;
        }
        
        if (isStunned && stunCountdown <= 0)
        {
            isStunned = false;
            SetSprite(EnemyState.alive);
        }
        if (isStunned && !inFinisher)
        {
            stunCountdown -= Time.deltaTime;
            return;
        }

        if (fov.canSeePlayer && !fov.player.GetComponent<Player>().IsDead())
        {
            Alert = true;
            alertCountdown = alertTimeout;
            //run to player/shoot player
            AttackPlayer();
        }
        else
        {
            if (Alert)
            {
                //run to last location of player
                
                //if already there - look around (rotate)
                if (destinationSetter.target.position == transform.position)
                {
                    //TODO look around between two points of forward+-90 degrees 
                    Vector2 lookDir = (Vector2)fov.player.transform.position - rb.position;
                    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                    rb.rotation = angle;
                }
                alertCountdown -= Time.deltaTime;
            }
            
            if (Alert && alertCountdown <= 0)
            {
                Alert = false;
                destinationSetter.target = defaultPosition;
            }
            if (patrolWaypoints.Length != 0)
            {
                Patrol();
            }
            /*else
            {
                //return to start point
                target = defaultPosition;
            }*/
            
        }

        /*if (destinationSetter.target != null)
        {
            /*if (transform.position == target)
            {
                target = emptyVector;
            }
            else
            {
                
            }#1#
            Vector3 dir = destinationSetter.target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        }*/
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, destinationSetter.target.position) <= 0.2f)
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
                //rotate in direction of player
                Vector2 lookDir = (Vector2)fov.player.transform.position - rb.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = angle;
                
                //attack with ranged weapon
                weapon.Attack(transform.rotation.z);
            }
            else
            {
                GameObject playerLastSeen = Instantiate(playerLastSeenLocationPrefab, fov.player.transform.position, Quaternion.identity);
                Destroy(playerLastSeen, 7f);

                //get closer
                destinationSetter.target = playerLastSeen.transform;
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
                GameObject playerLastSeen = Instantiate(playerLastSeenLocationPrefab, fov.player.transform.position, Quaternion.identity);
                Destroy(playerLastSeen, 7f);

                //get closer
                destinationSetter.target = playerLastSeen.transform;
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
        destinationSetter.target = patrolWaypoints[waypointIndex];
    }

    /*public void TakeBullet()
    {
        Die();
    }
    
    public void GetHit()
    {
        Die();
    }*/
    
    public void ReceiveDamage(int damage, float rotation)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die(rotation);
        }
    }
    
    public void GetHitByFist(float rotation)
    {
        if (!isStunned)
        {
            beStunned(rotation);
            // ReceiveDamage(1);
        }
    }

    public void beStunned(float rotation)
    {
        stunCountdown = stunTimeout;
        
        if (weapon != null)
        {
            DropWeapon();
        }
        
        isStunned = true;
        SetSprite(EnemyState.stunned);
        RotateSprite(rotation);
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

    public virtual void Die(float rotation)
    {
        if (!isAlive)
        {
            return;
        }
        isAlive = false;
        // Debug.Log("Enemy is Dead");
        SetSprite(EnemyState.dead);
        //TOFIX magic numbers
        if (rotation != 1000f)
        {
            RotateSprite(rotation);
        }
        // spriteDead.transform.Rotate(0f, 0f, rotation);
        DisableCollider();
        if (weapon != null)
        {
            DropWeapon();
        }
        
        GameObject bloodSpot = Instantiate(bloodSpotPrefab, transform.position, Quaternion.identity);
        bloodSpot.GetComponent<BloodSpot>().PlayRandomAnimation();
        /*Animator animator = bloodSpot.GetComponent<Animator>();
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        animator.Play(animationClips[0].name);*/
    }

    private void RotateSprite(float rotation)
    {
        rb.rotation = rotation;
    }

    public void DisableCollider()
    {
        collider_.enabled = false;
    }

    public virtual void Voice()
    {
        
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

    public void DeathByThrownWeapon(float rotation)
    {
        Die(rotation);
    }

    public void GetFinished()
    {
        SoundManager.PlaySound(SoundManager.Sound.finisher);
        Die(1000f);
    }

    public void IncreaseFinisherCounter()
    {
        finisherCounter++;
        if (finisherCounter == finisherCounterMax)
        {
            Die(1000f);
            inFinisher = false;
        }
    }

    public void StartFinisher()
    {
        inFinisher = true;
    }
}
