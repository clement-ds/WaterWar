using UnityEngine;
using System.Collections;

public class Island1 : MonoBehaviour {

	public GameObject dockingTrigger;
	public GameObject playerShip;
	public GameManager gm;
	private TravelCutscene travelCutscene;

	private Rigidbody2D playerRb2d = null;

	// Use this for initialization
	void Start () {
		playerRb2d = playerShip.GetComponent<Rigidbody2D>();
		travelCutscene = GameObject.Find("CutsceneManager").GetComponent<TravelCutscene>();

        Island island = IslandManager.GetInstance().islands[int.Parse(tag)];
        island.x = dockingTrigger.transform.position.x;
        island.y = dockingTrigger.transform.position.y;
        Debug.Log(island.name + " position is " + island.x + " - " + island.y);

    }

    // Update is called once per frame
    void Update () {
	
	}

	public void OnMouseDown()
	{
		print("dockingtrigger : " + dockingTrigger.transform.localPosition);
		//Vector2 dock;
		//dock.x = dockingTrigger.transform.localPosition.x;
		//dock.y = -dockingTrigger.transform.localPosition.y;
		//print("dock : " + dock);
		//playerRb2d.MovePosition(dock);
		playerShip.transform.position = Vector3.MoveTowards(playerShip.transform.position, dockingTrigger.transform.position, 10000);
        PlayerManager.GetInstance().player.mapPosition = new Vector2(dockingTrigger.transform.position.x,
                                                                    dockingTrigger.transform.position.y);

        travelCutscene.StartCutscene();
        StartCoroutine(MoveShip());
	}

    IEnumerator MoveShip()
    {
        yield return new WaitForSeconds(travelCutscene.duration);
        if (Random.Range(0, 2) == 1)
            gm.GoFight();
        else
        {
            print("TAG : " + tag);
            PlayerManager.GetInstance().player.currentIsland = int.Parse(tag);
            gm.GoInteraction();
        }

    }


}
