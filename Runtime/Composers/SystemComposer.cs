using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using OmiLAXR.xAPI.Actors;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / System Statement Composer (xAPI)")]
    public sealed class SystemComposer : XApiComposer<SystemTrackingBehaviour>
    {
        protected override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(SystemTrackingBehaviour tb)
        {
            tb.OnGameStarted.AddHandler(_ =>
            {
                var statement = actor.Does(xapi.systemControl.verbs.started)
                    .Activity(xapi.systemControl.activities.game)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                
                SendStatement(statement);
            });

            tb.OnGameQuit.AddHandler(_ =>
            {
                var statement = actor.Does(xapi.systemControl.verbs.ended)
                    .Activity(xapi.systemControl.activities.game)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                
                SendStatement(statement);
            });
        }
    }
}