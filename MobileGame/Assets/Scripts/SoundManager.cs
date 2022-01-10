using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource Buff;
    [SerializeField]
    public AudioSource Throw;
    [SerializeField]
    AudioSource Hurt;
    [SerializeField]
    AudioSource LevelUp;
    [SerializeField]
    AudioSource ExpUp;

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

    public void PlayExpUp()
    {
        PlaySound(ExpUp);
    }
    public void PlayLvUp()
    {
        PlaySound(LevelUp);
    }

}
