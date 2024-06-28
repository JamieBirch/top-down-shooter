using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    public FieldOfView fov;

    private Animator currentAnimator;
    
    public GameObject spriteAlive;
    public GameObject spriteStunned;
    public GameObject spriteDead;
    public GameObject spritePipe;
    public GameObject spriteShotgun;
    
    private GameObject currentSprite = null;
    
    public Rigidbody2D rb;

    public AIDestinationSetter destinationSetter;
    public AIPath aipath;
    
    public bool Alert = false;
    
    public float speed;
    public float fistAttackRange;

    public int HP;
    private bool isAlive = true;
    private bool isStunned = false;

    [FormerlySerializedAs("collider")] public BoxCollider2D collider_;

    public GameObject weaponSlot;
    public GameObject weaponPrefab;
    public Weapon weapon;

    public float stunTimeout;
    public float stunCountdown;

    public float alertTimeout;
    public float alertCountdown;

    public LookingAround LookingAround;
    public bool lookingAround = false;
    
    public Transform[] patrolWaypoints;
    private int waypointIndex = 0;

    public GameObject playerLastSeenLocationPrefab;
    public GameObject defaultEnemyLocationPrefab;
    public Transform defaultPosition;

    public GameObject bloodSpotPrefab;
    private int finisherCounterMax = 3;
    private int finisherCounter = 0;
    private bool inFinisher = false;

    public float attackTimeout;
    public float attackCountdown = 0;
    
    private void Start()
    {
        currentSprite = spriteAlive;
        defaultPosition = Instantiate(defaultEnemyLocationPrefab, transform.position, Quaternion.identity).transform; 
        
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
            
            if (weapon.weaponType == WeaponType.pipe)
            {
                ChangeSprite(spritePipe);
                collider_.size = weapon.colliderSize;
                collider_.offset = weapon.colliderOffset;
            } else if (weapon.weaponType == WeaponType.shotgun)
            {
                ChangeSprite(spriteShotgun);
                collider_.size = weapon.colliderSize;
                collider_.offset = weapon.colliderOffset;
            }
        }
        else
        {
            ChangeSprite(spriteAlive);
        }
    }


    // Update is called once per frame
    void Update()
    {
        attackCountdown -= Time.deltaTime;
        
        /*if (HP <= 0)
        {
            Die();
        }*/
        
        if (!isAlive)
        {
            return;
        }

        if (isStunned)
        {
            if (stunCountdown <= 0)
            {
                isStunned = false;
                ChangeSprite(spriteAlive);
                EnableMonement();
            }
            if (!inFinisher)
            {
                stunCountdown -= Time.deltaTime;
                return;
            }
            else
            {
                return;
            }
        }
        
        

        if (fov.canSeePlayer && !fov.player.GetComponent<Player>().IsDead())
        {
            lookingAround = false;
            Alert = true;
            alertCountdown = alertTimeout;
            
            //rotate in direction of player
            Vector2 lookDir = (Vector2)fov.player.transform.position - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rotateTo(angle);

            //run to player/shoot player
            AttackPlayer();
        }
        else
        {
            if (Alert)
            {
                //run to last location of player
                if (destinationSetter.target != null)
                {
                    //if already there - look around (rotate)
                    if (Vector2.Distance(destinationSetter.target.position, transform.position) <= 0.2)
                    {
                        if (lookingAround)
                        {
                        
                        }
                        else
                        {
                            lookingAround = true;
                        }
                        alertCountdown -= Time.deltaTime;
                    }
                }
                else
                {
                    if (lookingAround)
                    {
                        
                    }
                    else
                    {
                        lookingAround = true;
                    }
                    alertCountdown -= Time.deltaTime;
                }
                
            }
            
            if (Alert && alertCountdown <= 0)
            {
                Alert = false;
                destinationSetter.target = defaultPosition;
                lookingAround = false;
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

    private void EnableMonement()
    {
        Debug.Log("enable movement");
        if (aipath != null)
        {
            aipath.enabled = true;
        }
    }

    private void rotateTo(float angle)
    {
        float targetAngle = angle - 90f;
        rb.rotation = targetAngle;
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
            if (weapon.attackRange > distanceToPlayer && attackCountdown <= 0)
            {
                //attack with ranged weapon
                attackCountdown = attackTimeout;
                currentAnimator.SetTrigger("attack");
                weapon.Attack(rb.rotation);
            }
            else
            {
                GameObject playerLastSeen = Instantiate(playerLastSeenLocationPrefab, fov.player.transform.position, Quaternion.identity);
                Destroy(playerLastSeen, 30f);

                //get closer
                destinationSetter.target = playerLastSeen.transform;
            }
        }
        else
        {
            //attack with fists
            if (distanceToPlayer <= fistAttackRange && attackCountdown <= 0)
            {
                attackCountdown = attackTimeout;
                FistAttack();
            }
            else
            {
                GameObject playerLastSeen = Instantiate(playerLastSeenLocationPrefab, fov.player.transform.position, Quaternion.identity);
                Destroy(playerLastSeen, 30f);

                //get closer
                destinationSetter.target = playerLastSeen.transform;
            }
        }
    }

    private void FistAttack()
    {
        currentAnimator.SetTrigger("punch");
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
        DisableMovement();
        
        if (weapon != null)
        {
            DropWeapon();
        }
        
        isStunned = true;
        ChangeSprite(spriteStunned);
        RotateSprite(rotation);
    }

    private void DisableMovement()
    {
        Debug.Log("disable movement");
        if (aipath != null)
        {
            aipath.enabled = false;
        }
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
        DisableMovement();
        // Debug.Log("Enemy is Dead");
        ChangeSprite(spriteDead);
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
    
    private void ChangeSprite(GameObject newSprite)
    {
        if (currentSprite != null)
        {
            currentSprite.SetActive(false);
        }
        currentSprite = newSprite;
        currentSprite.SetActive(true);
        if (currentSprite.TryGetComponent(out Animator anim_))
        {
            currentAnimator = anim_;
        }
        else
        {
            currentAnimator = null;
        }
    }
}
