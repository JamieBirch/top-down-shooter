using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isAlive = true;
    public GameObject spriteAlive;
    public GameObject spriteDead;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeBullet()
    {
        Die();
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log("Enemy is Dead");
        spriteAlive.SetActive(false);
        spriteDead.SetActive(true);
    }
}
