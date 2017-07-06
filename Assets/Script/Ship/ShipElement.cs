using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class AvailablePosition
{
    public Vector3 position;
    public bool available;
    public int crewId;

    public AvailablePosition(Vector3 position)
    {
        this.position = position;
        this.available = true;
    }
}

public abstract class ShipElement : GuiElement
{
    protected readonly int life;
    protected int currentLife;
    protected bool available = true;
    protected bool repairing = false;
    protected bool attacking = false;
    protected bool canAttack = true;
    public Slider slider = null;
    protected GameObject pSlider = null;
    protected GameObject mSlider = null;
    protected List<AvailablePosition> availablePosition = new List<AvailablePosition>();

    protected override void StartMySelf()
    {
        createAvailableCrewMemberPosition();
        //TODO garance ça plante
        /*
        GameObject pSlider = GameObject.Find("Battle_UI/ex_slidder").gameObject;
        GameObject itemObj = Instantiate(pSlider);
        itemObj.transform.SetParent(GameObject.Find("Battle_UI").transform);
        slider = itemObj.GetComponent<Slider>();
        itemObj.transform.localScale = new Vector3(1, 1, 1);
        mSlider = itemObj;*/

    }

    protected ShipElement(int lifeValue)
    {
        this.life = lifeValue;
        this.setCurrentLife(life);
    }

    void Update()
    {
        if (Camera.main)
        {
            float camHalfHeight = Camera.main.orthographicSize;
            float camHalfWidth = Camera.main.aspect * camHalfHeight;

            Bounds bounds = this.GetComponent<SpriteRenderer>().bounds;
            var wantedPos = Camera.main.WorldToViewportPoint(this.transform.position);

            // Set a new vector to the top left of the scene 
            Vector3 topLeftPosition = new Vector3(-camHalfWidth, camHalfHeight, 0) + Camera.main.transform.position;

            // Offset it by the size of the object 
            topLeftPosition += new Vector3(bounds.size.x / 2, -bounds.size.y / 2, 0);

            topLeftPosition.x += (wantedPos.y);
            topLeftPosition.y -= (wantedPos.x);

            if (mSlider)
            {
                mSlider.transform.position = topLeftPosition;
            }
        }
    }

    /** SLIDER HP **/
    public void updateSliderValue()
    {
        if (slider)
        {
            slider.value = (currentLife * 100) / life;
        }
    }

    void OnMouseEnter()
    {
        if (slider)
        {
            slider.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (slider)
        {
            slider.enabled = false;
        }
    }

    /** AVAILABLE POSITION **/
    protected abstract void createAvailableCrewMemberPosition();

    public Vector3 chooseAvailableCrewMemberPosition(int id)
    {
        for (int i = 0; i < this.availablePosition.Count; ++i)
        {
            if (this.availablePosition[i].available)
            {
                this.availablePosition[i].available = false;
                this.availablePosition[i].crewId = id;
                return this.availablePosition[i].position;
            }
        }
        return new Vector3(0f, 0f, 0f);
    }

    public void freeCrewMemberPosition(int id)
    {
        for (int i = 0; i < this.availablePosition.Count; ++i)
        {
            if (this.availablePosition[i].crewId == id)
            {
                this.availablePosition[i].available = true;
                break;
            }
        }
    }

    /** ON HIT EFFECT **/
    protected abstract void dealDamageAsRepercution(int damage);

    protected abstract void dealDamageOnDestroy();

    protected abstract void applyMalusOnHit();

    protected abstract void applyMalusOnDestroy();

    /** ACTIONS **/
    public abstract bool actionIsRunning();

    public abstract bool actionStopRunning();

    /** REPAIR **/
    protected abstract void doRepairActionEnd();

    private void doRepairEnd()
    {
        this.repairing = false;
        this.doRepairActionEnd();
        this.updateActionMenu();
    }
    
    public bool doRepair()
    {
        if (getMember() != null && !this.isRepairing())
        {
            this.repairing = true;
            this.doRepairAction();
            this.updateActionMenu();
            return true;
        }
        return false;
    }

    protected abstract bool doRepairAction();

    /** DO DAMAGE **/
    public bool doDamage()
    {
        if (this.available && getMember() != null)
        {
            if (this.doDamageAction())
            {
                this.doDamageAnimation();
            }
            this.updateActionMenu();
            return true;
        }
        else
        {
            this.setAttacking(false);
        }
        return false;
    }

    protected abstract bool doDamageAction();

    protected abstract void doDamageAnimation();

    /** RECEIVE DAMAGE **/
    protected void die()
    {
        this.dealDamageOnDestroy();
        this.applyMalusOnDestroy();
        Battle_CrewMember member = this.GetComponent<Battle_CrewMember>();
        if (member)
        {
            member.freeCrewMemberFromShipElement(this, this.transform.parent.gameObject);
        }
    }
    public bool receiveDamage(int damage)
    {
        if (this.available)
        {
            if (this.receiveDamageAction(damage))
            {
                this.receiveDamageAnimation();
            }
            this.dealDamageAsRepercution(damage);
            this.applyMalusOnHit();
            this.updateActionMenu();
            return true;
        }
        return false;
    }

    protected abstract bool receiveDamageAction(int damage);

    protected abstract void receiveDamageAnimation();


    /** GETTERS **/
    public Battle_CrewMember getMember()
    {
        return transform.GetComponentInChildren<Battle_CrewMember>();
    }

    public int getLife()
    {
        return this.life;
    }

    public int getCurrentLife()
    {
        return this.currentLife;
    }

    public float getPercentLife()
    {
        return this.currentLife * 100 / this.life;
    }

    public bool isAvailable()
    {
        return this.available;
    }

    public bool isRepairing()
    {
        return this.repairing;
    }

    /** SETTERS **/
    public void setCurrentLife(int value)
    {
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
        this.available = (this.currentLife > 0);
        if (!available)
        {
            this.die();
        }
    }

    public void setAvailable(bool value)
    {
        this.available = value;
    }

    protected void setAttacking(bool value)
    {
        this.attacking = value;
        if (!this.attacking)
            canAttack = true;
        this.updateActionMenu();
    }
}