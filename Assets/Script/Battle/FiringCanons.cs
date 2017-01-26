using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FiringCanons : MonoBehaviour {
    public GameManager gm;

    private GameObject MainCanon;
    private Rect windowRect;
    private bool GUIEnabled = false;

    void Start() {
        MainCanon = null;
        windowRect.position = new Vector2(20, 20);
        windowRect.size = new Vector2(120, 50);
    }

    void Update() {
    }

    // Use this for initialization
    public void noCanon() {
        MainCanon = null;
    }

    public void fireOn(ShipElement target) {
        if (MainCanon != null && MainCanon.GetComponent<Cooldown>().getPossibility() == true)
        {
            // Reset cooldown after attack
            MainCanon.GetComponent<Cooldown>().resetPossibility();

            // Create particle of canon and play it
            MainCanon.GetComponent<Canon>().doDamage();



            /*
             * It doesn't work but this is a good idea .. I think....
            CanonBall ball = new CanonBall();
            ball.Start();
            ball.setTransformation(MainCanon.transform);
            ball.setTarget(target.transform);
            */
            Battle_Enemy enemy = target.GetComponentInParent<Battle_Enemy>();
            print("Canon " + MainCanon.name + " fires on " + target.name + " with boulet " + MainCanon.GetComponent<SetAsCanonOnClick>().bouletname);
            if (enemy != null)
            {
                int resultDamage = target.receiveDamage(20);
                if (resultDamage != -1)
                {
                    print("Aouch we loose 20 pv");
                    enemy.receiveDamage(resultDamage);
                }
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
        if (GUI.Button(new Rect(25, 75, 100, 20), "Continue")) {
            gm.GoInteraction();
        }

    }

    public GameObject getMainCanon() {
        return MainCanon;
    }

    public void setMainCanon(GameObject canon) {
        MainCanon = canon;
    }
}

