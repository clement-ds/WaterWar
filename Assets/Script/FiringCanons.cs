using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FiringCanons : MonoBehaviour {
    public GameObject MainCanon { get; set; }
    public GameManager gm;
    public Rect windowRect;
    public bool GUIEnabled = true;

    void Start() {
        MainCanon = null;
    }

    void Update() {
    }

    // Use this for initialization
    public void noCanon() {
        MainCanon = null;
    }

    public void fireOn(GameObject target) {
        if (MainCanon != null && MainCanon.GetComponent<Cooldown>().getPossibility() == true)
        {
            ParticleSystem canonExplosion = MainCanon.GetComponent<ParticleSystem>();
            canonExplosion.Play();
            Battle_Enemy enemy = target.GetComponentInParent<Battle_Enemy>();
            print("Canon " + MainCanon.name + " fires on " + target.name + " with boulet " + MainCanon.GetComponent<SetAsCanonOnClick>().bouletname);
            if (enemy != null)
            {
                ParticleSystem explosionRoom = target.GetComponent<ParticleSystem>();
                explosionRoom.Play();
                enemy.setCurrentLife(enemy.getCurrentLife() - 20);
                print("Aouch we loose 20 pv");
                if (enemy.getCurrentLife() <= 0)
                    GUIEnabled = true;


            }
        }
        else {
            GUIEnabled = false;
            OnGUI(); //print("No Canon");
        }
    }
    void OnGUI()
    {
        if (GUIEnabled)
        windowRect = GUI.Window(0, new Rect(Screen.width/2 - 75, Screen.height/2 - 50, 150, 100), DoMyWindow, "Victory");
    }

    void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(25, 25, 100, 40), "Loot here");
        if (GUI.Button(new Rect(25, 75, 100, 20), "Continue"))
        {
            gm.GoInteraction();
        }

    }

}

