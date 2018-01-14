using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshQuestBook : MonoBehaviour {

	[SerializeField]
	private QuestController questController;


	void OnMouseDown() {
		questController.Populate();
	}
}
