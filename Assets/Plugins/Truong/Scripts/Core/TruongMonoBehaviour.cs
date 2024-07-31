using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Inheriting this class helps facilitate maintenance and expansion, enhancing flexibility in project development.   
/// </summary>
public abstract class TruongMonoBehaviour : TruongBehavioral
{
    /// <summary>
    /// Automatically set default values after code changes
    /// </summary>
    private async void OnValidate()
    {
        await Task.Delay(100); //Delay to wait for App.IsPlaying = true when entering PlayMode
        if (Application.isPlaying) return;
        if (!Application.isEditor) return;
        try
        {
            SetDefault();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    protected virtual void Awake()
    {
        SetDefault();
    }


    protected virtual void OnEnable()
    {
        // For Override 
    }

    protected virtual void Start()
    {
        // For Override 
    }

    protected virtual void FixedUpdate()
    {
        // For Override 
    }

    protected virtual void Update()
    {
        // For Override 
    }

    protected virtual void LateUpdate()
    {
        // For Override 
    }

    protected virtual void OnDisable()
    {
        // For Override 
    }

    protected virtual void OnDestroy()
    {
        // For Override 
    }

    /// <summary>
    /// Renaming variables often leads to variables and dependencies being reset.
    /// Call this function in Awake to ensure that variables and dependencies are assigned values when entering the game.
    /// </summary>
    protected virtual void SetDefault()
    {
        CreateChildren();
        LoadComponents();
        SetVariableToDefault();
    }

    protected virtual void CreateChildren()
    {
    }

    /// <summary>
    /// Renaming variables often leads to the loss of dependencies for components.
    /// Therefore, assign default values to components in this function to initialize them quickly instead of re-entering them in the Unity editor.
    /// </summary>
    protected virtual void LoadComponents()
    {
        //For override
    }

    /// <summary>
    /// Renaming variables often leads to variables being reset.
    /// Therefore, assign default values to variables in this function to initialize them quickly instead of re-entering them in the Unity editor.
    /// </summary>
    protected virtual void SetVariableToDefault()
    {
        //For override
    }

    /// <summary>
    /// Calling this function makes all variables and dependencies of self and children assigned values.
    /// </summary>
    [ContextMenu("Set Defaults All")]
    private void SetDefaultAll()
    {
        SetDefault();
        SetDefaultAllChild();
    }

    /// <summary>
    /// Calling this function helps the children's variables and dependencies to be assigned values.
    /// </summary>
    private void SetDefaultAllChild()
    {
        var child = GetComponentsInChildren<TruongMonoBehaviour>().ToList();
        child.ForEach(c => c.SetDefault());
    }
}