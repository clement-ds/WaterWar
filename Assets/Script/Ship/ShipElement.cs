using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class ShipElement : MonoBehaviour
{
    protected readonly int life;
    protected int currentLife;
    protected string id;
    protected bool selected = false;
    protected bool focused = false;
    protected bool available = true;
    protected bool repairing = false;
    protected bool attacking = false;
    protected bool canAttack = true;
    public Slider slider = null;
    protected SimpleObjectPool buttonObjectPool;
    protected GameObject pSlider = null;
    protected GameObject mSlider = null;
    protected GameObject actionMenu = null;
    protected SpriteOutline outline = null;
    protected List<ActionMenuItem> actionList = new List<ActionMenuItem>();

    void Start()
    {
        this.id = Guid.NewGuid().ToString();
        this.buttonObjectPool = GameObject.Find("SimpleActionMenuPool").GetComponent<SimpleObjectPool>();
        this.outline = GetComponent<SpriteOutline>();

        if (this.GetComponentInParent<Battle_Player>())
        {
            createActionMenu();
        }
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

    /** INPUT **/
    public void hasInputMouse(Boolean clicked)
    {
        if (clicked)
        {
            if (this.focused)
            {
                this.unfocus();
            }
            else
            {
                this.focus();
            }
        }
        else
        {
            this.unselect();
        }
    }

    /** INTERACTION **/
    public void focus()
    {
        this.selected = true;
        this.focused = true;
        this.outline.enabled = true;
        this.updateActionMenu();
    }

    public void unfocus()
    {
        this.selected = false;
        this.focused = false;
        this.outline.enabled = false;
        this.actionMenu.SetActive(false);
    }

    public void unselect()
    {
        this.selected = false;
    }

    /** GUI CREATOR **/
    protected abstract void createActionList();

    private void createActionMenu()
    {
        this.actionMenu = buttonObjectPool.GetObject();
        this.actionMenu.transform.SetParent(GameObject.Find("Battle_UI").gameObject.transform);

        this.actionMenu.GetComponent<RectTransform>().offsetMin = new Vector2(-100, -100);
        this.actionMenu.GetComponent<RectTransform>().offsetMax = new Vector2(100, 100);
        this.actionMenu.transform.localScale = new Vector3(1, 1, 1);

        this.actionMenu.GetComponentInChildren<ActionMenuList>().init(this.actionList, this);
        this.actionMenu.SetActive(false);
    }

    private void updateActionMenuItem()
    {
        if (!this.actionMenu)
            return;
        this.createActionList();
        this.actionMenu.GetComponentInChildren<ActionMenuList>().update(this.actionList);
    }

    public void updateActionMenu()
    {
        print("update menu");
        if (!this.actionMenu)
            return;
        this.updateActionMenuItem();
        if (this.actionList.Count != 0)
            this.actionMenu.SetActive(true);
        else
            this.actionMenu.SetActive(false);
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

    /** ON HIT EFFECT **/
    protected abstract void dealDamageAsRepercution(int damage);

    protected abstract void dealDamageOnDestroy();

    protected abstract void applyMalusOnHit();

    protected abstract void applyMalusOnDestroy();

    /** ACTIONS **/
    public abstract bool actionIsRunning();

    /** REPAIR **/
    protected abstract void doRepairEnd();

    public bool doRepair()
    {
        if (getMember() != null)
        {
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

    public string getId()
    {
        return this.id;
    }

    public int getLife()
    {
        return this.life;
    }

    public int getCurrentLife()
    {
        return this.currentLife;
    }

    public bool isAvailable()
    {
        return this.available;
    }

    public bool isSelected()
    {
        return this.selected;
    }

    public bool isFocused()
    {
        return this.focused;
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
            this.dealDamageOnDestroy();
            this.applyMalusOnDestroy();
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