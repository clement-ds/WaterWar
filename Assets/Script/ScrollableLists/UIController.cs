using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

// BASECLASS, works with ListPanelRoot/ListReceiverPanel/ListRowPanel kind of items
public abstract class UIController : MonoBehaviour
{

    public RectTransform panel;
    public GameObject rootPanel;
    public GameObject rowPrefab;


    public virtual void Populate()
    {
        ClearPanel();
    }

    public void TogglePanel()
    {
        rootPanel.SetActive(!rootPanel.active);
        panel.gameObject.SetActive(rootPanel.active);
        if (panel.gameObject.active)
            Populate();
    }

    private void ClearPanel()
    {
        foreach (Transform child in panel)
        {
            if (child != panel.transform)
                Destroy(child.gameObject);
        }
    }


}
