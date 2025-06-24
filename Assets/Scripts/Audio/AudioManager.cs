using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Audio[] musicTracks, sfxSounds;
    [SerializeField] private AudioSource musicSrc, sfxSrc;

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Audio music = Array.Find(musicTracks, track => track.name == name);
        if (music != null)
        {
            musicSrc.clip = music.clip;
            musicSrc.Play();
        }
        else
        {
            Debug.LogWarning("Music track not found: " + name);
        }
    }

    public void StopMusic() {
        if (musicSrc.isPlaying)
        {
            musicSrc.Stop();
        }
    }
    public void PlaySFX(string name)
    {
        Audio sfx = Array.Find(sfxSounds, sound => sound.name == name);
        if (sfx != null)
        {
            sfxSrc.clip = sfx.clip;
            sfxSrc.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning("SFX sound not found: " + name);
        }
    }
    public void StopSFX()
    {
        if (sfxSrc.isPlaying)
        {
            sfxSrc.Stop();
        }
    }
    public void ToggleMusic()
    {
        musicSrc.mute = !musicSrc.mute;
    }
    public void ToggleSfx()
    {
        sfxSrc.mute = !sfxSrc.mute;
    }
    public float GetMusicVolume()
    {
        return musicSrc.volume;
    }
    public float GetSfxVolume()
    {
        return sfxSrc.volume;
    }
    public void SetMusicVolume(float volume)
    {
        musicSrc.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxSrc.volume = volume;
    }
}
