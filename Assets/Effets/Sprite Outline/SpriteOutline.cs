﻿using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;

    private Material m_Material;

    public void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    void OnEnable() {
        UpdateOutline(true);
    }

    void OnDisable() {
        UpdateOutline(false);
    }

    void Update() {

        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        if (m_Material != null)
        {
            m_Material.SetColor("_Color", outline ? Color.red : Color.white);
        }
        /*
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);*/
    }
}