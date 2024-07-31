using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class TruongAudioSourceAbstract : TruongMonoBehaviour
{
    [SerializeField] protected bool isActive;
    [SerializeField] protected string keyLocal;
    public bool IsActive => isActive;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] private TruongAudioClips audioClips;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        SetKeyLocal();
        SetIsActive(GetIsActiveVarFromLocal());
        LoadAudioSource();
        LoadAudioClips();
    }

    private void LoadAudioClips()
    {
        audioClips = GetComponentInChildrenOfFather<TruongAudioClips>();
    }

    private void LoadAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected abstract void SetKeyLocal();

    protected virtual void SetKeyLocal(string value)
    {
        this.keyLocal = value;
    }

    public void SetIsActive(bool value)
    {
        this.isActive = value;
        OnSetIsActive(value);
    }

    protected virtual void OnSetIsActive(bool value)
    {
        SetIsActiveVarToLocal(value);
    }

    private bool GetIsActiveVarFromLocal()
    {
        if (PlayerPrefs.HasKey(keyLocal)) return PlayerPrefs.GetInt(keyLocal) == 1;

        SetIsActive(true);
        return true;
    }

    private void SetIsActiveVarToLocal(bool value)
    {
        PlayerPrefs.SetInt(keyLocal, value ? 1 : 0);
    }

    [Button]
    public void Play(string clipName)
    {
        var clip = audioClips.GetAudioClip(clipName);
        Play(clip);
    }

    [Button]
    public void PlayWithAddressable(string audioClipPath)
    {
        if (audioClipPath.IsNullOrWhitespace()) return;
        Addressables.LoadAssetAsync<AudioClip>(audioClipPath).Completed += OnLoadCompleted;
    }

    [Button]
    public void Play(AudioClip clip)
    {
        if (!isActive) return;
        OnPlay(clip);
    }

    protected abstract void OnPlay(AudioClip clip);


    private void OnLoadCompleted(AsyncOperationHandle<AudioClip> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip audioClip = operation.Result;
            Play(audioClip);
            Release(audioClip);
            return;
        }

        Debug.LogError("Failed to load AudioClip: " + operation.OperationException);
    }


    protected abstract void Release(AudioClip audioClip);
}