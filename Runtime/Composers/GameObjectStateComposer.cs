#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Game Object State Composer (xAPI)")]
    [Description("Creates statements:\n- actor deleted vrObject with vrObjectName(String)")]
    public sealed class GameObjectStateComposer : xApiComposer<GameObjectStateTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(GameObjectStateTrackingBehaviour tb)
        {
            tb.OnDestroyedGameObject.AddHandler((_tb, _sender, go) =>
            {
                var statement = actor.Does(xapi.generic.verbs.deleted)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.name));
                SendStatement(statement);
            });
            
            tb.OnEnabledGameObject.AddHandler((_tb, _sender, go) =>
            {
                // todo
            });
            
            tb.OnDisabledGameObject.AddHandler((_tb, _sender, go) =>
            {
                // todo
            });
        }
    }
}
#endif