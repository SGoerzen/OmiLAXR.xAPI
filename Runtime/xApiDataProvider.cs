/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using UnityEngine;

namespace OmiLAXR.xAPI
{
    /// <summary>
    /// Unity component that serves as a specialized data provider for xAPI-specific analytics data.
    /// Extends the base DataProvider functionality to handle xAPI statement enrichment and context
    /// information gathering within the OmiLAXR analytics pipeline, enabling xAPI hooks and composers
    /// to access relevant data sources for comprehensive learning analytics statement generation.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Data Providers / xAPI Data Provider")]
    public class xApiDataProvider : DataProvider
    {
        // This class inherits all functionality from DataProvider
        // and serves as a specialized marker for xAPI-specific data provision
        // Additional xAPI-specific data provider functionality can be added here as needed
    }
}