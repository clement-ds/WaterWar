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
        GameObject playerShip = GameObject.Find("BasicShipPool").GetComponent<SimpleObjectPool>().GetObject();


        playerShip.name = "Player";
        playerShip.AddComponent<SpriteRenderer>();
        //Debug.Log("ship: " + player.ship.type);
        playerShip.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + player.ship.type);
        playerShip.AddComponent<Battle_Player>();
        playerShip.GetComponent<Battle_Player>().slider = playerShip.transform.Find("Canvas").transform.Find("HBar").GetComponent<Slider>();
        playerShip.transform.position = new Vector3((isLeft ? -3 : 5), 1, 100);

        Battle_Ship p = playerShip.GetComponent<Battle_Ship>();
        p.setId(player.id);
        p.setWeaponForCrew(player.inventory.getCrewWeapon());
        this.addRoomToShip(playerShip, player.ship.shipDisposition.rooms, playerShip.GetComponent<SpriteRenderer>().sprite.rect.width, playerShip.GetComponent<SpriteRenderer>().sprite.rect.height);
        p.init();
        GameRulesManager.GetInstance().ships.Add(p);
        GameRulesManager.GetInstance().characters.Add(p.getId(), new Pair<Player, DestroyedStatus>(player, DestroyedStatus.ALIVE));
        GameRulesManager.GetInstance().playerID = p.getId();

        // create shortcut
        GameObject shortcutManager = new GameObject("ShortcutManager");
        shortcutManager.AddComponent<ShortCutManager>();
        shortcutManager.transform.parent = playerShip.transform;
        

        // create enemy
        List<Player> enemies = PlayerManager.GetInstance().getCharacterForEnemy(PlayerManager.GetInstance().player.currentIsland);

        Player ai = enemies[0];
        GameObject aiShip = GameObject.Find("BasicShipPool").GetComponent<SimpleObjectPool>().GetObject();
        aiShip.name = "Enemy";
        aiShip.AddComponent<SpriteRenderer>();
        aiShip.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + ai.ship.type);
        aiShip.AddComponent<Battle_Enemy>();
        aiShip.GetComponent<Battle_Enemy>().slider = aiShip.transform.Find("Canvas").transform.Find("HBar").GetComponent<Slider>();
        aiShip.transform.position = new Vector3((isLeft ? 5 : -3), 1, 100);

        Battle_Ship e = aiShip.GetComponent<Battle_Ship>();
        e.setId(ai.id);
        e.setWeaponForCrew(ai.inventory.getCrewWeapon());
        this.addRoomToShip(aiShip, ai.ship.shipDisposition.rooms, aiShip.GetComponent<SpriteRenderer>().sprite.rect.width, aiShip.GetComponent<SpriteRenderer>().sprite.rect.height);
        e.init();
        GameRulesManager.GetInstance().ships.Add(e);
        GameRulesManager.GetInstance().characters.Add(e.getId(), new Pair<Player, DestroyedStatus>(ai, DestroyedStatus.ALIVE));


        GameObject.Find("Distance").GetComponent<CheckDistanceBetweenObjects>().init(playerShip, aiShip);
        GameObject.Find("Main Camera").GetComponent<FollowObjectInSpace>().init(playerShip);

        //TODO escape
        Button btn = GameObject.Find("Escape").GetComponent<Button>();
        btn.onClick.AddListener(playerShip.GetComponent<Battle_Ship>().escape);
        btn.gameObject.SetActive(false);

        CanonPanelGuiManager manager = GameObject.Find("CanonPanel").GetComponent<CanonPanelGuiManager>();
        manager.initCanons(p, e);

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
