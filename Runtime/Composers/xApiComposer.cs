/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    /// <summary>
    /// Abstract base class for xAPI statement composers.
    /// Provides common functionality for creating and managing xAPI statements.
    /// </summary>
    /// <typeparam name="T">A tracking behavior component that implements ITrackingBehaviour</typeparam>
    public abstract class xApiComposer<T> : Composer<T>
        where T : ActorPipelineComponent, ITrackingBehaviour
    {
        // Builder responsible for constructing xAPI statements
        /// <summary>
        /// Builds the actor information for xAPI statements
        /// </summary>
        protected xApiStatement.Builder actor;

        // Static access to xAPI context definitions
        /// <summary>
        /// Provides access to global xAPI context definitions
        /// </summary>
        protected static xAPI_Contexts xapi => xApiRegistry.definitions;

        // Base URI for the xAPI statements
        protected string Uri = "";

        /// <summary>
        /// Virtual method to retrieve the current URI.
        /// Can be overridden by derived classes to provide custom URI logic.
        /// </summary>
        /// <returns>The current URI for xAPI statements</returns>
        protected virtual string GetUri() => Uri;

        /// <summary>
        /// Initializes the xAPI composer when the component is enabled.
        /// Sets up the URI and actor information for xAPI statements.
        /// </summary>
        protected override void OnEnable()
        {
            // Try to find a global xAPI registry in parent components
            var globalUri = GetComponentInParent<xApiRegistry>();
            
            // Update URI if a global registry is found and enabled
            if (globalUri && globalUri.enabled)
                Uri = globalUri.uri;
            
            // Initialize the actor builder with the current URI and author
            actor = new xApiStatement.Builder(GetUri(), GetAuthor());
            
            // Call the base OnEnable method
            base.OnEnable();
        }
    }
}
#endif