using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongAudioClips : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    [Button]
    public AudioClip GetAudioClip(string clipName)
    {
        return audioClips.Find(a => a.name == clipName);
    }
}