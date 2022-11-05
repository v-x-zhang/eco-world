using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    public bool isGame;
    public bool fadeOut;

    public AudioMixer mixer;

    public AudioSource treeDestroySFX;
    public AudioSource buildingPlaceSFX;
    public AudioSource buildingDemolishSFX;

    [Header("Game Music")]
    public AudioSource[] music;
    public float gameMusicDelay;

    [Header("Menu")]
    public AudioSource menuMusic;
    public float menuMusicDelay;

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
    }

    private void Start()
    {
        if (isGame)
        {
            StartCoroutine(GameMusicLoop());
        }
        else
        {
            StartCoroutine(MenuMusicLoop());
        }
    }

    IEnumerator MenuMusicLoop()
    {
        float timer = menuMusic.clip.length + menuMusicDelay;
        while (!isGame && !fadeOut)
        {
            timer += Time.deltaTime;
            if(timer > menuMusic.clip.length + menuMusicDelay)
            {
                menuMusic.PlayOneShot(menuMusic.clip);
                timer = 0;
            }
            yield return null;
        }
        float startVolume = menuMusic.volume;

        while (menuMusic.volume > 0)
        {
            menuMusic.volume -= startVolume * Time.deltaTime / .5f;

            yield return null;
        }

    }

    IEnumerator GameMusicLoop()
    {
        int counter = Random.Range(0,music.Length -1);
        float timer = music[counter].clip.length + gameMusicDelay;
        while(isGame && !fadeOut)
        {
            timer += Time.deltaTime;
            if (timer > music[counter].clip.length + gameMusicDelay)
            {
                music[counter].PlayOneShot(music[counter].clip);
                timer = 0;
                counter++;
                if (counter > music.Length)
                {
                    counter = 0;
                }
            }
            yield return null;
        }
        float startVolume = music[counter].volume;

        while (music[counter].volume > 0)
        {
            menuMusic.volume -= startVolume * Time.deltaTime / .5f;

            yield return null;
        }
    }
}
