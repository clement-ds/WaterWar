using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateGameObject : MonoBehaviour {

	// Use this for initialization
	void Start () {

        bool isLeft = Random.Range(1, 3) == 1;

        Player player = PlayerManager.GetInstance().player;
        GameObject playerShip = GameObject.Find(player.ship.type + "Pool").GetComponent<SimpleObjectPool>().GetObject();
        playerShip.name = "Player";
        playerShip.AddComponent<Battle_Player>();
        playerShip.GetComponent<Battle_Player>().slider = playerShip.transform.Find("Canvas").transform.Find("HBar").GetComponent<Slider>();
        playerShip.transform.position = new Vector3((isLeft ? -3 : 3), 0, 100);

        GameObject shortcutManager = new GameObject("ShortcutManager");
        shortcutManager.AddComponent<ShortCutManager>();
        shortcutManager.transform.parent = playerShip.transform;
        this.addRoomToShip(playerShip, player.ship.shipDisposition.rooms);

        Player ai = PlayerManager.GetInstance().ai;
        GameObject aiShip = GameObject.Find(ai.ship.type + "Pool").GetComponent<SimpleObjectPool>().GetObject();
        aiShip.name = "Enemy";
        aiShip.AddComponent<Battle_Enemy>();
        aiShip.GetComponent<Battle_Enemy>().slider = aiShip.transform.Find("Canvas").transform.Find("HBar").GetComponent<Slider>();
        aiShip.transform.position = new Vector3((isLeft ? 3 : -3), 0, 100);
        this.addRoomToShip(aiShip, ai.ship.shipDisposition.rooms);
    }

    private void addRoomToShip(GameObject ship, List<Room> rooms)
    {
        foreach (Room room in rooms)
        {
            GameObject obj = GameObject.Find(room.type + "Pool").GetComponent<SimpleObjectPool>().GetObject();

            obj.transform.parent = ship.transform;
            obj.transform.localPosition = new Vector3(room.x, room.y, room.z);
            obj.transform.localRotation = Quaternion.Euler(0, 0, room.rotation);
            ship.GetComponent<Battle_Ship>().addShipElement(obj.GetComponent<ShipElement>());
        }
    }
}
