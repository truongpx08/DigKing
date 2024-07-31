using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
///  Inherit this function to ensure that there is only one instance of a class created.
///  You can access the unique object from anywhere in the program using the syntax "ClassName.Instance".
/// </summary>
public abstract class TruongSingleton<T> : TruongChild
{
    private static T instance;
    public static T Instance => instance;
    public static bool IsAvailable => Instance != null;

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        AddTruongInitialization();
    }

    private void AddTruongInitialization()
    {
        if (HasComponent<TruongInitialization>()) return;
        this.gameObject.AddComponent<TruongInitialization>();
    }

    /// <summary>
    /// Called from TruongInitialization 
    /// </summary>
    // [Button]
    public void InitializeSingleton()
    {
        Debug.Log("Init Singleton: " + this.name);
        if (!IsObjNull(Instance))
        {
            Destroy(this.gameObject); // Destroy the current object if another object already exists
            return;
        }

        SetInstance();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SetInstanceToNull();
    }

    /// <summary>
    /// Destroy the current object if another object already exists
    /// When the singleton is destroyed, delete the instance reference to avoid null errors when called
    /// </summary>
    private void SetInstanceToNull()
    {
        if (!Application.isPlaying) return;
        if (this.gameObject.scene.name == "DontDestroyOnLoad")
            return; // When deleted because another object already exists, it is the dontdestroyonload object. Will not destroy its instance
        Debug.LogWarning("SetInstanceToNull : " + this.gameObject.name);
        instance = default(T);
    }

    // [Button]
    private T DebugInstance()
    {
        return Instance;
    }

    // [Button]
    private void SetInstance()
    {
        var components = GetComponents<T>();
        LogException(components);
        if (components == null) return;
        instance = components[0];
    }

    private void LogException(T[] components)
    {
        if (components == null)
        {
            Debug.LogError(
                $"Component inheriting from TruongSingleton not found.");
            return;
        }

        if (components.Length <= 1) return;
        Debug.LogError(
            $"There are more than one component inheriting from TruongSingleton.");
    }
}