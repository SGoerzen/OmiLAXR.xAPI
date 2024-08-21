
using OmiLAXR.Composers;
using OmiLAXR.Hooks;
using OmiLAXR.xAPI.Composers;
using UnityEngine;
using xAPI.Registry;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / xAPI Hook")]
    public abstract class xApiHook : Hook
    {
        protected readonly xAPI_Contexts xapi = new xAPI_Contexts();

        public override IStatement AfterCompose(IStatement statement)
            => statement.GetType() != typeof(xApiStatement) ? statement : AfterCompose(statement as xApiStatement);

        protected abstract xApiStatement AfterCompose(xApiStatement statement);
    }
}