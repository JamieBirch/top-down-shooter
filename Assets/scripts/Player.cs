using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [FormerlySerializedAs("animator")] public Animator emptyHandsAnimator;
    public Animator pipeAnimator;
    public Animator shotgunAnimator;

    private Animator currentAnimator;
    
    public Rigidbody2D rb;
    [FormerlySerializedAs("collider")] public BoxCollider2D collider_;
    public GameObject bloodSpotPrefab;
    
    private bool isAlive = true;
    public GameObject spriteAlive;
    public GameObject spriteDead;
    public GameObject spritePipe;
    public GameObject spriteShotgun;
    
    public GameObject legs;

    public GameObject currentSprite;
    
    public GameObject deathCanvas;

    public float weaponPickupRange;
    public GameObject weaponSlot;

    private Weapon heldWeapon;
    
    public float fistsAttackRange;
    public float fistsAttackAngle;
    public float attackTimeout;
    public float attackCountdown = 0;
    
    public LayerMask enemyLayer;
    public LayerMask wallLayer;

    private bool activeHandRight = true;
    private Enemy finishingEnemy;

    private void Start()
    {
        currentSprite = spriteAlive;
        currentAnimator = emptyHandsAnimator;
    }

    void Update()
    {
        if (finishingEnemy != null)
        {
            if (!finishingEnemy.IsAlive())
            {
                finishingEnemy = null;
                return;
            }
            
            if (Input.GetButtonDown("Fire1"))
            {
                //hit with fist
                PunchEnemy(finishingEnemy, 0f);
                //increase counter
                finishingEnemy.IncreaseFinisherCounter();
            }
            else
            {
                return;
            }
        }
        
        if (Input.GetKey("r"))
        {
            ReloadLevel();
            /*if (isAlive)
            {
                return;
            }
            else
            {
                ReloadLevel();
            }*/
        }

        if (Input.GetKey("space"))
        {
            PlayFinisher();
        }

        if (!isAlive)
        {
            return;
        }
        
        if (Input.GetButtonDown("Fire1") && attackCountdown <= 0)
        {
            attackCountdown = attackTimeout;
            Attack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (StandingNearWeapon(out var weapon))
            {
                if (HoldsWeapon())
                {
                    ThrowWeapon();
                }
                PickupWeapon(weapon);
            }
            else
            {
                if (HoldsWeapon())
                {
                    ThrowWeapon();
                    ChangeSprite(spriteAlive);
                    currentAnimator = emptyHandsAnimator;
                    collider_.size = new Vector2(0.7f, 0.4f);
                    collider_.offset = new Vector2(0, 0);
                }
            }
        }

        attackCountdown -= Time.deltaTime;
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PickupWeapon(Weapon weapon)
    {
        if (weapon.IsHeld())
        {
            return;
        }
        heldWeapon = weapon;
        if (weapon.weaponType == WeaponType.pipe)
        {
            ChangeSprite(spritePipe);
            currentAnimator = pipeAnimator;
            collider_.size = weapon.colliderSize;
            collider_.offset = weapon.colliderOffset;
            /*collider.size = new Vector2(0.75f, 0.55f);
            collider.offset = new Vector2(0, 0.05f);*/
        } else if (weapon.weaponType == WeaponType.shotgun)
        {
            ChangeSprite(spriteShotgun);
            currentAnimator = shotgunAnimator;
            collider_.size = weapon.colliderSize;
            collider_.offset = weapon.colliderOffset;
            /*collider.size = new Vector2(0.7f, 1f);
            collider.offset = new Vector2(0, 0.4f);*/
        }
        
        
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
        if (HoldsWeapon())
        {
            heldWeapon.Attack(rb.rotation);
            
            if (activeHandRight)
            {
                currentAnimator.SetTrigger("hit enemy right");
                SwitchHands();
            }
            else
            {
                currentAnimator.SetTrigger("hit enemy left");
                SwitchHands();
            }
            // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            // rb.AddForce(-transform.forward * heldWeapon.weaponKickback, ForceMode2D.Impulse);
        }
        else
        {
            AttackWithFists();
        }
    }

    private void AttackWithFists()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, fistsAttackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            //currently for only 1 enemy
            Transform target = hitEnemies[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < fistsAttackAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallLayer))
                {
                    PunchEnemy(hitEnemies[0].GetComponent<Enemy>(), rb.rotation);
                    // canSeePlayer = true;
                }
                else
                {
                    PunchMiss();
                }
                
            }
            else
            {
                PunchMiss();
            }
        }
        else
        {
            PunchMiss();
        }
        
        /*foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Enemy>().GetHitByFist();
            Debug.Log("we hit " + hitEnemy.name + " with fists");
        }*/
    }

    private void PunchEnemy(Enemy hitEnemy, float rotation)
    {
        hitEnemy.GetComponent<Enemy>().GetHitByFist(rotation);
        SoundManager.PlaySound(SoundManager.Sound.fist_hit);
        if (activeHandRight)
        {
            emptyHandsAnimator.SetTrigger("punch enemy");
            SwitchHands();
        }
        else
        {
            emptyHandsAnimator.SetTrigger("punch enemy left");
            SwitchHands();
        }
    }
    
    private void PunchMiss()
    {
        SoundManager.PlaySound(SoundManager.Sound.fist_miss);
        
        if (activeHandRight)
        {
            emptyHandsAnimator.SetTrigger("punch miss");
            SwitchHands();
        }
        else
        {
            emptyHandsAnimator.SetTrigger("punch miss left");
            SwitchHands();
        }
    }

    private void PlayFinisher()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, fistsAttackRange, enemyLayer);
        if (hitEnemies.Length > 0)
        {
            Enemy enemy = hitEnemies[0].GetComponent<Enemy>();
            
            if (enemy.IsStunned())
            {
                if (HoldsWeapon())
                {
                    //quick finisher
                    //TODO play animation with weapon
                    
                    currentAnimator.SetTrigger("finisher");
                    enemy.GetFinished();
                }
                else
                {
                    //start long finisher
                    finishingEnemy = enemy;
                    StandOn(enemy);
                    enemy.StartFinisher();
                }
            }
        }
    }

    private void StandOn(Enemy enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        float enemyRbRotation = enemy.rb.rotation;
        gameObject.transform.position = enemyPosition;
        rb.rotation = enemyRbRotation;
        enemy.DisableCollider();
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
        
        legs.SetActive(false);
        ChangeSprite(spriteDead);
        
        deathCanvas.SetActive(true);
        
        GameObject bloodSpot = Instantiate(bloodSpotPrefab, transform.position, Quaternion.identity);
        bloodSpot.GetComponent<BloodSpot>().PlayRandomAnimation();
    }

    public bool IsDead()
    {
        return !isAlive;
    }

    public bool HoldsWeapon()
    {
        return heldWeapon != null;
    }

    public Weapon GetWeapon()
    {
        return heldWeapon;
    }

    public bool inFinisher()
    {
        return finishingEnemy != null;
    }

    private void ChangeSprite(GameObject newSprite)
    {
        currentSprite.SetActive(false);
        currentSprite = newSprite;
        currentSprite.SetActive(true);
    }
    
    private void SwitchHands()
    {
        if (activeHandRight)
        {
            activeHandRight = false;
        }
        else
        {
            activeHandRight = true;
        }
    }
}
