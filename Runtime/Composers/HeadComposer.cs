using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Head Statement Composer (xAPI)")]
    [Description("Creates statements:\n- actor nodded/shaked head with timeSpan(TimeSpan), numberOfGestures(Int)")]
    public class HeadComposer : xApiComposer<HeadTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(HeadTrackingBehaviour tb)
        {
            tb.OnNodded.AddHandler((sender, args) =>
            {
                var stmt = actor.Does(xapi.gestures.verbs.nodded)
                    .Activity(xapi.gestures.activities.head)
                    .WithResult(xapi.generic.extensions.result.timeSpan(args.TimeSpan))
                    .WithResult(xapi.gestures.extensions.result.numberOfGestures(args.NumberOfGesture));
                SendStatement(stmt);
            });
            tb.OnShook.AddHandler((sender, args) =>
            {
                var stmt = actor.Does(xapi.gestures.verbs.shaked)
                    .Activity(xapi.gestures.activities.head)
                    .WithResult(xapi.generic.extensions.result.timeSpan(args.TimeSpan))
                    .WithResult(xapi.gestures.extensions.result.numberOfGestures(args.NumberOfGesture));
                SendStatement(stmt);
            });
        }
    }
}