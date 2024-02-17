using System.Threading.Tasks;
using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public static AudioManagerController instance;
    
    public GameObject prefabAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySfx(string audioName, bool loop = false, float pitch = 1, float volume = 1)
    {
        PlayAudio(GetSfxByName(audioName), loop, pitch, volume);
    }

    private void PlayAudio(AudioClip audioClip, bool loop, float pitch, float volume)
    {
        if (audioClip == null) return;

        AudioSource audioSource = Instantiate(prefabAudioSource, transform).GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.loop = loop;

        audioSource.Play();

        audioSource.pitch = pitch;
        audioSource.volume = volume;

        DestroyAudioSource(audioSource);
    }

    private AudioClip GetSfxByName(string nameAudio)
    {
        AudioClip[] audios = UnityEngine.Resources.LoadAll<AudioClip>("Audios/SFX");

        foreach (var currentAudio in audios)
            if (currentAudio.name.Equals(nameAudio)) return currentAudio;

        Debug.Log("Audio "+nameAudio+ " Not Found");

        return null;
    }

    private async void DestroyAudioSource(AudioSource audioSource)
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
