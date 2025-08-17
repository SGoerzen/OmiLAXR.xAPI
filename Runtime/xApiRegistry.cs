/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using System.ComponentModel;
using UnityEngine;
using xAPI.Registry;

namespace OmiLAXR.xAPI
{
    /// <summary>
    /// Unity component for managing xAPI Registry configuration and vocabulary definitions.
    /// Provides centralized access to xAPI Registry contexts and URI configuration for
    /// consistent xAPI statement generation across all composers, hooks, and endpoints
    /// within the OmiLAXR analytics framework, ensuring xAPI specification compliance.
    /// </summary>
    [AddComponentMenu("OmiLAXR / xAPI Registry")]
    [Description("This is an instance for managing xAPI Registry settings.")]
    public class xApiRegistry : MonoBehaviour
    {
        /// <summary>
        /// Static instance of xAPI Registry contexts containing all vocabulary definitions.
        /// Provides global access to standardized xAPI verbs, activities, extensions,
        /// and other vocabulary elements for consistent statement generation across
        /// the entire analytics pipeline and learning record creation.
        /// </summary>
        public static xAPI_Contexts definitions = new xAPI_Contexts();
        
        /// <summary>
        /// Base URI for resolving xAPI identifiers and vocabulary definitions.
        /// Used as the namespace root for generating properly formatted xAPI IRIs
        /// that ensure compatibility with xAPI Registry definitions and Learning Record Store
        /// requirements for semantic interoperability and data exchange.
        /// </summary>
        public string uri = "https://xapi.elearn.rwth-aachen.de/definitions";

        // Note: TinCan.TCAPIVersion configuration commented out for future use
        // May be needed for explicit xAPI version control in advanced scenarios
        //private static TinCan.TCAPIVersion version = TinCan.TCAPIVersion.latest();
    }
}