using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Transform Composer (xAPI)")]
    [Description("Creates statements:\n- actor moved/rotated/scaled vrObject with vrObjectName(name) and result position, rotation, scale, startValue, endValue")]
    public class TransformComposer : xApiComposer<TransformTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(TransformTrackingBehaviour tb)
        {
            tb.OnChangedPosition.AddHandler((_, tw, change) =>
            {
                var t = tw.transform;
                var stmt = actor.Does(xapi.virtualReality.verbs.moved)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .vrObjectName(tw.GetTrackingName()))
                    .WithResult(xapi.virtualReality.extensions.result
                        .position(t.position)
                        .rotation(t.eulerAngles)
                        .scale(t.localScale))
                    .WithResult(xapi.generic.extensions.result
                        .startValue(change.OldValue)
                        .endValue(change.NewValue));
                SendStatement(stmt);
            });
            tb.OnChangedRotation.AddHandler((_, tw, change) =>
            {
                var t = tw.transform;
                var stmt = actor.Does(xapi.virtualReality.verbs.rotated)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .vrObjectName(tw.GetTrackingName()))
                    .WithResult(xapi.virtualReality.extensions.result
                        .position(t.position)
                        .rotation(t.eulerAngles)
                        .scale(t.localScale))
                    .WithResult(xapi.generic.extensions.result
                        .startValue(change.OldValue)
                        .endValue(change.NewValue));
                SendStatement(stmt);
            });
            tb.OnChangedScale.AddHandler((_, tw, change) =>
            {
                var t = tw.transform;
                var stmt = actor.Does(xapi.virtualReality.verbs.scaled)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity
                        .vrObjectName(tw.GetTrackingName()))
                    .WithResult(xapi.virtualReality.extensions.result
                        .position(t.position)
                        .rotation(t.eulerAngles)
                        .scale(t.localScale))
                    .WithResult(xapi.generic.extensions.result
                        .startValue(change.OldValue)
                        .endValue(change.NewValue));
                SendStatement(stmt);
            });
        }
    }
}
