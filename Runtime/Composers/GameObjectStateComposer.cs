using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours.System;
using OmiLAXR.xAPI.Actors;
using UnityEngine;

namespace OmiLAXR.xAPI.Composers
{
    [AddComponentMenu("OmiLAXR / 4) Composers / Mouse Statement Composer (xAPI)")]
    public sealed class GameObjectStateComposer : XApiComposer<GameObjectsStateTrackingBehaviour>
    {
        protected override Author GetAuthor()
            => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");
        protected override void Compose(GameObjectsStateTrackingBehaviour tb)
        {
            tb.OnDestroyedGameObject += (_, o) =>
            {
                var statement = actor.Does(xapi.virtualReality.verbs.removed)
                    .Activity(xapi.virtualReality.activities.vrObject)
                    .WithExtension(xapi.virtualReality.extensions.activity.vrObjectName(o.name));
                SendStatement(statement);
            };
        }
    }
}