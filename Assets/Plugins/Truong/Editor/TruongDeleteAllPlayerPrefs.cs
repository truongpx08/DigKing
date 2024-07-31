using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TruongDeleteAllPlayerPrefs : MonoBehaviour
{
    [MenuItem("Truong/Delete All PlayerPrefs")]
    private static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}