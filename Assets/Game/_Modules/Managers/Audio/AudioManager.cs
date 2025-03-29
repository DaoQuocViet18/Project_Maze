using UnityEngine;
using System.Threading.Tasks;

public enum GameAudioClip
{
    GAMEOVER_SOUND,
    POP,
    WOA,
    COLLECT,
    BACKGROUND_MUSIC,
    REWARD_SOUND,
    POP_SOUND_EFFECT,
}

public class AudioManager : Singleton<AudioManager> 
{
    public AudioSource SoundSource;
    private bool isMusicEnabled = true;
    
    public bool IsMusicEnabled
    {
        get => isMusicEnabled;
        set
        {
            isMusicEnabled = value;
            if (!value)
                SoundSource.Stop();
            else if (SoundSource.clip != null)
                SoundSource.Play();
        }
    }

    private void Awake()
    {
        SoundSource.playOnAwake = false;
    }

    private AudioClip LoadAudioClip(GameAudioClip clip)
    {
        string filename = clip.ToString().ToLower();
        var audioClip = Resources.Load<AudioClip>($"Sounds/{filename}");
        
        if (audioClip == null)
            Debug.LogError($"AudioManager: Could not load audio clip {filename}");
            
        return audioClip;
    }

    private AudioSource ConfigureAudioSource(AudioSource source, AudioClip clip, float volumeDb, float pitch = 1f)
    {
        source.clip = clip;
        source.pitch = pitch;
        source.volume = DBToLinear(volumeDb);
        return source;
    }

    public void PlayMusic(GameAudioClip clip, float volumeDb = 0f)
    {
        var audioClip = LoadAudioClip(clip);
        if (audioClip == null) return;

        ConfigureAudioSource(SoundSource, audioClip, volumeDb);
        
        if (IsMusicEnabled)
            SoundSource.Play();
    }

    public async void PlaySound(GameAudioClip clip, float volumeDb = 0f, float pitch = 1f)
    {
        var audioClip = LoadAudioClip(clip);
        if (audioClip == null) return;

        var audioSource = SoundSource;
        ConfigureAudioSource(audioSource, audioClip, volumeDb, pitch);
        audioSource.Play();

        await Task.Delay((int)(audioClip.length * 1000));
    }

    public void PlaySoundWithRandomPitch(GameAudioClip clip, float volumeDb = 0f, float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        float randomPitch = Random.Range(minPitch, maxPitch);
        PlaySound(clip, volumeDb, randomPitch);
    }

    public async void StopMusic(bool fade = true, float duration = 2f)
    {
        if (!SoundSource.isPlaying) return;

        if (!fade)
        {
            SoundSource.Stop();
            return;
        }

        float startVolume = SoundSource.volume;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            SoundSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            await Task.Yield();
        }

        SoundSource.Stop();
    }

    public void SetMusicPitch(float pitch)
    {
        SoundSource.pitch = pitch;
    }

    public async void PlayMusicWithFadeOut(GameAudioClip music, float fadeTime)
    {
        StopMusic(true, fadeTime);
        await Task.Delay((int)(fadeTime * 500));
        PlayMusic(music);
    }

    private float DBToLinear(float dB) => Mathf.Pow(10f, dB / 20f);
}