#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / System Composer (xAPI)")]
    [Description("Creates statements:\n- actor started game with name(String)" +
                 "\n- actor ended game with name(String)" +
                 "\n- actor paused/resumed game with name(String)" +
                 "\n- actor focused/unfocused game with name(String)")]
    public sealed class SystemComposer : xApiComposer<SystemTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(SystemTrackingBehaviour tb)
        {
            tb.OnStartedGame.AddHandler((owner, timestamp) =>
            {
                print("started game");
                var stmt = actor.Does(xapi.systemControl.verbs.started)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });

            tb.OnQuitGame.AddHandler((owner, timestamp) =>
            {
                var stmt = actor.Does(xapi.systemControl.verbs.ended)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });
            
            tb.OnPausedGame.AddHandler((owner, timestamp, isPaused) =>
            {
                var verb = isPaused ? xapi.systemControl.verbs.paused : xapi.systemControl.verbs.resumed;
                var stmt = actor.Does(verb)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });
            
            tb.OnFocusedGame.AddHandler((owner, timestamp, isFocused) =>
            {
                var verb = isFocused ? xapi.systemControl.verbs.focused : xapi.systemControl.verbs.unfocused;
                var stmt = actor.Does(verb)
                    .Activity(xapi.systemControl.activities.game)
                    .WithTimestamp(timestamp)
                    .WithExtension(xapi.systemControl.extensions.activity.name(Application.productName));
                SendStatement(owner, stmt);
            });
        }
    }
}
#endif