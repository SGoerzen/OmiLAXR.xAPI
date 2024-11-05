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
            
            if (HeartRateMonitor.Instance && HeartRateMonitor.Instance.enabled)
            {
                var provider = HeartRateMonitor.Instance.provider;
                if (!provider)
                {
                    DebugLog.xAPI.Error("Cannot provider heart rate as no heart rate provider is assigned to the Heart Rate Monitor.");
                }
                else if (provider.enabled)
                {
                    statement.WithResult(xapi.ppm.extensions.result.heartRate(provider.GetHeartRate()));
                }
            }

            return statement;
        }
    }
}