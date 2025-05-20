using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    public class SessionComposer : xApiComposer<SessionTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(SessionTrackingBehaviour tb)
        {
            tb.OnSessionStarted.AddHandler((_, startDate) =>
            {
                Debug.Log("Started session");
                if (Registration.Instance && Registration.Instance.enabled)
                {
                    var stmt = actor.Does(xapi.systemControl.verbs.started)
                        .Activity(xapi.systemControl.activities.session)
                        .WithTimestamp(startDate)
                        .WithExtension(xapi.systemControl.extensions.activity.name(Registration.Instance.uuid));  
                    SendStatement(stmt, immediate: true);
                }
            });
            tb.OnSessionStopped.AddHandler((_, stopDate) =>
            {
                if (Registration.Instance && Registration.Instance.enabled)
                {
                    var stmt = actor.Does(xapi.systemControl.verbs.ended)
                        .Activity(xapi.systemControl.activities.session)
                        .WithTimestamp(stopDate)
                        .WithExtension(xapi.systemControl.extensions.activity.name(Registration.Instance.uuid));
                    SendStatement(stmt, immediate: true);
                }
            });
        }
    }
}