/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
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
