using OmiLAXR.TrackingBehaviours.System;

namespace OmiLAXR.xAPI.Composers
{
    public class GameObjectStateStatementComposer : xApiStatementComposer<GameObjectsStateTrackingBehaviour>
    {
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