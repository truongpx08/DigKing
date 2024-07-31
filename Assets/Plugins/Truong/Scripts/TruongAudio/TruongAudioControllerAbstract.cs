using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class TruongAudioControllerAbstract<T> : TruongSingleton<T>
{
    [SerializeField] private TruongBGM bgmSource;
    public TruongBGM BgmSource => bgmSource;
    [SerializeField] private TruongSFX sfxSource;
    public TruongSFX SfxSource => sfxSource;

    protected override void CreateChildren()
    {
        base.CreateChildren();
        CreateChild(TruongConstant.BGM)?.AddComponent<TruongBGM>().gameObject.AddComponent<AudioSource>();
        CreateChild(TruongConstant.SFX)?.AddComponent<TruongSFX>().gameObject.AddComponent<AudioSource>();
        CreateChild(TruongConstant.AUDIO_CLIPS)?.AddComponent<TruongAudioClips>();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBgmSource();
        LoadSFXSource();
    }

    private void LoadSFXSource()
    {
        sfxSource = GetComponentInChildren<TruongSFX>();
    }

    private void LoadBgmSource()
    {
        bgmSource = GetComponentInChildren<TruongBGM>();
    }

    // protected override void SetDontDestroyOnLoad()
    // {
    //     SetDontDestroyOnLoad(true);
    // }
}