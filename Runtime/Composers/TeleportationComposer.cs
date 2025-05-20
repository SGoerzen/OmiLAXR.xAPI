#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.UnityXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Teleportation Composer (xAPI)"),
     Description("Creates statements:\n- actor teleported teleportPoint/teleportArea with vrObjectName(String) and result startValue(Vector3), endValue(Vector3), cameraYOffset(Float)")]
    public class TeleportationComposer : xApiComposer<TeleportationTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

        protected override void Compose(TeleportationTrackingBehaviour tb)
        {
           tb.OnTeleported.AddHandler((_, args) =>
           {
               var activity = args.TargetType == TeleportationTrackingBehaviour.TeleportationTargetType.Anchor ? 
                   xapi.virtualReality.activities.teleportPoint : xapi.virtualReality.activities.teleportArea;
               var stmt = actor.Does(xapi.virtualReality.verbs.teleported)
                   .WithResult(xapi.generic.extensions.result
                       .startValue(args.StartState.FloorPosition)
                       .endValue(args.EndState.FloorPosition))
                   .WithResult(xapi.virtualReality.extensions.result.cameraYOffset(args.CameraYOffset))
                   .Activity(activity)
                   .WithExtension(xapi.virtualReality.extensions.activity
                       .vrObjectName(args.Target.gameObject.GetTrackingName()));
               SendStatement(stmt);
           });
        }
    }
}
#endif