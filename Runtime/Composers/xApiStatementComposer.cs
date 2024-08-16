using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public abstract class xApiStatementComposer<T> : BindedStatementComposer<T>
        where T : TrackingBehaviour
    {
        /// <summary>
        /// Who is responsible for the correctness of the statement.
        /// </summary>
        protected xApiStatement.ActorRole actor;
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();
        
        protected override void Awake()
        {
            base.Awake();
            actor = new xApiStatement.ActorRole(trackingBehaviour.GetActor(), GetAuthor());
        }

        
    }
}