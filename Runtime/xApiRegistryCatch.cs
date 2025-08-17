/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if !XAPI_REGISTRY_EXISTS
using System;
using System.Collections;
using System.Collections.Generic;
using OmiLAXR;

namespace xAPI.Registry
{
    /// <summary>
    /// Static utility class providing error messages when xAPI Registry is not available.
    /// Serves as a safety net for compilation scenarios where the xAPI Registry package
    /// is not installed, providing clear guidance to developers about required dependencies.
    /// </summary>
    internal static class xApiRegistry
    {
        /// <summary>
        /// Standard error message displayed when xAPI Registry functionality is accessed without proper installation.
        /// Guides developers to install the required 'xAPI4Unity' package to enable full xAPI Registry support.
        /// </summary>
        public const string CatchMessage = "Use 'xAPI4Unity to create a valid xAPI Registry.";
    }
    
    /// <summary>
    /// Extension methods stub that throws informative exceptions when xAPI Registry is unavailable.
    /// Prevents compilation errors while clearly indicating missing dependency requirements
    /// for proper xAPI actor conversion functionality.
    /// </summary>
    public static class OmiLAXR_Ext {
        /// <summary>
        /// Stub method for Actor to xAPI_Actor conversion that indicates missing xAPI Registry.
        /// </summary>
        public static xAPI_Actor ToXAPIActor(this Actor actor) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Stub method for Actor array to xAPI_Actor array conversion that indicates missing xAPI Registry.
        /// </summary>
        public static xAPI_Actor[] ToXAPIActors(this Actor[] actor) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Stub method for Team to xAPI_Actor conversion that indicates missing xAPI Registry.
        /// </summary>
        public static xAPI_Actor ToXAPIActor(this Team team) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Stub method for Instructor to xAPI_Actor conversion that indicates missing xAPI Registry.
        /// </summary>
        public static xAPI_Actor ToXAPIActor(this Instructor team) => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    
    /// <summary>
    /// Stub structure for xAPI_Actor that throws informative exceptions when accessed without xAPI Registry.
    /// Provides interface compatibility while clearly indicating missing dependency requirements.
    /// </summary>
    public struct xAPI_Actor
    {
        /// <summary>
        /// Actor name property stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public string Name => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Actor email property stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public string Email => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Actor constructor stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public xAPI_Actor(string name = "", string email = "")
            => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    
    /// <summary>
    /// Empty stub class for xAPI_Contexts when xAPI Registry is not available.
    /// Prevents compilation errors while indicating missing functionality.
    /// </summary>
    public class xAPI_Contexts {}

    /// <summary>
    /// Abstract stub class for xAPI_Definition that provides interface compatibility.
    /// All methods throw NotImplementedException to indicate missing xAPI Registry dependency.
    /// </summary>
    public abstract class xAPI_Definition
    {
        /// <summary>
        /// Definition key property stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public string Key => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Multilingual names dictionary stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public Dictionary<string, string> Names => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Multilingual descriptions dictionary stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public Dictionary<string, string> Descriptions => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// ID creation method stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public string CreateValidId(string uri) =>
            throw new NotImplementedException(xApiRegistry.CatchMessage);
            
        /// <summary>
        /// Path matching method stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public bool MatchesPath(string path, char sep = '/') => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    
    /// <summary>
    /// Stub class for xAPI_Activity extending xAPI_Definition for interface compatibility.
    /// </summary>
    public class xAPI_Activity : xAPI_Definition {}

    /// <summary>
    /// Stub class for xAPI_Verb extending xAPI_Definition for interface compatibility.
    /// </summary>
    public class xAPI_Verb : xAPI_Definition
    {
        // Inherits all stub functionality from xAPI_Definition
    }

    /// <summary>
    /// Stub class for xAPI_Extension extending xAPI_Definition for interface compatibility.
    /// </summary>
    public class xAPI_Extension : xAPI_Definition
    {
        // Inherits all stub functionality from xAPI_Definition
    }

    /// <summary>
    /// Abstract stub class for xAPI_Extensions extending Dictionary for interface compatibility.
    /// Provides dictionary interface while throwing exceptions for xAPI Registry dependency.
    /// </summary>
    public abstract class xAPI_Extensions : Dictionary<xAPI_Extension, object>
    {
        /// <summary>
        /// Key existence check stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public bool ContainsKey(string key) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Path existence check stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public bool ContainsPath(string key) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Range addition method stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public bool AddRange(xAPI_Extensions ext) => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Path-based removal method stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public bool Remove(string path, char sep = '/') => throw new NotImplementedException(xApiRegistry.CatchMessage);
        
        /// <summary>
        /// Key-based removal method stub that indicates missing xAPI Registry dependency.
        /// </summary>
        public bool RemoveByKey(string key) => throw new NotImplementedException(xApiRegistry.CatchMessage);
    }
    
    /// <summary>
    /// Stub class for context-specific xAPI extensions when xAPI Registry is unavailable.
    /// </summary>
    public class xAPI_Extensions_Context : xAPI_Extensions {}
    
    /// <summary>
    /// Stub class for result-specific xAPI extensions when xAPI Registry is unavailable.
    /// </summary>
    public class xAPI_Extensions_Result : xAPI_Extensions {}
    
    /// <summary>
    /// Stub class for activity-specific xAPI extensions when xAPI Registry is unavailable.
    /// </summary>
    public class xAPI_Extensions_Activity : xAPI_Extensions {}
}
#endif