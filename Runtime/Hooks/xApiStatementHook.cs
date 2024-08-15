
using OmiLAXR.Composers;
using OmiLAXR.Hooks;
using OmiLAXR.xAPI.Composers;
using UnityEngine;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / xAPI Statement Hook")]
    public abstract class xApiStatementHook : StatementHook
    {
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();

        public override IStatement AfterCompose(IStatement statement)
            => statement.GetType() != typeof(xApiStatement) ? statement : AfterComposeXApi(statement as xApiStatement);

        protected abstract xApiStatement AfterComposeXApi(xApiStatement statement);
    }
}