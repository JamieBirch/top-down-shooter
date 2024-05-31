using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public GameObject bloodSpotPrefab;
    
    private bool isAlive = true;
    public GameObject spriteAlive;
    public GameObject spriteDead;
    
    public GameObject deathCanvas;

    public float weaponPickupRange;
    public GameObject weaponSlot;

    private Weapon heldWeapon;
    
    public float fistsAttackRange;
    public float fistsAttackAngle;
    public LayerMask enemyLayer;
    public LayerMask wallLayer;

    private bool activeHandRight = true;
    private Enemy finishingEnemy;

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
                PunchEnemy(finishingEnemy);
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
        
        if (Input.GetButtonDown("Fire1"))
        {
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
                }
            }
        }
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
            heldWeapon.Attack();
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
                    PunchEnemy(hitEnemies[0].GetComponent<Enemy>());
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

    private void PunchEnemy(Enemy hitEnemy)
    {
        
        hitEnemy.GetComponent<Enemy>().GetHitByFist();
        SoundManager.PlaySound(SoundManager.Sound.fist_hit);
        if (activeHandRight)
        {
            animator.SetTrigger("punch enemy");
            activeHandRight = false;
        }
        else
        {
            animator.SetTrigger("punch enemy left");
            activeHandRight = true;
        }
    }
    
    private void PunchMiss()
    {
        SoundManager.PlaySound(SoundManager.Sound.fist_miss);
        
        if (activeHandRight)
        {
            animator.SetTrigger("punch miss");
            activeHandRight = false;
        }
        else
        {
            animator.SetTrigger("punch miss left");
            activeHandRight = true;
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
                    // animator.SetTrigger(heldWeapon.GetFinisherName());
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
        spriteAlive.SetActive(false);
        spriteDead.SetActive(true);
        
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
}
