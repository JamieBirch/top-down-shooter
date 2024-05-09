using System;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public Player player;
    public Text text;


    private void Update()
    {
        Weapon playerHeldWeapon = player.heldWeapon;
        if (playerHeldWeapon != null)
        {
            text.text = playerHeldWeapon.GetBulletCount().ToString();
        }
        else
        {
            text.text = "-";
        }
    }
}
