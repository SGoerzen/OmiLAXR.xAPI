using System.ComponentModel;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Timestamp Pseudonymizer Hook (xAPI)"),
     Description("Adds to the timestamp an hour offset (default: 10 days ago). This way the timestamps will be pseudonymized.")]   
    public class TimestampPseudonymizerHook : xApiHook
    {
        public int hourOffset = -24 * 10; // -10 days
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