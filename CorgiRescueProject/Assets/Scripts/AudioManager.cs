using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    [SerializeField]
    private GameObject audioSource;
    // Start is called before the first frame update
    public static AudioManager instance;
    private string currentMusic;
    private AudioSource musicAS;
    



    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            if(FindObjectOfType<UI>() != null)
            {
                FindObjectOfType<UI>().am = this;
            }
            PlayMusic("DirtTheme");
        }
        else
        {
            FindObjectOfType<UI>().am = instance;
            instance.PlayMusic("DirtTheme");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Update is called once per frame
    public void Play(string name, Vector3 pos)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("Cannot find sound named " + name);
            return;
        }
        GameObject newSound = Instantiate(audioSource, pos, Quaternion.identity);        
        newSound.GetComponent<AudioSource>().clip = s.clip;
        newSound.GetComponent<AudioSource>().volume = s.volume;
        newSound.GetComponent<AudioSource>().pitch += UnityEngine.Random.Range(-0.2f, 0.2f);
        newSound.GetComponent<AudioSource>().Play();
    }
    public void PlayMusic(string name)
    {
        if(currentMusic != name)
        {
            musicAS = GetComponent<AudioSource>();
            Sound s = Array.Find(sounds, sound => sound.name == name);
            StartCoroutine(FadeAudioSource.StartFade(musicAS, 1, s.volume, s.clip));
            currentMusic = name;
        }        
    }
}
