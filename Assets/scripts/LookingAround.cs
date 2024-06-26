using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookingAround : MonoBehaviour
{
    public Enemy enemy;
    public float rotationSpeed;
    
    public Rigidbody2D rb;

    private bool propertiesSet = false;

    private float currentGoal;
    private float angle1;
    private float angle2;
    
    // Update is called once per frame
    void Update()
    {
        if (enemy.lookingAround)
        {
            Debug.Log(rb.rotation);

            if (propertiesSet)
            {
                if (Math.Abs(rb.rotation - currentGoal) < 0.01)
                {
                    SwitchGoals();
                }
                else
                {
                    Move();
                }

            }
            else
            {
                angle1 = rb.rotation + 45;
                angle2 = rb.rotation - 45;
                currentGoal = angle1;
                propertiesSet = true;
            }
        }
        else
        {
            propertiesSet = false;
            return;
        }
    }

    private void Move()
    {
        if (Math.Abs(currentGoal - angle1) < 0.1)
        {
            rotateTo(rb.rotation + rotationSpeed);
        }
        else if (Math.Abs(currentGoal - angle2) < 0.1)
        {
            rotateTo(rb.rotation - rotationSpeed);
        } 
    }

    private void SwitchGoals()
    {
        if (Math.Abs(currentGoal - angle1) < 0.01)
        {
            currentGoal = angle2;
        }
        else
        {
            currentGoal = angle1;
        }
    }

    private void rotateTo(float angle)
    {
        float targetAngle = angle/* - 90f*/;
        rb.rotation = targetAngle;
    }
}
