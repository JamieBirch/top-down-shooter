using System;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public Player player;
    public Text text;
    public GameObject bulletsPanel;


    private void Update()
    {
        Weapon playerHeldWeapon = player.heldWeapon;
        if (playerHeldWeapon != null)
        {
            if (playerHeldWeapon.GetBulletCount() >= 0)
            {
                bulletsPanel.SetActive(true);
                text.text = playerHeldWeapon.GetBulletCount().ToString();
            }
            else
            {
                bulletsPanel.SetActive(false);
            }
        }
        else
        {
            bulletsPanel.SetActive(false);
        }
    }
}
