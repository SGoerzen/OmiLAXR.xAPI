/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using OmiLAXR.TrackingBehaviours.Learner;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.UnityXR.xAPI.Composers
{
    /// <summary>
    /// xAPI composer for creating learning analytics statements from VR teleportation interactions.
    /// Generates locomotion-based statements when teleportation events occur, capturing spatial navigation
    /// patterns and movement behaviors within virtual reality learning environments.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / Teleportation Composer (xAPI)"),
     Description("Creates statements:\n- actor teleported teleportPoint/teleportArea with vrObjectName(String), hand(Hand) and result startValue(Vector3), endValue(Vector3), cameraYOffset(Float)")]
    public class TeleportationComposer : xApiComposer<TeleportationTrackingBehaviour>
    {
        /// <summary>
        /// Categorizes this composer under locomotion tracking for organizational purposes.
        /// Groups teleportation analytics with other movement and navigation-related composers.
        /// </summary>
        /// <returns>ComposerGroup.Locomotion indicating this handles movement and navigation</returns>
        public override ComposerGroup GetGroup() => ComposerGroup.Locomotion;
        
        /// <summary>
        /// Identifies the author of this composer implementation for attribution and support.
        /// </summary>
        /// <returns>Author information including name and contact details</returns>
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Configures xAPI statement composition for teleportation events.
        /// Binds event handlers that create statements with comprehensive spatial data,
        /// including start/end positions, target information, and interaction context.
        /// </summary>
        /// <param name="tb">TeleportationTrackingBehaviour instance to bind event handlers to</param>
        protected override void Compose(TeleportationTrackingBehaviour tb)
        {
           // Handle teleportation events with comprehensive spatial and contextual data
           tb.OnTeleported.AddHandler((owner, args) =>
           {
               // Select appropriate activity based on teleportation target type
               var activity = args.TargetType == TeleportationTrackingBehaviour.TeleportationTargetType.Anchor ? 
                   xapi.virtualReality.activities.teleportPoint : xapi.virtualReality.activities.teleportArea;
               
               // Create comprehensive teleportation statement with spatial and contextual data
               var stmt = actor.Does(xapi.virtualReality.verbs.teleported)
                   .WithResult(xapi.generic.extensions.result
                       .startValue(args.StartState.FloorPosition)  // Starting floor position
                       .endValue(args.EndState.FloorPosition))     // Ending floor position
                   .WithResult(xapi.virtualReality.extensions.result.cameraYOffset(args.CameraYOffset)) // Camera height offset
                   .Activity(activity)
                   .WithExtension(xapi.virtualReality.extensions.activity
                       .vrObjectName(args.Target.gameObject.GetTrackingName())) // Target object name
                   .WithExtension(xapi.gestures.extensions.activity.hand(args.Hand))         // Hand used for teleportation
                   .WithDuration(args.RequestTime); // Time taken to complete teleportation
               
               SendStatement(owner, stmt);
           });
        }
    }
}
#endif