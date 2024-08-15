using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public abstract class xApiStatementComposer<T> : BindedStatementComposer<T>
        where T : TrackingBehaviour
    {
        protected xApiStatement.ActorRole actor;
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();
        
        protected override void Awake()
        {
            base.Awake();
            actor = new xApiStatement.ActorRole(trackingBehaviour.GetActor());
        }

        
    }
}