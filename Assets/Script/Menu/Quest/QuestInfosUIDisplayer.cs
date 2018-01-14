using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class QuestInfosUIDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	IntroSceneManager sceneManager;
	private PlayerQuest playerQuest;

	private GameObject UICanvas;
	private TextMeshProUGUI title;
	private TextMeshProUGUI description;
	private TextMeshProUGUI rewards;

	void Start() {
		sceneManager = GameObject.Find("SceneManager").GetComponent<IntroSceneManager>();
	}

	public void SetQuest(PlayerQuest playerQuest) {
		this.playerQuest = playerQuest;
	}

	public void SetObjectsReferences(GameObject canvas, TextMeshProUGUI title, TextMeshProUGUI description, TextMeshProUGUI rewards) {
		this.UICanvas = canvas;
		this.title = title;
		this.description = description;
		this.rewards = rewards;
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
		if (sceneManager.GetStateForBool("Quest")) {
			UICanvas.SetActive(true);
			title.SetText(playerQuest.title);
			description.SetText(playerQuest.description);
			List<string> rewardsList = playerQuest.GetRewardString();
			if (rewardsList != null) {
				rewards.SetText(string.Join("\n", rewardsList.ToArray()));
			} else {
				rewards.SetText("No reward");
			}

		}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (sceneManager.GetStateForBool("Quest")) {
			UICanvas.SetActive(false);			
		}
    }
}
