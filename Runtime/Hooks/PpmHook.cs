#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.Actors.HeartRate;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Peripheral Physiological Measures (PPM) Hook (xAPI)")]
    [Description("Adds peripheral physiological measures values to each statement.")]
    public class PpmHook : xApiHook
    {
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            var heartRateProvider = GetProvider<HeartRateProvider>(statement);
            
            if (heartRateProvider && heartRateProvider.enabled)
            {
                statement.WithResult(xapi.ppm.extensions.result.heartRate(heartRateProvider.GetHeartRate()));
            }

            return statement;
        }
    }
}
#endif