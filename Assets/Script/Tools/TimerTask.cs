using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerTask
{
    public delegate void TaskDelegate();

    public TaskDelegate taskDelegate;

    public float cooldown;
    private float current;
    private bool canRun;

    public TimerTask(TaskDelegate task, float cooldown) : this(task, cooldown, true)
    {
    }

    public TimerTask(TaskDelegate task, float cooldown, bool canRun)
    {
        this.taskDelegate = task;
        this.cooldown = cooldown;
        this.current = cooldown;
        this.canRun = canRun;
    }

    public void update()
    {
        if (this.canRun)
        {
            this.current -= Time.deltaTime;

            if (current <= 0)
            {
                taskDelegate();
                this.current = this.cooldown;
            }
        }
    }

    public void start()
    {
        this.canRun = true;
    }

    public void stop()
    {
        this.canRun = false;
    }
}