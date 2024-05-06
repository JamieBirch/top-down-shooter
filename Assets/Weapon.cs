using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Rigidbody2D rb;

    public float dropForce = 5f;
    
    public abstract void Attack();

    public void Drop()
    {
        rb.simulated = true;
        rb.AddForce(transform.up * dropForce, ForceMode2D.Impulse);
    }
}