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

    public void fireOn(GameObject target) {
        if (MainCanon != null && MainCanon.GetComponent<Cooldown>().getPossibility() == true)
        {
            // Reset cooldown after attack
            MainCanon.GetComponent<Cooldown>().resetPossibility();

            // Create particle of canon and play it
            ParticleSystem canonShotExplosion = (ParticleSystem)MainCanon.transform.Find("CanonShotExplosion/PS_CanonShotExplosion").gameObject.GetComponent<ParticleSystem>();
            canonShotExplosion.Play();


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
                ParticleSystem targetExplosion = target.transform.Find("BoatExplosion/PS_BoatExplosion").gameObject.GetComponent<ParticleSystem>();
                targetExplosion.Play();

                if (target.GetComponent<HasLife>())
                    target.GetComponent<HasLife>().setLife(target.GetComponent<HasLife>().getLife() - 20);
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

