using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TruongInitialization : TruongMonoBehaviour
{
    protected override void Awake()
    {
        InitializeSingleton();
    }

    // [Button]
    private void InitializeSingleton()
    {
        try
        {
// Iterate over all the components attached to the current game object and its children
            foreach (Component item in GetComponents<Component>())
            {
                foreach (var baseType in GetBaseTypeList(item))
                {
                    // Check if the component is a subclass of TruongSingleton
                    if (!baseType.ToString().Contains("TruongSingleton")) continue;
                    InvokeInitialize(item, baseType);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(gameObject.name);
            throw;
        }
    }

    private void InvokeInitialize(Component item, Type baseType)
    {
        // Get the Initialize() method of the base class TruongSingleton using reflection
        MethodInfo m = baseType.GetMethod("InitializeSingleton");
        if (m == null) return;

        // Invoke the Initialize() method
        m.Invoke(item, null);
    }

    // [Button]
    public List<Type> GetBaseTypeList(Component component)
    {
        List<Type> list = new List<Type>();
        Type type = component.GetType();
        for (int count = 0; count < 100; count++)
        {
            if (type == null || type.BaseType == typeof(MonoBehaviour)) break;
            type = type.BaseType;
            if (type == null) continue;
            list.Add(type);
        }

        return list;
    }
}