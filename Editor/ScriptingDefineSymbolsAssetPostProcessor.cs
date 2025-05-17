#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using OmiLAXR.Editor;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.xAPI.Editor
{
    internal class ScriptingDefineSymbolsAssetPostProcessor : AssetPostprocessor
    {
        const string XAPI_REGISTRY_EXISTS = "XAPI_REGISTRY_EXISTS";
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Check if the folder was created or deleted
            foreach (var path in importedAssets)
            {
                if (path.EndsWith("xAPI.Registry") && AssetDatabase.IsValidFolder(path))
                {
                    DefineUtility.AddXapiRegistryExistsDefine(XAPI_REGISTRY_EXISTS);
                    Debug.Log("xAPI.Registry folder created! \"XAPI_REGISTRY_EXISTS\" was set.");
                }
            }
            foreach (var path in deletedAssets)
            {
                if (path.EndsWith("xAPI.Registry"))
                {
                    DefineUtility.RemoveXapiRegistryExistsDefine(XAPI_REGISTRY_EXISTS);
                    Debug.Log("xAPI.Registry folder deleted! \"XAPI_REGISTRY_EXISTS\" was removed.");
                }
            }
        }
    }
}
#endif