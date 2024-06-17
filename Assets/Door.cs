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
        rb.bodyType = RigidbodyType2D.Static;
    }

}
