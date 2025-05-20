#if !XAPI_REGISTRY_EXISTS
using System.IO;
using OmiLAXR.Editor;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.xAPI.Editor
{
    public class xApiRegistryFolderChecker
    {
        [InitializeOnLoad]
        internal static class XapiRegistryFolderChecker
        {
            private const string FolderPath = "Assets/xAPI.Registry";
            private static string XAPI_REGISTRY_EXISTS => ScriptingDefineSymbolsAssetPostProcessor.XAPI_REGISTRY_EXISTS;

            static XapiRegistryFolderChecker()
            {
                
                
                // Run only once on editor launch
                if (Directory.Exists(FolderPath))
                {
                    DefineUtility.AddXapiRegistryExistsDefine(XAPI_REGISTRY_EXISTS);
                    Debug.Log($"\"{XAPI_REGISTRY_EXISTS}\" defined because folder exists at: {FolderPath}");
                }
                else
                {
                    DefineUtility.RemoveXapiRegistryExistsDefine(XAPI_REGISTRY_EXISTS);
                    Debug.Log($"\"{XAPI_REGISTRY_EXISTS}\" removed because folder does not exist at: {FolderPath}");
                }
                
                
            }
        }
    }
}
#endif