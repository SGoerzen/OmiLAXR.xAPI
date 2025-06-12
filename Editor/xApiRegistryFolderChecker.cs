#if UNITY_EDITOR

using System.IO;
using OmiLAXR.Editor;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.xAPI.Editor
{
    internal class xApiRegistryFolderChecker
    {
        private const string FolderPath = "Assets/xAPI.Registry";
        private static string XAPI_REGISTRY_EXISTS => ScriptingDefineSymbolsAssetPostProcessor.XAPI_REGISTRY_EXISTS;

        [MenuItem("OmiLAXR / Validation / Check if xAPI Registry exists")]
        [InitializeOnLoadMethod]
        private static void CheckOnEditorStartup()
        {
            if (Directory.Exists(FolderPath))
            {
                if (!DefineUtility.IsDefined(XAPI_REGISTRY_EXISTS))
                {
                    DefineUtility.Set(XAPI_REGISTRY_EXISTS);
                    Debug.Log($"\"{XAPI_REGISTRY_EXISTS}\" defined because folder exists at: '{FolderPath}'.");
                }
            }
            else
            {
                if (DefineUtility.IsDefined(XAPI_REGISTRY_EXISTS))
                {
                    DefineUtility.Unset(XAPI_REGISTRY_EXISTS);
                    Debug.LogWarning($"Unset \"{XAPI_REGISTRY_EXISTS}\" because folder no longer exists.");
                }
                else
                {
                    Debug.LogWarning(
                        $"xAPI.Registry not found at '{FolderPath}'. If needed, please create it via xAPI4Unity or import from package 'Samples'. Think of to place it with the name 'xAPI.Registry' to 'Assets' root.");
                }
            }
        }
    }
}
#endif