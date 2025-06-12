#if XAPI_REGISTRY_EXISTS
using System.ComponentModel;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Timestamp Anonymizer Hook (xAPI)"),
     Description("Adds to the timestamp a random hour offset. This way the timestamps will be anonymized.")]   
    public class TimestampAnonymizerHook : xApiHook
    {
        private void Start()
        {
            hourOffset = UnityEngine.Random.Range(hourOffsetMin, hourOffsetMax);
        }

        [ReadOnly]
        public int hourOffset = 0;
        
        public int hourOffsetMin = -24 * 30;
        public int hourOffsetMax = 24 * 30;
        
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            if (!statement.GetTimestamp().HasValue)
                return statement;
            var newTimestamp = statement.GetTimestamp()!.Value.AddHours(hourOffset);
            statement.WithTimestamp(newTimestamp);
            return statement;
        }
    }
}
#endif