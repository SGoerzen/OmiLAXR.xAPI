/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace OmiLAXR.xAPI.Editor
{
    /// <summary>
    /// Utility class for adding a predefined xAPI prefab to the current Unity scene.
    /// Provides a menu item to easily instantiate xAPI-related components.
    /// </summary>
    internal static class AddPrefabToScene
    {
        /// <summary>
        /// Hardcoded path to the xAPI prefab within the package.
        /// Ensures the prefab can be loaded from a specific package location.
        /// </summary>
        private const string PrefabPath = "Packages/com.rwth.unity.omilaxr.xapi/Resources/Prefabs/OmiLAXR.xAPI.prefab";

        /// <summary>
        /// Menu item to add xAPI Data Provider components to the current scene.
        /// Accessible via Unity menu: OmiLAXR > Prefabs > 2) Add OmiLAXR.xAPI Data Provider components to Scene
        /// </summary>
        [MenuItem("OmiLAXR / Prefabs / 2) Add OmiLAXR.xAPI Data Provider components to Scene")]
        public static void AddPrefab()
        {
            // Attempt to load the prefab from the specified path
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            
            // Handle case where prefab cannot be found
            if (prefab == null)
            {
                DebugLog.xAPI.Error($"Prefab not found at path: {PrefabPath}");
                return;
            }

            // Instantiate the prefab in the current scene
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            
            // Register the creation for undo support
            Undo.RegisterCreatedObjectUndo(instance, "Add xAPI Prefab");

            // Position the prefab at the scene origin
            instance.transform.position = Vector3.zero;

            // Mark the scene as modified to prompt saving
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            // Log successful prefab addition
            DebugLog.xAPI.Print($"Added prefab '{prefab.name}' to scene.");
        }
    }
}
#endif