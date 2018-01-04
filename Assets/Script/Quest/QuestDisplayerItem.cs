using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDisplayerItem : MonoBehaviour {

	public TextMeshProUGUI Q_Title;
	public TextMeshProUGUI Q_Description;
	public TextMeshProUGUI Q_Objective;

	private PlayerQuest quest;

	public void SetPlayerQuest(PlayerQuest quest) {
		Debug.Log("QuestDisplayerItem Quest: " + quest.Describe());
		this.quest = quest;
		Q_Title.SetText(quest.title);
		Q_Description.SetText(quest.description);
		Q_Objective.SetText(quest.objective);
	}

	void OnMouseDown() {
		PlayerManager.GetInstance().AcceptQuest(quest);
		GameObject.Destroy(this.gameObject);
	}
}
