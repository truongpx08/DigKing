using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class TruongPath
{
    public static string GetPrefabInResourcePath(string name)
    {
        return Path.Combine(TruongConstant.PREFABS, name);
    }

    public static string GetCommonObjSceneInResourcePath()
    {
        return Path.Combine(TruongConstant.PREFABS, TruongConstant.LOAD_ON_LOAD_SCENE);
    }

    public static string GetSpriteInResourcePath(string name)
    {
        return Path.Combine(TruongConstant.SPRITES, name);
    }
}