using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Game Object State Composer (xAPI)")]
    [Description("Creates statements:\n- actor removed vrObject with vrObjectName(name)")]
    public sealed class GameObjectStateComposer : xApiComposer<GameObjectsStateTrackingBehaviour>
    {
        public override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(GameObjectsStateTrackingBehaviour tb)
        {
            tb.OnDestroyedGameObject.AddHandler((_tb, _sender, go) =>
            {
                var statement = actor.Does(xapi.virtualReality.verbs.removed)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(go.name));
                SendStatement(statement);
            });
        }
    }
}