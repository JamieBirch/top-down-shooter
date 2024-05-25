using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    public Rigidbody2D rb;

    public void Open()
    {
        isLocked = false;
    }

    private void Update()
    {
        if (isLocked)
        {
            rb.mass = 1000f;
        }
        else
        {
            rb.mass = 0.5f;
        }
    }
}
