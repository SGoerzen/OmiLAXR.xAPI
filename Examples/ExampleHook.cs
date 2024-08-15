using System;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Hooks;
using UnityEngine;

namespace OmiLAXR.xAPI.Examples
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Example Hook (xAPI)")]
    public class ExampleHook : xApiStatementHook
    {
        private void Start()
        {
            
        }

        protected override xApiStatement AfterComposeXApi(xApiStatement statement)
        {
            // change all clicked verbs by interacted
            if (statement.MatchKeys("clicked", "uiElement"))
            {
                DebugLog.xAPI.Print($"Matches keys 'clicked' and 'uiElement': {statement}");
                statement.Verb(xapi.virtualReality.verbs.interacted);
                DebugLog.xAPI.Print($"Exchanged 'clicked' by 'interacted': {statement}");
            }

            // Add additional extension
            if (statement.GetActivity().MatchesPath("virtualReality.activities.uiElement"))
            {
                DebugLog.xAPI.Print($"Matches path 'virtualReality.activities.uiElement': {statement}");
                statement.WithExtension(xapi.generic.extensions.activity.color("red"));
                DebugLog.xAPI.Print($"Added color 'red': {statement}");
            }
            
            // Exchange extension values
            if (statement.HasExtension("virtualReality.extensions.activity.uiElementValue"))
            {
                DebugLog.xAPI.Print($"Has extension 'virtualReality.extensions.activity.uiElementValue': {statement}");
                statement.WithExtension(xapi.virtualReality.extensions.activity.uiElementValue("exchanged"));
                DebugLog.xAPI.Print($"Exchanged 'virtualReality.extensions.activity.uiElementValue' to 'exchanged': {statement}");
            }
            
            // Exchange extension values
            if (statement.HasExtension("virtualReality.extensions.activity.uiElementMinValue"))
            {
                DebugLog.xAPI.Print($"Has extension 'virtualReality.extensions.activity.uiElementMinValue': {statement}");
                statement.DropExtension("virtualReality.extensions.activity.uiElementMinValue");
                DebugLog.xAPI.Print($"Dropped extension by path 'virtualReality.extensions.activity.uiElementMinValue': {statement}");
            }
            
            return statement;
        }
    }
}