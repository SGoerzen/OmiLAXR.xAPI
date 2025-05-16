using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public abstract class xApiComposer<T> : Composer<T>
        where T : ActorPipelineComponent, ITrackingBehaviour
    {
        /// <summary>
        /// Who is responsible for the correctness of the statement.
        /// </summary>
        protected xApiStatement.ActorRole actor;
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();
        
        protected override void Awake()
        {
            base.Awake();
            if (!trackingBehaviour)
            {
                // disable if corresponding Tracking Behaviour is disabled
                enabled = false;
                return;
            }
            actor = new xApiStatement.ActorRole(trackingBehaviour.GetActor(), GetAuthor(), trackingBehaviour.GetInstructor());
        }

        
    }
}