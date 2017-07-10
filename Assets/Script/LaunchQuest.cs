using UnityEngine;
using System.Collections;

public class LaunchQuest : MonoBehaviour
{
    public QuestManager questManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TriggerButton()
    {
        questManager.StartQuest("Quest1");
    }
}
