using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TruongCreateFolders : MonoBehaviour
{
    [MenuItem("Truong/Create 2D Folders")]
    private static void Create2DFolders()
    {
        List<string> list = new List<string>
        {
            TruongConstant.SCENES,
            TruongConstant.SCRIPTS,
            TruongConstant.PREFABS,
            TruongConstant.RESOURCES,
            TruongConstant.PLUGINS,
            TruongConstant.SPRITES,
        };
        list.ForEach(item => { CreateAFolder(TruongConstant.ASSETS, item); });

        CreateFolderInResources(TruongConstant.PREFABS);
        CreateFolderInResources(TruongConstant.SPRITES);
        CreateAFolder(Path.Combine(TruongConstant.ASSETS, TruongConstant.RESOURCES, TruongConstant.PREFABS),
            TruongConstant.LOAD_ON_LOAD_SCENE);
    }

    private static void CreateFolderInResources(string folderName)
    {
        string resourcesPath = Path.Combine(TruongConstant.ASSETS, TruongConstant.RESOURCES);
        CreateAFolder(resourcesPath, folderName);
    }

    private static void CreateAFolder(string parentFolder, string folderName)
    {
        string prefabFolderPath = Path.Combine(parentFolder, folderName);
        if (AssetDatabase.IsValidFolder(prefabFolderPath)) return;

        AssetDatabase.CreateFolder(parentFolder, folderName);
        Log(folderName);
    }

    private static bool HasFolder(string folderName)
    {
        string folderPath = Path.Combine(Application.dataPath, folderName);
        return Directory.Exists(folderPath);
    }

    private static void Log(string item)
    {
        Debug.Log("Has created folder " + item);
    }
}