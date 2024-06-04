using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundManager instance;
    
 public enum Sound
    {
        soundstrack,
        enemy_mumble, 
        fist_hit, 
        fist_miss,
        pickup_weapon,
        melee_hit,
        finisher,
        shoot,
        no_ammo
    }

    public static Dictionary<Sound, AudioClip> soundsDictionary = new Dictionary<Sound, AudioClip>();
    public SoundAudioClip[] soundAudioClips;
    
    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        foreach (SoundAudioClip soundAudioClip in soundAudioClips)
        {
            soundsDictionary.Add(soundAudioClip.sound, soundAudioClip.audioClip);
        }
    }

    public static void PlaySound(Sound sound)
    {
        // if (CanPlaySound(sound))
        {
            //TODO fixhere?
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(soundsDictionary[sound]);
        }
    }
    
    /*public static void PlayButtonSound()
    {
        // if (CanPlaySound(sound))
        {
            //TODO fixhere?
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(soundsDictionary[Sound.button_click]);
        }
    }*/

    public static void PlaySoundTrack()
    {
        GameObject soundGameObject = new GameObject("SoundTrack");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.clip = soundsDictionary[Sound.soundstrack];
        audioSource.Play();
    }
    
    public static GameObject PlayLoopedSound(Sound sound)
    {
        GameObject soundGameObject = new GameObject(sound.ToString());
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.clip = soundsDictionary[sound];
        audioSource.Play();
        return soundGameObject;
    }
}
