#if UNITY_EDITOR
using System.IO;
using System.Linq;
using OmiLAXR.Editor;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.xAPI.Editor
{
    internal class ScriptingDefineSymbolsAssetPostProcessor : AssetPostprocessor
    {
        private static bool _isProcessing = false;
        private static bool _scheduled = false;

        public const string XAPI_REGISTRY_EXISTS = "XAPI_REGISTRY_EXISTS";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (_isProcessing || _scheduled)
                return;

            var defineExists = DefineUtility.IsDefined(XAPI_REGISTRY_EXISTS);

            var shouldAdd = !defineExists && importedAssets.Any(path =>
                Path.GetFileName(path) == "xAPI.Registry" &&
                AssetDatabase.IsValidFolder(path) &&
                !path.EndsWith(".meta"));

            var shouldRemove = defineExists && deletedAssets.Any(path =>
                Path.GetFileName(path) == "xAPI.Registry" &&
                !path.EndsWith(".meta"));
            
            if (!shouldAdd && !shouldRemove)
                return;

            _scheduled = true;

            EditorApplication.delayCall += () =>
            {
                _isProcessing = true;

                try
                {
                    if (shouldAdd)
                    {
                        DefineUtility.Set(XAPI_REGISTRY_EXISTS);
                        Debug.Log("xAPI.Registry does exist! \"XAPI_REGISTRY_EXISTS\" was set. You can start working with xAPI.Registry.");
                    }
                    else if (shouldRemove)
                    {
                        DefineUtility.Unset(XAPI_REGISTRY_EXISTS);
                        Debug.LogError("xAPI.Registry folder deleted! \"XAPI_REGISTRY_EXISTS\" was removed. If you like to work with xAPI.Registry, please use xAPI4Unity to create it.");
                    }
                }
                finally
                {
                    _isProcessing = false;
                    _scheduled = false;
                }
            };
        }
    }
}
#endif
