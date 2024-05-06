using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Rigidbody2D rb;
    public float dropForce = 5f;

    private bool isHeld = false;
    
    public abstract void Attack();

    public void Throw()
    {
        rb.simulated = true;
        rb.AddForce(transform.up * dropForce, ForceMode2D.Impulse);
        isHeld = false;
    }
    
    public void Drop()
    {
        rb.simulated = true;
        isHeld = false;
    }

    public void PickUp()
    {
        isHeld = true;
    }
    
    public bool IsHeld()
    {
        return isHeld;
    }
    
}