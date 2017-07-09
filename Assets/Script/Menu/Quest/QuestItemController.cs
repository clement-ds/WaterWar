using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestItemController : MonoBehaviour
{
    public Text title, objective, reward, moneyReward;

    private Button btn;

    // Use this for initialization
    void Start()
    {
        btn = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TaskOnClick()
    {
        Debug.Log("Click ! ");
    }
}
