using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TruongDeleteLocalData : MonoBehaviour
{
    [MenuItem("Truong/Delete Local Data")]
    private static void DeleteData()
    {
        var dataPath = Path.Combine(Application.persistentDataPath, TruongConstant.FILE_NAME);
        File.Delete(dataPath);
    }
}