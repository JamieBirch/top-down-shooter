using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public CameraFollow CameraFollow;
    
    public Door[] doors;
    
    public GameObject pipe;
    public GameObject shotgun;

    public Enemy[] enemies;
    
    public Player player;
    public Text text;
    public int currentStep = 0;
    
    public Dictionary<int, string> instructions = new Dictionary<int, string>();

    public int movementCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySoundTrack();
        
        instructions.Add(0, "WASD to move");
        instructions.Add(1, "врежь ему!");
        instructions.Add(2, "гаси бритого!");
        instructions.Add(3, "Хватай железку!");
        instructions.Add(4, "Врежь козлу!");
        instructions.Add(5, "Хватай ствол");
        instructions.Add(6, "Время пострелять!");
        instructions.Add(7, "");
        instructions.Add(8, "");
        
        PerformOneTimeActions();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStep)
        {
            case 0:
            {
                // doors[0].Open();
                // enemies[0].Voice();
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
                // doors[1].Open();
                // enemies[1].Voice();
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
                // doors[2].Open();
                // enemies[2].Voice();
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
            NextStep();
        }
    }

    private void CheckIfHitWithPipe()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            NextStep();
        }
    }

    private void CheckIfPunched()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            NextStep();
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
            NextStep();
        }
    }


    private void CheckIfTookGunshot()
    {
        if (player.heldWeapon != null)
        {
            if (player.heldWeapon.gameObject == shotgun)
            {
                NextStep();
            }
        }
    }

    private void CheckIfTookPipe()
    {
        if (player.heldWeapon != null)
        {
            if (player.heldWeapon.gameObject == pipe)
            {
                NextStep();
            }
        }
    }

    private void PerformOneTimeActions()
    {
        switch (currentStep)
        {
            case 0:
            {
                break;
            }
            case 1:
            {
                StartCoroutine(ShowObject(enemies[0].transform));
                doors[0].Open();
                enemies[0].Voice();
                break;
            }
            case 2:
            {
                break;
            }
            case 3:
            {
                StartCoroutine(ShowObject(pipe.transform));
                doors[1].Open();
                enemies[1].Voice();
                break;
            }
            case 4:
            {
                break;
            }
            case 5:
            {
                StartCoroutine(ShowObject(shotgun.transform));
                break;
            }
            case 6:
            {
                doors[2].Open();
                enemies[2].Voice();
                break;
            }
            case 7:
            {
                break;
            }
            case 8:
            {
                Debug.Log("tutorial finished?");
                break;
            }
        }
    }

    private void NextStep()
    {
        currentStep++;
        PerformOneTimeActions();
    }

    IEnumerator ShowObject(Transform go)
    {
        Transform prevTarget = CameraFollow.target;
        CameraFollow.target = go;
        yield return new WaitForSeconds(2);
        CameraFollow.target = prevTarget;
    }
}
