using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

class SheetPostprocessor : AssetPostprocessor
{
    private static string _codeDirectory = Path.Combine("Scripts", "Sheet");
    private static string _binaryDirectory = Path.Combine("Data");
    
    void OnPreprocessAsset()
    {
        var assetPath = assetImporter.assetPath;
        var assetExtension = Path.GetExtension(assetPath);
        
        try
        {
            if (assetExtension == ".xlsx")
            {
                GenerateCode(assetPath);
                EditorUtility.DisplayProgressBar("Sheet Imported", assetPath, 0f);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
    
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded() {
        var dataPathDir = new DirectoryInfo( Application.dataPath );
        var dataPathUri = new Uri( Application.dataPath );

        var fileList = dataPathDir.GetFiles("*.xlsx", SearchOption.AllDirectories);
        if (fileList.Length == 0) return;

        try
        {
            for (int i = 0; i < fileList.Length; i++)
            {
                var file = fileList[i];

                var relativeUri = dataPathUri.MakeRelativeUri(new Uri(file.FullName));
                var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

                GenerateBinary(relativePath);
                EditorUtility.DisplayProgressBar("Sheet Conveted", relativePath, (float) i / fileList.Length);
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private static void GenerateCode(string assetPath)
    {
        var assetName = Path.GetFileName(assetPath).Replace(".xlsx", string.Empty);
        
        CodeGenerator codeGen = new CodeGenerator(_codeDirectory, assetName + "Sheet");
        codeGen.Generate(assetPath);

        Debug.LogFormat($"<b><color=green>[Sheet Imported] {assetPath}</color></b>");
    }

    private static void GenerateBinary(string assetPath)
    {
        var assetName = Path.GetFileName(assetPath).Replace(".xlsx", string.Empty);
        
        SheetGenerator sheetGen = new SheetGenerator(_binaryDirectory, assetName);
        sheetGen.Generate(assetPath);
        
        Debug.LogFormat($"<b><color=green>[Sheet Converted] {assetPath}</color></b>");
    }
}

#endif
