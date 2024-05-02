using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D playerRB;

    public float dropForce = 5f;
    
    public abstract void Attack();

    public void Drop()
    {
        rb.AddForce(transform.up * dropForce, ForceMode2D.Impulse);
        playerRB = null;
    }
    
    void FixedUpdate()
    {
        if (transform.parent != null)
        {
            rb.MovePosition((transform.parent.position));
            rb.rotation = playerRB.rotation;
        }
    }

    public void PickUp()
    {
        playerRB = transform.parent.GetComponent<WeaponSlot>().playerRB;
    }
}