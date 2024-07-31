using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TruongSceneController<T> : TruongSingleton<T>
{
    protected override void CreateChildren()
    {
        base.CreateChildren();
        CreateChild(TruongConstant.COMMON_GO_SPAWNER)?.AddComponent<TruongCommonGoSpawner>();
    }
}