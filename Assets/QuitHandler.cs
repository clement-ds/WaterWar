using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitHandler : MonoBehaviour {

	public GameObject loadingPanel;

	public void QuitBattle() {
		loadingPanel.SetActive(true);
		GameManager.Instance.GoIntroMenu();
	}
}
