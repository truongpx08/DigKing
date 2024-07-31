using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// The prefabs in the Resources/Prefabs/LoadOnLoadScene folder will be automatically created each time the scene is initialized.
/// Already integrated in TruongSceneController.
/// To use it, inherit that TruongSceneController and drag the game objects you want to exist in all scenes into the Resources/Prefabs/LoadOnLoadScene folder.
/// </summary>
public class TruongCommonGoSpawner : TruongSpawner
{
    protected override void LoadPrefabInResource()
    {
        LoadPrefabInResourceWithPrefabPath(TruongPath.GetCommonObjSceneInResourcePath());
    }

    protected override void Awake()
    {
        base.Awake();
        SpawnAllPrefab();
    }
}