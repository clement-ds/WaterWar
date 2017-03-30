using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionMenuButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text nameLabel;


    private ActionMenuItem item;
    private ActionMenuList scrollList;

    // Use this for initialization
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void Setup(ActionMenuItem currentItem, ActionMenuList currentScrollList)
    {
        item = currentItem;
        nameLabel.text = item.actionName;
        scrollList = currentScrollList;
        buttonComponent.transform.localPosition = new Vector3(buttonComponent.transform.position.x, buttonComponent.transform.position.y, 0);
        buttonComponent.transform.localScale = new Vector3(1, 1, 1);
    }

    public void HandleClick()
    {
        item.actionDelegate();
    }
}