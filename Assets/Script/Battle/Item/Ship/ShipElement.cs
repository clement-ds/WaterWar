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

public enum Ship_Direction { FRONT, RIGHT, LEFT, NONE };

public enum Ship_Item { CANON, CANTEEN, HELM, INFIRMARY, WAREHOUSE, PLAYGROUND }

public abstract class ShipElement : GuiElement
{
    protected readonly float life;
    protected float currentLife;

    protected Ship_Item type;
    protected bool available = true;
    protected bool repairing = false;
    protected bool attacking = false;
    protected bool canAttack = true;

    public Slider slider = null;
    public float sliderTimer;
    protected List<AvailablePosition> availablePosition = new List<AvailablePosition>();

    public override void StartMyself()
    {
        createAvailableCrewMemberPosition();
    }

    protected ShipElement(float lifeValue, Ship_Item type)
    {
        this.type = type;
        this.life = lifeValue;
        this.setCurrentLife(life);
        this.sliderTimer = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Battle_CanonBall canonBall = col.gameObject.GetComponent<Battle_CanonBall>();
        if (canonBall)
        {
            float value = UnityEngine.Random.value;
            print(this + " (" + this.GetInstanceID() + ")  :   " + canonBall.getTarget().GetInstanceID() + "  --> " + canonBall.getHitStatus());
            if ((canonBall.getHitStatus() == HitStatus.HIT && canonBall.getTarget().GetInstanceID() == this.GetInstanceID())
                || (canonBall.getHitStatus() == HitStatus.FAIL && canonBall.getTarget().GetInstanceID() != this.GetInstanceID()))
            {
                this.receiveDamage(canonBall);
                Destroy(col.gameObject);
            }
        }
    }

    void Update()
    {
        mouseIsHover();
        updateHover();
        updateMyself();
    }

    protected virtual void updateMyself()
    {
        // do nothing here
    }

    /** HOVER **/
    private bool mouseIsHover()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);

        if (this.transform.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
        {
            this.showSlider();
            if (MouseManager.getInstance().getCursorTexture() == ECursor.SEARCH_TARGET)
            {
                MouseManager.getInstance().setCursor(ECursor.FOCUS_TARGET);
            }
            return true;
        } else if (this.sliderTimer == 0)
        {
            this.hideSlider();
        }
        return false;
    }

    private void updateHover()
    {
        this.sliderTimer -= Time.deltaTime;
        this.sliderTimer = (this.sliderTimer < 0 ? 0 : this.sliderTimer);
    }

    /** SLIDER HP **/
    public void updateSliderValue()
    {
        if (this.slider)
        {
            this.showSlider();
            this.slider.value = (this.currentLife * 100) / this.life;
            this.sliderTimer = 2;
        }
    }

    void OnMouseEnter()
    {
        this.showSlider();
    }

    void OnMouseExit()
    {
        this.hideSlider();
    }

    protected void showSlider()
    {
        if(this.slider)
            slider.gameObject.SetActive(true);
    }

    protected void hideSlider()
    {
        if (this.slider)
            slider.gameObject.SetActive(false);
    }

    /** AVAILABLE POSITION **/
    protected abstract void createAvailableCrewMemberPosition();

    public bool hasAvailableCrewMemberPosition()
    {
        for (int i = 0; i < this.availablePosition.Count; ++i)
        {
            if (this.availablePosition[i].available)
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 chooseAvailableCrewMemberPosition(int id)
    {
        for (int i = 0; i < this.availablePosition.Count; ++i)
        {
            Debug.Log("test : " + this.availablePosition[i]);
            if (this.availablePosition[i].crewId == id)
            {
                return this.availablePosition[i].position;
            }
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
    protected abstract void dealDamageAsRepercution(Battle_CanonBall canonBall);

    protected abstract void dealDamageOnDestroy();

    protected abstract void applyMalusOnHit(Battle_CanonBall canonBall);

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
    public bool receiveDamage(Battle_CanonBall canonBall)
    {
        if (this.available)
        {
            if (this.receiveDamageAction(canonBall))
            {
                this.receiveDamageAnimation();
            }
            this.dealDamageAsRepercution(canonBall);
            this.applyMalusOnHit(canonBall);
            this.updateActionMenu();
            return true;
        }
        return false;
    }

    protected abstract bool receiveDamageAction(Battle_CanonBall canonBall);

    protected abstract void receiveDamageAnimation();


    /** GETTERS **/
    public Battle_CrewMember getMember()
    {
        return transform.GetComponentInChildren<Battle_CrewMember>();
    }

    public float getLife()
    {
        return this.life;
    }

    public float getCurrentLife()
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

    public Ship_Item getType()
    {
        return this.type;
    }

    /** SETTERS **/
    public void setCurrentLife(float value)
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