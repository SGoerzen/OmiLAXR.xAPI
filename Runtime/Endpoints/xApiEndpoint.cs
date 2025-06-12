using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
using OmiLAXR.xAPI.Composers;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Endpoints
{
    public abstract class xApiEndpoint : Endpoint
    {
        protected static xAPI_Contexts xapi => xApiRegistry.definitions;

        protected override TransferCode HandleSending(IStatement statement)
        {
            if (!(statement is xApiStatement))
                return TransferCode.NoStatements;
            var stmt = statement as xApiStatement;
            return HandleSending(stmt);
        }
        
        protected abstract TransferCode HandleSending(xApiStatement statement);
    }
}