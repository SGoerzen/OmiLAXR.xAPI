using OmiLAXR;
using OmiLAXR.Schedules;
using OmiLAXR.TrackingBehaviours;

public class IntervalTrackingBehaviour : TrackingBehaviour
{
    public IntervalTicker.Settings settings;
    public readonly TrackingBehaviourEvent OnTickEvent = new TrackingBehaviourEvent();
    protected override void OnStartedPipeline(Pipeline pipeline)
    {
        // Using settings
        SetInterval(() =>
        {
            OnTickEvent?.Invoke(this);
        }, settings);
        
        // Interval with 3 seconds
        SetInterval(() =>
        {
            OnTickEvent?.Invoke(this);
        }, 3);
    }
}
