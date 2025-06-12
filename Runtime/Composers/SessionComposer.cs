#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Session Composer (xAPI)"),
    Description("Creates statements: \n- actor started session with timestamp(DateTime), name(UUID) " +
                "\n- actor ended session with timestamp(DateTime), name(UUID)")]
    public class SessionComposer : xApiComposer<SessionTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(SessionTrackingBehaviour tb)
        {
            tb.OnSessionStarted.AddHandler((owner, startDate) =>
            {
                if (Registration.Instance && Registration.Instance.enabled)
                {
                    DebugLog.xAPI.Print($"Started session with UUID: {Registration.Instance.uuid}.");
                    var stmt = actor.Does(xapi.systemControl.verbs.started)
                        .Activity(xapi.systemControl.activities.session)
                        .WithTimestamp(startDate)
                        .WithExtension(xapi.systemControl.extensions.activity.name(Registration.Instance.uuid));  
                    SendStatement(owner, stmt);
                }
                else
                {
                    DebugLog.xAPI.Print("Started session.");
                }
            });
            tb.OnSessionStopped.AddHandler((owner, stopDate) =>
            {
                if (Registration.Instance && Registration.Instance.enabled)
                {
                    DebugLog.xAPI.Print($"Stopped session with UUID: {Registration.Instance.uuid}.");
                    var stmt = actor.Does(xapi.systemControl.verbs.ended)
                        .Activity(xapi.systemControl.activities.session)
                        .WithTimestamp(stopDate)
                        .WithExtension(xapi.systemControl.extensions.activity.name(Registration.Instance.uuid));
                    SendStatement(owner, stmt);
                }
                else
                {
                    DebugLog.xAPI.Print("Stopped session.");
                }
            });
        }
    }
}
#endif