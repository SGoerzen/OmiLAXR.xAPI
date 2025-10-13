/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Components;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers.Environment
{
    /// <summary>
    /// xAPI composer for environment areas and rooms.
    /// Emits statements when the learner enters or exits an Area or a Room,
    /// including the target name and the world-space enter/exit point.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 4) Composers / [xAPI] Area Composer")]
    [Description(
        "Creates statements:" +
        "\n- actor entered area (activity: area) with values: areaName(name), enterPoint(vec3)" +
        "\n- actor exited area (activity: area) with values: areaName(name), exitPoint(vec3)" +
        "\n- actor entered room (activity: room) with values: roomName(name), enterPoint(vec3)" +
        "\n- actor exited room (activity: room) with values: roomName(name), exitPoint(vec3)"
    )]
    public class AreaComposer : xApiComposer<AreaTrackingBehaviour>
    {
        /// <summary>
        /// Groups this composer under environment analytics.
        /// </summary>
        public override ComposerGroup GetGroup() => ComposerGroup.Environment;
        
        /// <summary>
        /// Author information for this composer.
        /// </summary>
        public override Author GetAuthor() => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        /// <summary>
        /// Determines whether the provided area instance represents a Room.
        /// </summary>
        /// <param name="area">Area instance to test.</param>
        /// <returns>True if the area is a Room; otherwise false.</returns>
        private static bool IsRoom(Area area) => area.GetType() == typeof(Room);

        /// <summary>
        /// Creates a baseline statement for the given verb and area type.
        /// Selects the appropriate activity (room or area) and adds its name.
        /// </summary>
        /// <param name="verb">The xAPI verb (entered/exited).</param>
        /// <param name="area">The target Area or Room.</param>
        /// <returns>Initialized xAPI statement with activity and name set.</returns>
        private xApiStatement CreateDefaultStatement(xAPI_Verb verb, Area area)
        {
            if (IsRoom(area))
            {
                return actor.Does(verb)
                    .Activity(xapi.virtualReality.activities.room)
                    .WithValue(xapi.virtualReality.extensions.activity.roomName(area.name));
            }
            return actor.Does(verb)
                .Activity(xapi.virtualReality.activities.area)
                .WithValue(xapi.virtualReality.extensions.activity.areaName(area.name));
        }
        
        /// <summary>
        /// Subscribes to area enter/exit events and composes corresponding xAPI statements.
        /// For enter, attaches enterPoint; for exit, attaches exitPoint.
        /// </summary>
        /// <param name="tb">Tracking behaviour raising area events.</param>
        protected override void Compose(AreaTrackingBehaviour tb)
        {
            // Enter event: emit statement with area/room name and world-space enter point.
            tb.OnEntered.AddHandler((sender, area, point) =>
            {
                var stmt = CreateDefaultStatement(xapi.virtualReality.verbs.entered, area);
                stmt.WithValue(xapi.virtualReality.extensions.activity.enterPoint(point));
                SendStatement(sender, stmt);
            });
            // Exit event: emit statement with area/room name and world-space exit point.
            tb.OnLeft.AddHandler((sender, area, point) =>
            {
                var stmt = CreateDefaultStatement(xapi.virtualReality.verbs.exited, area);
                stmt.WithValue(xapi.virtualReality.extensions.activity.exitPoint(point));
                SendStatement(sender, stmt);
            });
        }
    }
}
#endif