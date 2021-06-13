using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    [SerializeField]
    private GameObject audioSource;
    // Start is called before the first frame update
    public static AudioManager instance;
    private string currentMusic;
    private string currentAmbient;
    [SerializeField]
    private AudioSource musicAS;
    [SerializeField]
    private AudioSource ambientAS;
    [SerializeField]
    private AudioSource naratorAS;
    


    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            PlayMusic("_yootiful");
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = false;
            }
        }
        else
        {
            instance.PlayMusic("_yootiful");
            Destroy(gameObject);
            return;
        }

       
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("Cannot find sound named " + name);
            return;
        }
        naratorAS.clip = s.clip;
        naratorAS.volume = s.volume;
        naratorAS.Play();
    }
    public void PlayMusic(string name)
    {
        if(currentMusic != name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            currentMusic = name;
        }        
    }
    
    public void PlayAmbient(string name)
    {
        if(currentAmbient != name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            currentMusic = name;
        }        
    }
    
    public void PlayNaration()
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        Play("N" + SceneManager.GetActiveScene().buildIndex.ToString());
    }
}
