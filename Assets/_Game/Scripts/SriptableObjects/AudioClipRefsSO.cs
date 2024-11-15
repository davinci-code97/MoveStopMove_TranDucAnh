using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    [Header("Music")]
    public AudioClip[] menu;
    public AudioClip[] play;
    public AudioClip[] win;
    public AudioClip[] lose;

    [Header("SFX")]
    public AudioClip[] btnClick;
    public AudioClip[] shoot;
    public AudioClip[] hit;
    public AudioClip[] dead;
    public AudioClip[] upsize;


}
