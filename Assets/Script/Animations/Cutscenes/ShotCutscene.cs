﻿using UnityEngine;
using System.Collections;
using System;

public class ShotCutscene : ACutscene {

    public long duration;
    private long startTime;
    private float executionTime;

    public override void StartCutscene()
    {
        base.StartCutscene();
        startTime = System.DateTime.Now.Ticks;

    }

    public override void UpdateCutscene()
    {
        TimeSpan ts = TimeSpan.FromTicks(System.DateTime.Now.Ticks - startTime);
        if (ts.TotalSeconds >= duration)
            StopCutscene();
    }

}
