using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TruongCommon
{
    private static TruongSFX _truongSfx;
    public static TruongSFX TruongSfx
    {
        get
        {
            if (!_truongSfx)
            {
                // Debug.Log("truongSfx is not available");
                _truongSfx = Object.FindObjectOfType<TruongSFX>();
            }

            // Debug.Log("truongSfx is available");
            return _truongSfx;
        }
        set => _truongSfx = value;
    }
}