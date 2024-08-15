using OmiLAXR.Composers;
using OmiLAXR.TrackingBehaviours;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Composers
{
    public abstract class xApiStatementComposer<T> : BindedStatementComposer<T>
        where T : TrackingBehaviour
    {
        protected readonly xApiStatement.Actor actor = new xApiStatement.Actor();
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();
        protected xApiDataProvider[] dataProviders;

        protected override void Awake()
        {
            base.Awake();
            dataProviders = FindObjectsOfType<xApiDataProvider>();
        }

        protected void SendStatement(xApiStatement statement)
        {
            
        }
    }
}