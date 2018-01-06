using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

// BASECLASS, works with ListPanelRoot/ListReceiverPanel/ListRowPanel kind of items
public class UIController : MonoBehaviour
{
    [Header("Panel items")]
    public RectTransform panel;
    public GameObject rootPanel;
    public GameObject rowPrefab;


    public virtual void Populate()
    {
        ClearPanel();
    }

    public virtual void TogglePanel()
    {
        rootPanel.SetActive(!rootPanel.activeSelf);
        panel.gameObject.SetActive(rootPanel.activeSelf);
        if (panel.gameObject.activeSelf)
            Populate();
    }

    protected virtual void ClearPanel()
    {
        foreach (Transform child in panel)
        {
            if (child != panel.transform)
                Destroy(child.gameObject);
        }
    }


}
