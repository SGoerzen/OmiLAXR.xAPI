using System;
using System.IO;
using OmiLAXR.Endpoints;
using OmiLAXR.Utils;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Endpoints;

public class CsvFileExampleEndpoint : xApiEndpoint
{
    private CsvFile _csv;
    protected override void OnEnable()
    {
        base.OnEnable();
        var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "omilaxr_statements");
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        
        var filePath = Path.Combine(directoryPath, "customCsvFormatData.csv");
        
        _csv = new CsvFile(filePath, autoFlush: true);
        _csv.SetHeaders("id", "timestamp", "actor", "verb", "object");
    }

    protected override TransferCode HandleSending(xApiStatement statement)
    {
        if (_csv == null)
            return TransferCode.Error;
        var id = statement.GetId();
        var timestamp = statement.GetTimestampString();
        var actor = statement.GetActor().Name;
        var verb = statement.GetVerb().Key;
        var activity = statement.GetActivity().Key;
        
        _csv.WriteRow(id, timestamp, actor, verb, activity);
        
        return TransferCode.Success;
    }
}
