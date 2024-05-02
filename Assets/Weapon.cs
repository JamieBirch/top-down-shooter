using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // public float dropDistance;
    public float dropForce = 20f;
    
    public abstract void Attack();

    public void Drop()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * dropForce, ForceMode2D.Impulse);
    }
}