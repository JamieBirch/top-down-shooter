using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Door[] doors;
    
    public GameObject pipe;
    public GameObject gunshot;

    public Enemy[] enemies;
    
    public Player player;
    public Text text;
    public int currentStep = 0;
    
    public Dictionary<int, string> instructions = new Dictionary<int, string>();

    public int movementCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        instructions.Add(0, "WASD to move");
        instructions.Add(1, "врежь ему!");
        instructions.Add(2, "гаси бритого!");
        instructions.Add(3, "Хватай железку!");
        instructions.Add(4, "Врежь козлу!");
        instructions.Add(5, "Хватай ствол");
        instructions.Add(6, "Время пострелять!");
        instructions.Add(7, "");
        instructions.Add(8, "");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStep)
        {
            case 0:
            {
                doors[0].Open();
                CheckIfMoved();
                break;
            }
            case 1:
            {
                CheckIfPunched();
                break;
            }
            case 2:
            {
                CheckIfKilled(3);
                break;
            }
            case 3:
            {
                doors[1].Open();
                CheckIfTookPipe();
                break;
            }
            case 4:
            {
                CheckIfHitWithPipe();
                break;
            }
            case 5:
            {
                CheckIfKilled(2);
                break;
            }
            case 6:
            {
                doors[2].Open();
                CheckIfTookGunshot();
                break;
            }
            case 7:
            {
                CheckIfKilled(1);
                break;
            }
            case 8:
            {
                Debug.Log("tutorial finished?");
                break;
            }
        }

        text.text = instructions[currentStep];
    }

    private void CheckIfKilled(int num)
    {
        int count = enemies.Count(enemy => enemy.IsAlive());
        if (count < num)
        {
            currentStep++;
        }
    }

    private void CheckIfHitWithPipe()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentStep++;
        }
    }

    private void CheckIfPunched()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentStep++;
        }
    }

    private void CheckIfMoved()
    {
        if (Input.GetKey("w") || Input.GetKey("s")|| Input.GetKey("a") || Input.GetKey("d"))
        {
            movementCounter++;
        }

        if (movementCounter >= 4)
        {
            currentStep++;
        }
    }


    private void CheckIfTookGunshot()
    {
        if (player.heldWeapon != null)
        {
            if (player.heldWeapon.gameObject == gunshot)
            {
                currentStep++;
            }
        }
    }

    private void CheckIfTookPipe()
    {
        if (player.heldWeapon != null)
        {
            if (player.heldWeapon.gameObject == pipe)
            {
                currentStep++;
            }
        }
    }
}