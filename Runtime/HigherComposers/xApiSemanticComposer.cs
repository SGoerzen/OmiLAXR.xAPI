/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using OmiLAXR.Composers.HigherComposers;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers.HigherComposers
{
    /// <summary>
    /// Abstract base class for semantic-level xAPI composers that operate on pipeline data.
    /// Provides higher-order composition capabilities by analyzing patterns across multiple
    /// statements or tracking behaviors to generate semantic learning analytics insights
    /// that go beyond individual interaction events to capture learning progress and understanding.
    /// </summary>
    /// <typeparam name="TActorOfPipeline">Type of pipeline that serves as the actor source for semantic analysis</typeparam>
    public abstract class xApiSemanticComposer<TActorOfPipeline> : HigherComposer<xApiStatement>
        where TActorOfPipeline : Pipeline
    {
        /// <summary>
        /// Provides access to xAPI Registry contexts for creating standardized semantic statements.
        /// Enables semantic composers to generate statements using consistent xAPI vocabulary
        /// for complex learning analytics scenarios like competency assessment and learning progression.
        /// </summary>
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();

        /// <summary>
        /// Statement builder configured with URI and author information for semantic statement creation.
        /// Pre-configured builder that semantic composers can use to generate statements
        /// with consistent metadata and proper xAPI Registry integration.
        /// </summary>
        protected xApiStatement.Builder actor;
        
        /// <summary>
        /// Reference to the target pipeline that provides the data source for semantic analysis.
        /// Enables semantic composers to access and analyze patterns from the pipeline's
        /// statement history and tracking data for higher-order learning insights.
        /// </summary>
        protected TActorOfPipeline targetPipeline;
        
        /// <summary>
        /// Base URI for resolving xAPI identifiers in semantic statements.
        /// Provides namespace context for generating properly formatted xAPI IRIs
        /// that ensure semantic statement compatibility with xAPI Registry definitions.
        /// </summary>
        protected string Uri = "";
        
        /// <summary>
        /// Virtual method for retrieving the current URI context.
        /// Allows derived semantic composers to override URI resolution logic
        /// for specific semantic analysis scenarios or custom identifier schemes.
        /// </summary>
        /// <returns>Current base URI for xAPI identifier resolution</returns>
        protected virtual string GetUri() => Uri;

        /// <summary>
        /// Unity Awake method that initializes semantic composer dependencies and configuration.
        /// Sets up URI context from parent xApiRegistry, establishes pipeline connection,
        /// and prepares the statement builder with proper metadata for semantic statement generation.
        /// </summary>
        protected virtual void Awake()
        {
            // Retrieve global URI configuration from parent xAPI Registry
            var globalUri = GetComponentInParent<xApiRegistry>();
            if (globalUri && globalUri.enabled)
                Uri = globalUri.uri;

            // Establish connection to target pipeline for data access
            targetPipeline = GetPipeline<TActorOfPipeline>();
            
            // Initialize statement builder with URI context and author metadata
            actor = new xApiStatement.Builder(GetUri(), GetAuthor());
        }
    }
}
#endif