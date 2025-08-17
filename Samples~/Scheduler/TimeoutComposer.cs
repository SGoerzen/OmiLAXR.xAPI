/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
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
