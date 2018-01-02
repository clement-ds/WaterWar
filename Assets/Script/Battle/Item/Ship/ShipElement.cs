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

public enum Ship_Item { CANON, CANTEEN, WHEEL, INFIRMARY, WAREHOUSE, PLAYGROUND }

public abstract class ShipElement : MonoBehaviour
{
    protected readonly float life;
    protected float currentLife;
    protected RoomElement parentRoom;

    protected Ship_Item type;
    protected bool available = true;
    protected bool attacking = false;
    protected bool canAttack = true;

    public Slider slider = null;
    public float sliderTimer;
    protected AvailablePosition availablePosition = null;

    protected ShipElement(float lifeValue, Ship_Item type)
    {
        this.type = type;
        this.life = lifeValue;
        this.setCurrentLife(life);
        this.sliderTimer = 0;
    }

    /** INIT **/
    public abstract void init();

    public void changeParentRoom(RoomElement room)
    {
        this.parentRoom = room;
    }

    public abstract List<ActionMenuItem> createActionList();

    /** UPDATE **/
    void Update()
    {
        updateMyself();
    }

    protected void updateParentActionMenu()
    {
        this.getParentShip().updateActionMenu();
    }

    protected virtual void updateMyself()
    {
        // do nothing here
    }

    /** HOVER **/
    public bool mouseIsHover()
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
        }
        else if (this.sliderTimer == 0)
        {
            this.hideSlider();
        }
        return false;
    }

    public void updateHover()
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

    public void showSlider()
    {
        if (this.slider)
            slider.gameObject.SetActive(true);
    }

    public void hideSlider()
    {
        if (this.slider)
            slider.gameObject.SetActive(false);
    }

    /** AVAILABLE POSITION **/
    public abstract void createAvailableCrewMemberPosition();

    public bool hasAvailableCrewMemberPosition()
    {
        if (availablePosition != null)
        {
            return this.availablePosition.available;
        }
        return false;
    }

    public Vector3 chooseAvailableCrewMemberPosition(int id)
    {
        if (availablePosition != null)
        {
            if (this.availablePosition.crewId == id)
            {
                return this.availablePosition.position;
            }
            if (this.availablePosition.available)
            {
                this.availablePosition.available = false;
                this.availablePosition.crewId = id;
                return this.availablePosition.position;
            }
        }
        throw new System.Exception("no position available");
    }

    public void freeCrewMemberPosition(int id)
    {
        if (availablePosition != null)
        {
            if (this.availablePosition.crewId == id)
            {
                this.availablePosition.available = true;
                this.updateParentActionMenu();
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
    public void repair(float value)
    {
        this.currentLife += value;
        if (this.currentLife > this.life)
            this.currentLife = this.life;
    }

    /** DO DAMAGE **/
    public bool doDamage()
    {
        if (this.available && getMember() != null)
        {
            if (this.doDamageAction())
            {
                this.doDamageAnimation();
            }
            this.updateParentActionMenu();
            return true;
        }
        else
        {
            this.setAttacking(false);
        }
        this.updateParentActionMenu();
        return false;
    }

    protected abstract bool doDamageAction();

    protected abstract void doDamageAnimation();

    /** RECEIVE DAMAGE **/
    protected void die()
    {
        this.getParentShip().updateActionMenu();
        this.dealDamageOnDestroy();
        this.applyMalusOnDestroy();
        Battle_CrewMember member = this.GetComponent<Battle_CrewMember>();
        if (member)
        {
            member.freeCrewMemberFromShipElement();
        }
    }
    public bool receiveDamage(Battle_CanonBall canonBall)
    {
        this.getParentShip().updateActionMenu();
        if (this.available)
        {
            if (this.receiveDamageAction(canonBall))
            {
                this.receiveDamageAnimation();
            }
            this.dealDamageAsRepercution(canonBall);
            this.applyMalusOnHit(canonBall);
            return true;
        }
        return false;
    }

    protected abstract bool receiveDamageAction(Battle_CanonBall canonBall);

    protected abstract void receiveDamageAnimation();


    /** GETTERS **/
    public Battle_CrewMember getMember()
    {
        return this.transform.GetComponentInChildren<Battle_CrewMember>();
    }

    public Battle_Ship getParentShip()
    {
        return this.parentRoom.transform.GetComponentInParent<Battle_Ship>();
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

    public Ship_Item getType()
    {
        return this.type;
    }

    public Vector3 transformToParentPos(Vector3 pos)
    {
        return new Vector3(this.transform.localPosition.x + pos.x, this.transform.localPosition.y + pos.y, this.transform.localPosition.z + pos.z);
    }

    public RoomElement getParentRoom()
    {
        return this.parentRoom;
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
    }
}