using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked;
    private bool isOpen = false;
    public Rigidbody2D rb;
    // public GameObject rotationPoint;
    

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            isLocked = false;
            /*rotationPoint.*/transform.Rotate(0f, 0f, 90f);
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject colGameObject = col.gameObject;
        if (colGameObject.TryGetComponent<Bullet>(out _) || colGameObject.CompareTag("Player"))
        {
            Open();
        }
    }

}
