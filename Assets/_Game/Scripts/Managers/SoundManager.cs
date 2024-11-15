using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Character;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private AudioSource audioSource;


    void Start()
    {
        //AudioSource.volume = UserDataManager.Instance.userData.sfxVolume;
    }

    private void PlaySound(AudioClip audioClip, Vector3 position) {
        AudioSource.PlayClipAtPoint(audioClip, position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position);
    }

    public void PlayButtonClickSFX() {
        audioSource.PlayOneShot(audioClipRefsSO.btnClick[Random.Range(0, audioClipRefsSO.btnClick.Length)]);
    }

    public void PlayGrowSizeSFX(Vector3 position) {
        PlaySound(audioClipRefsSO.upsize, position);
    }

    public void PlayShootSFX(Vector3 position) {
        PlaySound(audioClipRefsSO.shoot, position);
    }

    public void PlayHitSFX(Vector3 position) {
        PlaySound(audioClipRefsSO.hit, position);
    }

    public void PlayDeadSFX(Vector3 position) {
        PlaySound(audioClipRefsSO.dead, position);
    }

}
