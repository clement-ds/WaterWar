using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FiringCanons : MonoBehaviour {
    ArrayList bullets;
    public Sprite bullet;
    public GameObject MainCanon { get; set; }
    public GameManager gm;
    public Rect windowRect = new Rect(Screen.width/2, Screen.height/2, 200, 60);
    public bool GUIEnabled = false;

    void Start() {
        MainCanon = null;
        bullets = new ArrayList();
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
            Battle_Enemy enemy = target.GetComponentInParent<Battle_Enemy>();

            bullet = new Sprite();
            bullets.Add(bullet);

            print("Canon " + MainCanon.name + " fires on " + target.name + " with boulet " + MainCanon.GetComponent<SetAsCanonOnClick>().bouletname);
            if (enemy != null)
            {
                enemy.setCurrentLife(enemy.getCurrentLife() - 20);
                print("Aouch we loose 20 pv");
                if (enemy.getCurrentLife() <= 0)
                    GUIEnabled = true;


            }
        }
        else {
            OnGUI(); //print("No Canon");
        }
    }
    void OnGUI()
    {
        if (GUIEnabled)
        windowRect = GUI.Window(0, windowRect, DoMyWindow, "Victory");
    }

    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 100, 20), "Continue"))
        {
            print("Have a good day");
            gm.GoInteraction();
        }

    }

}

