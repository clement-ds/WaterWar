using UnityEngine;

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
        Debug.Log("Coucou ON ENABLE");
        UpdateOutline(true);
    }

    void OnDisable() {
        Debug.Log("Coucou ON DISABLE");

        UpdateOutline(false);
    }

    void Update() {

        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        m_Material.SetColor("_Color", outline ? Color.red : Color.white);
        /*
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }*/
    }
}