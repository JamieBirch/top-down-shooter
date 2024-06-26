using System;
using UnityEngine;
using Random = System.Random;

public class BloodSpot : MonoBehaviour
{
    public Animator animator;
    public AnimationClip[] animationClips;

    public void PlayRandomAnimation()
    {
        int index = new Random().Next(animationClips.Length - 1);
        animator.Play(animationClips[index].name);
    }

}
