using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruongSFX : TruongAudioSourceAbstract
{
    protected override void SetKeyLocal()
    {
        SetKeyLocal("TruongSFX");
    }

    protected override void OnPlay(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    protected override void Release(AudioClip audioClip)
    {
    }
}