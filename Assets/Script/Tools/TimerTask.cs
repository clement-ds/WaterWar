using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerTask
{
    public delegate void TaskDelegate();

    public TaskDelegate taskDelegate;

    public float cooldown;
    private float current;

    public TimerTask(TaskDelegate task, float cooldown)
    {
        this.taskDelegate = task;
        this.cooldown = cooldown;
        this.current = cooldown;
    }

    public void update()
    {
        this.current -= Time.deltaTime;

        if (current <= 0)
        {
            taskDelegate();
            this.current = this.cooldown;
        }
    }
}