using OmiLAXR.Composers;
using OmiLAXR.xAPI;
using OmiLAXR.xAPI.Composers;

public class IntervalComposer : xApiComposer<IntervalTrackingBehaviour>
{
    public override Author GetAuthor()
        => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

    protected override void Compose(IntervalTrackingBehaviour tb)
    {
        tb.OnTickEvent.AddHandler(owner =>
        {
            // place your statement here
            DebugLog.xAPI.Print("=============Interval Example tick================");
        });
    }
}
