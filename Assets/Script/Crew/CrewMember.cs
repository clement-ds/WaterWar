using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public enum CrewMember_Job { Captain, Pirate, Medic, Engineer };

public enum Attribute { SPEED, AVAILABLE, MORAL, ENERGY }

public class SkillAttribute
{
    public static readonly SkillAttribute RCanonTime = new SkillAttribute(Attribute.ENERGY, "RCanonTime");
    public static readonly SkillAttribute ShootCanonValue = new SkillAttribute(Attribute.MORAL, "ShootCanonValue");
    public static readonly SkillAttribute RepairTime = new SkillAttribute(Attribute.ENERGY, "RepairTime");
    public static readonly SkillAttribute RepairValue = new SkillAttribute(Attribute.MORAL, "ReapirValue");
    public static readonly SkillAttribute WalkValue = new SkillAttribute(Attribute.SPEED, "WalkValue");

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

    private readonly Attribute range;
    private readonly string id;

    SkillAttribute(Attribute range, string id)
    {
        this.range = range;
        this.id = id;
    }

    public Attribute Range { get { return range; } }
    public string Name { get { return id; } }
}

public class CrewMember_Attribute
{
    public Attribute effect;
    public TimerTask task;
    public float value;
    public bool available;

    public CrewMember_Attribute(Attribute effect, float timer, float value)
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
public class CrewMember
{

    public string id;
    public string type;
    public string memberName;
    public float attackStrength = 1f;
    public bool useRangedWeapon = false;
    public float walkSpeed = 1f;
    public float wage = 1f;
    public float maxHunger = 1f;
    public float maxLife = 100f;
    public float life = 10f;
    public float satiety = 1f;

    public bool available = true;
    public CrewMember_Job job;
    protected List<CrewMember_Attribute> attributes;
    protected List<KeyValuePair<SkillAttribute, float>> skills;


    [NonSerialized]
    protected Cooldown attackSpeed;
    [NonSerialized]
    public long canonReloadSpeed;
    [NonSerialized]
    protected Cooldown steerSpeed;
    [NonSerialized]
    public long repairSpeed;

    // should be a class "Room"
    public string assignedRoom;

    public CrewMember(string id, CrewMember_Job job)
    {
        this.id = id;
        type = this.GetType().Name.Substring(this.GetType().Name.IndexOf("_") + 1);
        attackSpeed = new Cooldown();
        canonReloadSpeed = 50;
        //canonReloadSpeed = new Cooldown();
        steerSpeed = new Cooldown();
        //repairSpeed = new Cooldown();
        repairSpeed = 0;
        assignedRoom = "";
        memberName = id;
        this.job = job;
        this.attributes = new List<CrewMember_Attribute>();
        this.skills = new List<KeyValuePair<SkillAttribute, float>>();

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 3f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 310f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 3f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 20f));

        //attackSpeed.timeLeft = 1f;
        //canonReloadSpeed.timeLeft = 5f;
        //steerSpeed.timeLeft = 10f;
        //repairSpeed.timeLeft = 5f;
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

    public float getCrewSkill(SkillAttribute skill)
    {
        CrewMember_Attribute effect = this.getAttribute(skill.Range);
        float value = -1;

        if (this.skills.Exists(x => x.Key.Name == skill.Name))
        {
            value = this.skills.Find(x => x.Key.Name == skill.Name).Value;
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

    public void addAttribute(Attribute effect, float timer, float value)
    {
        this.removeAttribute(effect);
        this.attributes.Add(new CrewMember_Attribute(effect, timer, value));
    }

    public void removeAttribute(Attribute effect)
    {
        CrewMember_Attribute item = this.getAttribute(effect);

        if (item != null)
        {
            this.attributes.Remove(item);
        }
    }

    public CrewMember_Attribute getAttribute(Attribute effect)
    {
        return this.attributes.Find(x => x.effect == effect);
    }
}
