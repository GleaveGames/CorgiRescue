using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Buff;
    public AudioSource Throw;
    public AudioSource Hurt;

    private void PlaySound(AudioSource source)
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
    }

    public void PlayBuff()
    {
        PlaySound(Buff);
    }

    public void PlayThrow()
    {
        PlaySound(Throw);
    }
    public void PlayHurt()
    {
        PlaySound(Hurt);
    }

}
