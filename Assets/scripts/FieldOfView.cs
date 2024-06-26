using System;
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(1, 360)]public float angle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    
    public bool canSeePlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        // float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length > 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            // Debug.Log(directionToTarget);

            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
                
            }
            else
            {
                canSeePlayer = false;
            }
        } else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/
    
    /*void OnDrawGizmosSelected()
    {
        // float angle = 30.0f;
        // float rayRange = 10.0f;
        // float halfFOV = angle / 2.0f;
        // float coneDirection = 180;

        Quaternion upRayRotation = Quaternion.AngleAxis(-angle / 2 /*+ Vector2.Angle(transform.up, directionToTarget)#1#, Vector2.up);
        Quaternion downRayRotation = Quaternion.AngleAxis(angle / 2 /*+ Vector2.Angle(transform.up, directionToTarget)#1#, Vector2.up);

        Vector2 upRayDirection = upRayRotation * transform.right * radius;
        Vector2 downRayDirection = downRayRotation * transform.right * radius;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);
        Gizmos.DrawLine((Vector2)transform.position + downRayDirection, (Vector2)transform.position + upRayDirection);
    }*/
}
