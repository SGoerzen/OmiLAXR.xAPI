using System;
using System.IO;
using OmiLAXR.Composers;
using UnityEngine;
using OmiLAXR.Endpoints;
using OmiLAXR.Utils;
using OmiLAXR.xAPI;
using OmiLAXR.xAPI.Composers;
using OmiLAXR.xAPI.Endpoints;

public class PositionDataCsvEndpoint : xApiEndpoint
{
    private StreamWriter _streamWriter;
    private void Start()
    {
        var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "omilaxr_statements");
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        
        var filePath = Path.Combine(directoryPath, "positionData.csv");
        var wasExists = File.Exists(filePath);
        
        _streamWriter = new StreamWriter(filePath, append: true);
        _streamWriter.AutoFlush = false;
        
        if (!wasExists)
            _streamWriter.WriteLine("id,timestamp,position");
    }

    protected override TransferCode HandleSending(xApiStatement statement)
    {
        // ignore not relevant statements
        if (!statement.IsFromComposer<TransformComposer>())
            return TransferCode.NoStatements;
        
        var id = statement.GetId();
        var timestamp = statement.GetTimestampString();
        var positionData = statement.GetValue<Vector3>(xapi.virtualReality.extensions.result.position);

        var csvLine = $"{id},{timestamp},{CsvFormat.FormatCsvValue(positionData).Replace(" ", "")}";
        print(csvLine);
        _streamWriter.WriteLine(csvLine);
        _streamWriter.Flush();
        
        return TransferCode.Success;
    }
}