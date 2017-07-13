using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public enum CrewMember_Job { Captain, Pirate, Medic, Engineer };

public enum Effect { SPEED, AVAILABLE, MORAL, ENERGY }

public class SkillAttribute
{
    public static readonly SkillAttribute RCanonTime = new SkillAttribute(Effect.ENERGY, "RCanonTime");
    public static readonly SkillAttribute ShootCanonValue = new SkillAttribute(Effect.MORAL, "ShootCanonValue");
    public static readonly SkillAttribute RepairTime = new SkillAttribute(Effect.ENERGY, "RepairTime");
    public static readonly SkillAttribute RepairValue = new SkillAttribute(Effect.MORAL, "ReapirValue");
    public static readonly SkillAttribute WalkValue = new SkillAttribute(Effect.SPEED, "WalkValue");

    public static IEnumerable<SkillAttribute> Values
    {
        get
        {
            yield return RCanonTime;
            yield return ShootCanonValue;
            yield return RepairTime;
            yield return RepairValue;
            yield return WalkValue;
        }
    }

    private readonly Effect range;
    private readonly string id;

    SkillAttribute(Effect range, string id)
    {
        this.range = range;
        this.id = id;
    }

    public Effect Range { get { return range; } }
    public string Name { get { return id; } }
}

public class CrewMember_Effect
{
    public Effect effect;
    public TimerTask task;
    public float value;
    public bool available;

    public CrewMember_Effect(Effect effect, float timer, float value)
    {
        this.effect = effect;
        if (timer > 0)
        {
            this.task = new TimerTask(stop, timer);
        }
        this.value = value;
        this.available = true;
    }

    public void update()
    {
        if (this.task != null)
        {
            this.task.update();
        }
    }

    public void stop()
    {
        this.available = false;
    }
}

[Serializable]
public abstract class CrewMember
{

    public string id;
    public string type;
    public string memberName;
    public float attackStrength = 1f;
    public bool useRangedWeapon = false;
    public float wage = 1f;
    public float maxHunger = 1f;
    public float walkSpeed = 1f;
    public float maxLife = 100f;
    public float life = 10f;
    public float satiety = 1f;

    public bool available = true;
    public CrewMember_Job job;
    public Ship_Item assignedRoom;
    public string memberImage = "None";
    protected List<CrewMember_Effect> attributes;
    protected List<KeyValuePair<SkillAttribute, float>> skills;

    public CrewMember(string id, CrewMember_Job job)
    {
        this.id = id;
        type = this.GetType().Name.Substring(this.GetType().Name.IndexOf("_") + 1);
        memberName = id;
        this.job = job;
        this.attributes = new List<CrewMember_Effect>();
        this.skills = new List<KeyValuePair<SkillAttribute, float>>();
    }

    void update()
    {
        for (int i = 0; i < attributes.Count; ++i)
        {
            attributes[i].update();
            if (attributes[i].available)
            {
                attributes.RemoveAt(i);
                --i;
            }
        }
    }

    /** STATUS **/
    public void getDamage(float damage)
    {
        this.life -= damage;
        if (this.life <= 0)
        {
            this.life = 0;
            this.available = false;
        }
    }

    public void healDamage(float heal)
    {
        this.life += heal;
        this.life = (this.life > this.maxLife ? this.maxLife : this.life);
        this.available = true;
    }

    /** ACTION **/

    public float getValueByCrewSkill(SkillAttribute skill, float value)
    {
        CrewMember_Effect effect = this.getEffect(skill.Range);

        if (this.skills.Exists(x => x.Key.Name == skill.Name))
        {
            value = value * this.skills.Find(x => x.Key.Name == skill.Name).Value / 100;
        }

        if (value != -1 && effect != null)
        {
            value -= (value * effect.value / 100);
        }
        return value;
    }

    public void AdjustWage(float newWage)
    {
        this.wage = newWage;
    }

    /** Attributes **/

    public void addEffect(Effect effect, float timer, float value)
    {
        this.removeEffect(effect);
        this.attributes.Add(new CrewMember_Effect(effect, timer, value));
    }

    public void removeEffect(Effect effect)
    {
        CrewMember_Effect item = this.getEffect(effect);

        if (item != null)
        {
            this.attributes.Remove(item);
        }
    }

    public CrewMember_Effect getEffect(Effect effect)
    {
        return this.attributes.Find(x => x.effect == effect);
    }
}
