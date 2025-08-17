/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
#if XAPI_REGISTRY_EXISTS
using System.Collections.Generic;
using System.Linq;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Extensions
{
    /// <summary>
    /// Extension methods for converting OmiLAXR actor types to xAPI Registry actor representations.
    /// Provides seamless integration between the OmiLAXR analytics framework and the xAPI Registry
    /// by handling the transformation of various actor types (learners, instructors, teams) into
    /// standardized xAPI_Actor instances for consistent statement generation.
    /// </summary>
    public static class xApiRegistry_Ext
    {
        /// <summary>
        /// Converts a generic Actor instance to an xAPI Registry compatible xAPI_Actor.
        /// Maps the actor's name and email properties to the standardized xAPI actor format
        /// used throughout the xAPI Registry system for consistent actor identification.
        /// </summary>
        /// <param name="actor">The Actor instance to convert</param>
        /// <returns>xAPI_Actor instance with mapped name and email properties</returns>
        public static xAPI_Actor ToXAPIActor(this Actor actor)
            => new xAPI_Actor(actor.actorName, actor.actorEmail);
        
        /// <summary>
        /// Converts a collection of Actor instances to xAPI Registry compatible xAPI_Actor instances.
        /// Enables batch conversion of multiple actors for scenarios involving groups,
        /// teams, or multiple learners in collaborative learning environments.
        /// </summary>
        /// <param name="actors">Collection of Actor instances to convert</param>
        /// <returns>IEnumerable of xAPI_Actor instances with converted actor data</returns>
        public static IEnumerable<xAPI_Actor> ToXAPIActors(this IEnumerable<Actor> actors)
            => actors.Select(a => a.ToXAPIActor());
        
        /// <summary>
        /// Converts an Instructor instance to an xAPI Registry compatible xAPI_Actor.
        /// Handles the specific case of instructor actors by mapping instructor-specific
        /// properties to the standardized xAPI actor format for educational context tracking.
        /// </summary>
        /// <param name="actor">The Instructor instance to convert</param>
        /// <returns>xAPI_Actor instance representing the instructor in xAPI format</returns>
        public static xAPI_Actor ToXAPIActor(this Instructor actor)
            => new xAPI_Actor(actor.instructorName, actor.instructorEmail);
        
        /// <summary>
        /// Converts a Team instance to an xAPI Registry compatible xAPI_Actor.
        /// Enables team-based analytics by treating teams as collective actors
        /// within the xAPI framework for collaborative learning scenario tracking.
        /// </summary>
        /// <param name="team">The Team instance to convert</param>
        /// <returns>xAPI_Actor instance representing the team as a collective actor</returns>
        public static xAPI_Actor ToXAPIActor(this Team team)
            => new xAPI_Actor(team.teamName, team.teamEmail);
    }
}
#endif