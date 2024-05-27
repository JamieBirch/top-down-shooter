using System;
using UnityEngine;

public class TutorialEnemy : Enemy
{
    public GameObject soundObject;
    
    public override void Voice()
    {
        soundObject = SoundManager.PlayLoopedSound(SoundManager.Sound.enemy_mumble);
    }

    public override void AttackPlayer()
    {
        return;
    }

    public override void Die()
    {
        base.Die();
        Destroy(soundObject);
    }
}
