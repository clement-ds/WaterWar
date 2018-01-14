using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntroSceneManager : MonoBehaviour {
    public GameObject map, menu, coinPrefab;
    private CameraAnimationManager cameraManager;
    public GameObject wealthSpawnPoint;

    private int wealth;
    private List<GameObject> spawnedMoney;

	// Use this for initialization
	void Start () {
        GameObject o = GameObject.FindGameObjectWithTag("MainCamera");
        cameraManager = o.GetComponent<CameraAnimationManager>();
        Debug.Log("ISM: IsInGame: " + GameManager.Instance.IsInGame());
        if (GameManager.Instance.IsInGame() != 45)
        {
            cameraManager.PlayAnimation("CameraStart");
        }

        spawnedMoney = new List<GameObject>();
        InvokeRepeating("UpdateWealth", 0f, 1f);
        InvokeRepeating("SpawnMoney", .5f, .2f);
    }

    public bool GetStateForBool(string boolName) {
        return cameraManager.GetStateForBool(boolName);
    }

    private int moneyCurrentlySpawned = 0;
    private const float positionOffset = .5f;
    private void SpawnMoney() {
        if (moneyCurrentlySpawned < wealth) {
            GameObject obj = GameObject.Instantiate(coinPrefab);
            obj.transform.SetParent(wealthSpawnPoint.transform, false);
            obj.transform.eulerAngles = new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
            obj.transform.position += new Vector3(Random.Range(-positionOffset, positionOffset), Random.Range(-positionOffset, positionOffset), Random.Range(-positionOffset, positionOffset));
            spawnedMoney.Add(obj);
            moneyCurrentlySpawned += 10;
        }
        if (moneyCurrentlySpawned > wealth + 10) {
            GameObject maillonFaible = spawnedMoney[spawnedMoney.Count - 1];
            spawnedMoney.Remove(maillonFaible);
            GameObject.Destroy(maillonFaible);
            moneyCurrentlySpawned -= 10;
        }
    }
    private void UpdateWealth() {
        wealth = PlayerManager.GetInstance().player.money;
    }

    public void CameraStateChange(string state, bool hasForcedState = false, bool forcedState = false) {
        cameraManager.StateChange(state, hasForcedState, forcedState);
    }

    void Update() {
        if (GameManager.Instance.IsInGame() == 45) { // TODO: remplacer ca par mouvement de la camera render / display/not display
            if (!map.activeSelf)
            {
                map.SetActive(true);
            }
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
        }
    }

}
