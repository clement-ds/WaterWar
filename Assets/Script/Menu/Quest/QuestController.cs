using UnityEngine;
using System.Collections;

public class QuestController : MonoBehaviour {
  private int currentIsland;

  public GameObject QuestItemPrefab;
  public GameObject ContentPanel;

  private QuestLog questLog;


  // Use this for initialization
  void Start()
  {
    IslandManager manager = IslandManager.GetInstance();
    currentIsland = PlayerManager.GetInstance().player.currentIsland;
    questLog = manager.islands[currentIsland].questLog;
    FillQuestPanel();
  }

  void FillQuestPanel()
  {
    Debug.Log(questLog.quests[0].description);
    for (int i = 0; i < questLog.quests.Count; ++i)
    {
      GameObject newItem = Instantiate(QuestItemPrefab) as GameObject;
      QuestItemController controller = newItem.GetComponent<QuestItemController>();

      controller.name = questLog.quests[i].ToString();
      controller.title.text = questLog.quests[i].ToString();
      controller.objective.text = questLog.quests[i].description;
//      controller.reward.text = questLog.quests[i].reward.name;

      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }
  }

  // Update is called once per frame
  void Update()
  {
  }
}
