#if XAPI_REGISTRY_EXISTS
using System;
using System.ComponentModel;
using OmiLAXR.Types;
using OmiLAXR.xAPI.Composers;
using UnityEngine;

namespace OmiLAXR.xAPI.Hooks
{
    [AddComponentMenu("OmiLAXR / 5) Hooks / Reference Timestamp Pseudonmizer (xAPI)"),
     Description("Sets a fixed Date to the timestamp, thus the day of the collected data is pseudonymized.")]
    public class ReferenceTimestampPseudonmizerHook : xApiHook
    {
        private DateTime? _firstDateTime;
        [Tooltip("The time of the first statement will be by default 00:00:00.000, except you set Keep original time true.")]
        public bool keepOriginalTime = false;
        public SerializableDateTime referenceDateTime = SerializableDateTime.Now;
        private DateTime AnonymizeTimestamp(DateTime dateTime)
        {
            if (!keepOriginalTime && !_firstDateTime.HasValue)
            {
                _firstDateTime = dateTime;
            }
    
            var rdt = referenceDateTime;
    
            if (keepOriginalTime)
            {
                // Just use the time from the input dateTime with the reference date
                return new DateTime(
                    rdt.year,
                    rdt.month,
                    rdt.day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond);
            }
            else
            {
                // Calculate elapsed time since first timestamp
                var elapsed = dateTime - _firstDateTime!.Value;
        
                // Create a new DateTime using the reference date and the elapsed time
                return new DateTime(rdt.year, rdt.month, rdt.day)
                    .Add(elapsed);
            }
        }
        protected override xApiStatement AfterCompose(xApiStatement statement)
        {
            if (!statement.GetTimestamp().HasValue)
                return statement;
            statement.WithTimestamp(AnonymizeTimestamp(statement.GetTimestamp()!.Value));
            return statement;
        }
    }
}
#endif