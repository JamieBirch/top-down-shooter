using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    public Animator animator;

    private Vector2 movement;
    private Vector2 mousePosition;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Player>().isDead())
        {
            return;
        }
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    
    void FixedUpdate()
    {
        if (GetComponent<Player>().isDead())
        {
            return;
        }
        
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (movement.magnitude > 0f)
        {
            animator.SetFloat("speed", 1f);
        }
        else
        {
            animator.SetFloat("speed", 0f);
        }

        Vector2 lookDir = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
