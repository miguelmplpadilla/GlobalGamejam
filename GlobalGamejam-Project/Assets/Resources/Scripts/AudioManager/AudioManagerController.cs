using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public static AudioManagerController instance;
    
    public GameObject prefabAudioSource;

    public AudioSource audioSourceMusic;

    private void Awake()
    {
        instance = this;
    }

    public async Task<GameObject> PlaySfx(string audioName, bool loop = false, float pitch = 1, float volume = 1, Action callback = null)
    {
        return await PlayAudio(GetSfxByName(audioName), loop, pitch, volume, callbackEnd:callback);
    }

    public async Task<GameObject> PlayMusic(string audioClip)
    {
        if (audioSourceMusic != null)
        {
            await audioSourceMusic.DOFade(0, 1.5f).AsyncWaitForCompletion();
            Destroy(audioSourceMusic.gameObject);
        }
        
        audioSourceMusic = Instantiate(prefabAudioSource, transform).GetComponent<AudioSource>();

        audioSourceMusic.volume = 0;

        audioSourceMusic.DOFade(1, 1.5f);
        
        audioSourceMusic.clip = GetSfxByName(audioClip);
        audioSourceMusic.loop = true;

        return audioSourceMusic.gameObject;
    }

    private async Task<GameObject> PlayAudio(AudioClip audioClip, bool loop, float pitch, float volume, Action callbackEnd = null)
    {
        if (audioClip == null) return null;

        AudioSource audioSource = Instantiate(prefabAudioSource, transform).GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.loop = loop;

        audioSource.Play();

        audioSource.pitch = pitch;
        audioSource.volume = volume;

        if (loop == false)
        {
            await DestroyAudioSource(audioSource);
            callbackEnd?.Invoke();
        }
        else return audioSource.gameObject;

        return null;
    }

    private AudioClip GetSfxByName(string nameAudio)
    {
        AudioClip[] audios = UnityEngine.Resources.LoadAll<AudioClip>("Audios");

        foreach (var currentAudio in audios)
            if (currentAudio.name.Equals(nameAudio)) return currentAudio;

        Debug.Log("Audio "+nameAudio+ " Not Found");

        return null;
    }

    private async Task DestroyAudioSource(AudioSource audioSource)
    {
        while (true)
        {
            if (audioSource == null) return;
            if (!audioSource.isPlaying)
            {
                Destroy(audioSource.gameObject);
                return;
            }

            await Task.Delay(100);
        }
    }
}
