using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public Rigidbody2D rb;
    public GameObject rotationPoint;

    public void Open()
    {
        isLocked = false;
        rotationPoint.transform.Rotate(0f, 0f, 90f);
    }

    private void Update()
    {
        if (isLocked)
        {
            rb.mass = 1000f;
        }
        else
        {
            rb.mass = 5f;
        }
    }
}
