/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.xAPI.
*/
using OmiLAXR;
using OmiLAXR.Schedules;
using OmiLAXR.TrackingBehaviours;

public class TimeoutTrackingBehaviour : TrackingBehaviour
{
    public TimeoutTicker.Settings settings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public readonly TrackingBehaviourEvent OnTickEvent = new TrackingBehaviourEvent();
    protected override void OnStartedPipeline(Pipeline pipeline)
    {
        // Using settings
        SetTimeout(() =>
        {
            OnTickEvent?.Invoke(this);
        }, settings);
        
        // Timeout after 3 seconds
        SetTimeout(() =>
        {
            OnTickEvent?.Invoke(this);
        }, 3);
    }
}
