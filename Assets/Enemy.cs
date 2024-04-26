using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    
    private bool isAlive = true;
    public GameObject spriteAlive;
    public GameObject spriteDead;
    
    public Transform[] patrolWaypoints;
    private int waypointIndex = 0;

    private Transform target;

    private void Start()
    {
        if (patrolWaypoints.Length != 0)
        {
            target = patrolWaypoints[0];
        }
    }


    // Update is called once per frame
    void Update()
    {
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

    private void Die()
    {
        isAlive = false;
        Debug.Log("Enemy is Dead");
        spriteAlive.SetActive(false);
        spriteDead.SetActive(true);
    }
}
