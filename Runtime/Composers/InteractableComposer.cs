#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.Learner;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Interactable Composer"),
     Description("Creates statements:\n- actor interacted/grabbed/touched/released vrObject with vrObjectName(String)")]
    public class InteractableComposer : xApiComposer<InteractableTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(InteractableTrackingBehaviour tb)
        {
            tb.OnInteracted.AddHandler((_, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.interacted)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()));
                SendStatement(stmt);
            });
            tb.OnGrabbed.AddHandler((sender, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.grabbed)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()));
                SendStatement(stmt);
            });
            tb.OnTouched.AddHandler((sender, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.touched)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()));
                SendStatement(stmt);
            });
            tb.OnReleased.AddHandler((sender, e) =>
            {
                var go = e.Target;
                var stmt = actor.Does(xapi.virtualReality.verbs.released)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.GetTrackingName()));
                SendStatement(stmt);
            });
        }
    }
}
#endif