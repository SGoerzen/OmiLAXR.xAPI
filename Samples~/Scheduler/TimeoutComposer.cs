using OmiLAXR.Composers;
using OmiLAXR.xAPI;
using OmiLAXR.xAPI.Composers;

public class TimeoutComposer : xApiComposer<TimeoutTrackingBehaviour>
{
    public override Author GetAuthor()
        => new Author("Sergej Görzen", "goerzen@cs.rwth-aachen.de");

    protected override void Compose(TimeoutTrackingBehaviour tb)
    {
        print("Compose");
        tb.OnTickEvent.AddHandler(owner =>
        {
            // place your statement here
            DebugLog.xAPI.Print("=============Timeout Example tick================");
        });
    }
}
