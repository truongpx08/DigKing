using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class TruongBGMAbstract : TruongAudioSourceAbstract
{
    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        audioSource.loop = true;
    }

    protected override void SetKeyLocal()
    {
        SetKeyLocal("TruongBGM");
    }

    protected override void OnPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    protected override void Release(AudioClip audioClip)
    {
        Addressables.Release(audioClip);
    }

    protected override void OnSetIsActive(bool value)
    {
        base.OnSetIsActive(value);
        ResetPlay();
    }


    private void ResetPlay()
    {
        if (IsActive)
        {
            UnPause();
            Replay();
            return;
        }

        Pause();
    }

    private void Replay()
    {
        if (audioSource.isPlaying) return;
        OnReplay();
    }

    /// <summary>
    /// Play any song here because after unpausing there may be no songs previously playing
    /// </summary>
    protected abstract void OnReplay();

    [Button]
    private void UnPause()
    {
        audioSource.UnPause();
    }

    [Button]
    public void Pause()
    {
        audioSource.Pause();
    }
}