using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TruongBGM : TruongBGMAbstract
{
    protected override void OnReplay()
    {
        PlayWithAddressable("Assets/Audio/BGM/441650__vabsounds__puzzle.wav");
    }
}