using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDisplayerItem : MonoBehaviour {

	public TextMeshProUGUI Q_Title;
	public TextMeshProUGUI Q_Description;
	public TextMeshProUGUI Q_Rewards;

	private PlayerQuest quest;

	public void SetPlayerQuest(PlayerQuest quest) {
		this.quest = quest;
		Q_Title.SetText(quest.title);
		Q_Description.SetText(quest.description);
		Q_Rewards.SetText(string.Join("\n", quest.GetRewardString().ToArray()));
	}

	void OnMouseDown() {
		PlayerManager.GetInstance().AcceptQuest(quest);
		GameObject.Destroy(this.gameObject);
	}
}
