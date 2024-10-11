using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / System Statement Composer (xAPI)")]
    public sealed class SystemComposer : xApiComposer<SystemTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(SystemTrackingBehaviour tb)
        {
            tb.OnStartedGame.AddHandler((_, timestamp) =>
            {
                var stmt = actor.Does(xapi.systemControl.verbs.started)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(stmt);
            });

            tb.OnQuitGame.AddHandler((_, timestamp) =>
            {
                var stmt = actor.Does(xapi.systemControl.verbs.ended)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(stmt);
            });
            
            tb.OnPausedGame.AddHandler((_, timestamp, isPaused) =>
            {
                var verb = isPaused ? xapi.systemControl.verbs.paused : xapi.systemControl.verbs.resumed;
                var stmt = actor.Does(verb)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(stmt);
            });
            
            tb.OnFocusedGame.AddHandler((_, timestamp, isFocused) =>
            {
                // need to deploy to registry
                //var verb = isFocused ? xapi.systemControl.verbs.focused : xapi.systemControl.verbs.unfocused;
                //var stmt = actor.Does(verb)
                //    .Activity(xapi.systemControl.activities.game)
                //    .WithTimestamp(timestamp)
                //    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                //SendStatement(stmt);
            });
        }
    }
}