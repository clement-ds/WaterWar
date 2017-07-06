using UnityEngine;
using System.Collections;
using System;

public class TravelCutscene : ACutscene {

    public long duration;
    private long startTime;
    private float executionTime;
    public Animation anim;

    public override void StartCutscene()
    {
        base.StartCutscene();
        startTime = System.DateTime.Now.Ticks;
        anim.Play();

}

public override void UpdateCutscene()
    {
        TimeSpan ts = TimeSpan.FromTicks(System.DateTime.Now.Ticks - startTime);
        if (ts.TotalSeconds >= duration)
            StopCutscene();
    }

}
