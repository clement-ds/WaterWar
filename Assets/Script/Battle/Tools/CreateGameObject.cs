using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateGameObject : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        bool isLeft = Random.Range(1, 3) == 1;

        // reset
        RoomUtils.reset();

        // create player
        Player player = PlayerManager.GetInstance().player;
        GameObject playerShip = GameObject.Find(player.ship.type + "Pool").GetComponent<SimpleObjectPool>().GetObject();
        playerShip.name = "Player";
        playerShip.AddComponent<Battle_Player>();
        playerShip.GetComponent<Battle_Player>().slider = playerShip.transform.Find("Canvas").transform.Find("HBar").GetComponent<Slider>();
        playerShip.transform.position = new Vector3((isLeft ? -3 : 3), 0, 100);
        // create shortcut
        GameObject shortcutManager = new GameObject("ShortcutManager");
        shortcutManager.AddComponent<ShortCutManager>();
        shortcutManager.transform.parent = playerShip.transform;
        this.addRoomToShip(playerShip, player.ship.shipDisposition.rooms, playerShip.GetComponent<SpriteRenderer>().sprite.rect.width, playerShip.GetComponent<SpriteRenderer>().sprite.rect.height);

        // create enemy
        Player ai = PlayerManager.GetInstance().ai;
        GameObject aiShip = GameObject.Find(ai.ship.type + "Pool").GetComponent<SimpleObjectPool>().GetObject();
        aiShip.name = "Enemy";
        aiShip.AddComponent<Battle_Enemy>();
        aiShip.GetComponent<Battle_Enemy>().slider = aiShip.transform.Find("Canvas").transform.Find("HBar").GetComponent<Slider>();
        aiShip.transform.position = new Vector3((isLeft ? 3 : -3), 0, 100);
        this.addRoomToShip(aiShip, ai.ship.shipDisposition.rooms, aiShip.GetComponent<SpriteRenderer>().sprite.rect.width, aiShip.GetComponent<SpriteRenderer>().sprite.rect.height);

        GameObject.Find("Distance").GetComponent<CheckDistanceBetweenObjects>().init(playerShip, aiShip);
        GameObject.Find("Main Camera").GetComponent<FollowObjectInSpace>().init(playerShip);

        //TODO escape
        //GameObject.Find("Escape").GetComponent<Button>().onClick.AddListener(playerShip.GetComponent<Battle_Ship>().escape);

        // add stuff to ships
        Battle_Ship p = playerShip.GetComponent<Battle_Ship>();
        Battle_Ship e = aiShip.GetComponent<Battle_Ship>();

        p.setWeaponForCrew(player.inventory.getCrewWeapon());
        e.setWeaponForCrew(ai.inventory.getCrewWeapon());

        GameRulesManager.GetInstance().ships.Add(p);
        GameRulesManager.GetInstance().ships.Add(e);

    }

    private void addRoomToShip(GameObject ship, List<Room> rooms, float shipWidth, float shipHeight)
    {
        foreach (Room item in rooms)
        {
            float posX = item.x - (shipWidth / 2) + 13;
            float posY = item.y - (shipHeight / 2) + 13;
            GameObject room = GameObject.Find("RoomPool").GetComponent<SimpleObjectPool>().GetObject();
            room.transform.parent = ship.transform;
            room.transform.localPosition = new Vector3(posX / 100, posY / 100, -1);
            room.transform.localRotation = Quaternion.Euler(0, 0, 0);
            room.GetComponent<SpriteRenderer>().enabled = false;
            room.transform.localScale = new Vector3(item.width / 100, item.height / 100, 1);
            room.AddComponent<BoxCollider>();

            RoomElement roomElem = room.AddComponent<RoomElement>() as RoomElement;
            roomElem.init(ship.GetComponent<Battle_Ship>().getId(), item.id, item.links);

            if (item.component != "")
            {
                GameObject obj = GameObject.Find(item.component + "Pool").GetComponent<SimpleObjectPool>().GetObject();
                obj.transform.parent = room.transform;
                obj.transform.localPosition = new Vector3(0, 0, -1);
                obj.transform.localRotation = Quaternion.Euler(0, 0, item.rotation);
                obj.GetComponent<ShipElement>().changeParentRoom(roomElem);
                obj.AddComponent<TargetCollider>();
            }
            roomElem.StartMyself();
            ship.GetComponent<Battle_Ship>().addShipElement(roomElem);

            RoomUtils.Rooms.Add(roomElem);
        }
    }
}
