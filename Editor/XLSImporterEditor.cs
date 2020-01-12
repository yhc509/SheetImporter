using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class XLSImporterEditorWindow : EditorWindow
{
    [MenuItem("Assets/XLSImport")]
    private static void XLSImport()
    {
        foreach (var selectedObj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(selectedObj);
            XLSImporter.Import(path);
        }
    }
    
    [MenuItem("Assets/XLSImport", true)]
    private static bool NewMenuOptionValidation()
    {
        foreach (var selectedObj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(selectedObj);
            var ext = Path.GetExtension(path);
            if (ext != ".xlsx") return false;
        }
        return true;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            XLSImporter.Import(
                Path.Combine(Application.streamingAssetsPath, "test.xlsx")
            );
        }
        
    }
}
