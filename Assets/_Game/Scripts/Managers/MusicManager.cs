using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Character;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private AudioSource audioSource;

    void Start() {
        //audioSource.volume = UserDataManager.Instance.userData.musicVolume;
        Player.Instance.OnCharacterDead += Player_OnCharacterDead;
        PlayMenuMusic();
    }

    private void Player_OnCharacterDead(object sender, OnCharacterDeadEventArgs e) {
        PlayLoseMusic();
    }

    private void PlayMusic(AudioClip audioClip, float volumeMultiplier = 1f) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void PlayMusic(AudioClip[] audioClipArray, float volume = 1f) {
        PlayMusic(audioClipArray[Random.Range(0, audioClipArray.Length)], volume);
    }

    public void PlayMenuMusic() {
        PlayMusic(audioClipRefsSO.menu);
    }

    public void PlayGameplayMusic() {
        PlayMusic(audioClipRefsSO.play);
    }

    public void PlayWinMusic() {
        PlayMusic(audioClipRefsSO.win);
    }

    public void PlayLoseMusic() {
        PlayMusic(audioClipRefsSO.lose);
    }

}
