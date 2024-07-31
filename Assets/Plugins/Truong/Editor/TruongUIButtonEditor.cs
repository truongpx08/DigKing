using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Show clipName in inspector on PlaySfxOnClick = true
/// </summary>
[CustomEditor(typeof(TruongUIButtonHandler))]
public class TruongUIButtonEditor : Editor
{
    [SerializeField] private TruongUIButtonHandler script;

    public override void OnInspectorGUI()
    {
        // Call normal GUI (displaying "a" and any other variables you might have)
        base.OnInspectorGUI();

        SetScript();
        ShowOptions();
        ShowClipName();
        ShowClipPath();
    }


    private void ShowOptions()
    {
        if (!script.PlaySfxOnClick) return;

        ShowUI(() =>
        {
            EditorGUILayout.LabelField("        Is Clip Name", GUILayout.MaxWidth(156));
            script.isClipName = EditorGUILayout.Toggle(script.isClipName);
            script.isClipPath = !script.isClipName;
        });
        ShowUI(() =>
        {
            EditorGUILayout.LabelField("        Is Clip Path", GUILayout.MaxWidth(156));
            script.isClipPath = EditorGUILayout.Toggle(script.isClipPath);
            script.isClipName = !script.isClipPath;
        });
    }


    private void ShowClipName()
    {
        if (!script.PlaySfxOnClick) return;
        if (!script.isClipName) return;
        ShowUI(() =>
        {
            EditorGUILayout.LabelField("                Clip Name", GUILayout.MaxWidth(170));
            script.clipName = EditorGUILayout.TextField(script.clipName);
        });
    }

    private void ShowClipPath()
    {
        if (!script.PlaySfxOnClick) return;
        if (!script.isClipPath) return;
        ShowUI(() =>
        {
            EditorGUILayout.LabelField("                Clip Path", GUILayout.MaxWidth(170));
            script.clipPath = EditorGUILayout.TextField(script.clipPath);
        });
    }

    private void SetScript()
    {
        if (script) return;
        // Debug.Log("Setting script");
        script = (TruongUIButtonHandler)target;
    }

    private static void ShowUI(Action handler)
    {
        // Ensure the label and the value are on the same line
        EditorGUILayout.BeginHorizontal();
        handler?.Invoke();
        EditorGUILayout.EndHorizontal();
    }
}