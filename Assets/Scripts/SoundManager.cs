using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    static AudioClip clip1;
    static AudioClip clip2;
    static AudioClip clip3;
    static AudioClip explosion1;
    static AudioClip explosion2;
    static AudioClip explosion3;

    public static void Init()
    {
        clip1 = Resources.Load<AudioClip>("Audio/collision1");
        clip2 = Resources.Load<AudioClip>("Audio/collision2");
        clip3 = Resources.Load<AudioClip>("Audio/collision3");

        explosion1 = Resources.Load<AudioClip>("Audio/explosion1");
        explosion2 = Resources.Load<AudioClip>("Audio/explosion2");
        explosion3 = Resources.Load<AudioClip>("Audio/explosion3");
    }

    private static void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        if (clip != null)
        {
            Object.Destroy(soundGameObject, clip.length);
        }
    }

    public static void PlayCollisionSound(Vector3 position, float volume = 0.5f)
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                PlaySound(clip1, position, volume);
                break;
            case 1:
                Debug.Log(clip2);
                PlaySound(clip2, position, volume);
                break;
            case 2:
                PlaySound(clip3, position, volume);
                break;
        }
    }
    public static void PlayExplosionSound(Vector3 position, float volume = 0.7f)
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                PlaySound(explosion1, position, volume);
                break;
            case 1:
                PlaySound(explosion2, position, volume);
                break;
            case 2:
                PlaySound(explosion3, position, volume);
                break;
        }
    }
}
