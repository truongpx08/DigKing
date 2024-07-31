using System.IO;
using UnityEditor;
using UnityEngine;

public class TruongExportPackage
{
    [MenuItem("Truong/Export Truong Package")]
    private static void Export()
    {
        // Path to the folder containing the plugin
        string folderPath =
            Path.Combine(TruongConstant.ASSETS, TruongConstant.PLUGINS, TruongConstant.TRUONG);

        // Show save file panel to get the package path
        string savePath =
            EditorUtility.SaveFilePanel("Export Package", "", TruongConstant.TRUONG, "unitypackage");

        // If the user cancels the save file panel, return
        if (string.IsNullOrEmpty(savePath))
            return;

        // Export the plugin as a package
        AssetDatabase.ExportPackage(folderPath, savePath, ExportPackageOptions.Recurse);

        // Print a completion message
        Debug.Log("Plugin has been successfully exported as a .unitypackage file.");
    }
}