using OmiLAXR.TrackingBehaviours.System;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / System Statement Composer (xAPI)")]
    public class SystemStatementComposer : xApiStatementComposer<SystemTrackingBehaviour>
    {
        protected override void Compose(SystemTrackingBehaviour tb)
        {
            tb.OnGameStarted += _ =>
            {
                var statement = actor.Does(xapi.systemControl.verbs.started)
                    .Activity(xapi.systemControl.activities.game)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                
                SendStatement(statement);
            };

            tb.OnGameQuit += _ =>
            {
                var statement = actor.Does(xapi.systemControl.verbs.ended)
                    .Activity(xapi.systemControl.activities.game)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                
                SendStatement(statement);
            };
        }
    }
}