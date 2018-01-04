using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDisplayer : MonoBehaviour {

	private IslandManager IMInstance;
	private PlayerManager PMInstance;
	private int currentIslandID;
	public List<GameObject> spawnPoints;
	public GameObject questPaperPrefab;
	
	
	void Start() {
		IMInstance = IslandManager.GetInstance();
		PMInstance = PlayerManager.GetInstance();
		SwapCurrentQuests(PMInstance.player.currentIsland);
	}

	void Update() {
		if (currentIslandID != PMInstance.player.currentIsland) {
			SwapCurrentQuests(PMInstance.player.currentIsland);
		}
	}

	private void SwapCurrentQuests(int newIslandId) {
		this.currentIslandID = newIslandId;
		Island island = IMInstance.islands[this.currentIslandID];
		List<PlayerQuest> quests = island.questLog.quests;
		List<int> alreadyUsedIndexes = new List<int>();

		if (quests.Count > spawnPoints.Count) {
			quests.RemoveRange(quests.Count -1, quests.Count - spawnPoints.Count - 1);
		}

		foreach (PlayerQuest quest in quests) {
			int spawnIndex = 0;
			do {
				spawnIndex = Random.Range(0, this.spawnPoints.Count);
			} while (alreadyUsedIndexes.IndexOf(spawnIndex) != -1);
			alreadyUsedIndexes.Add(spawnIndex);

			Vector3 objectScale = questPaperPrefab.transform.localScale;
			GameObject QuestPaper = Instantiate(questPaperPrefab, spawnPoints[spawnIndex].transform, false);
			QuestDisplayerItem item = QuestPaper.GetComponent<QuestDisplayerItem>();
			QuestPaper.transform.localScale = new Vector3(.05f, .05f, .05f);
			QuestPaper.transform.localPosition = new Vector3(0, 0, 0);
			QuestPaper.transform.localEulerAngles = new Vector3(Random.Range(-10, 10), 0, 90);
			item.SetPlayerQuest(quest);
		}
	}
}
