using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class QuestDisplay : MonoBehaviour
{

    public Quest quest;

    public Image nodeImage;
    public Text nodeTitle;
    public Text nodeText;

    private QNode currentNode;

    public void Initialize(Quest quest)
    {
        this.quest = quest;
        SetupUIElements();
    }


    public void SetupUIElements()
    {
        LoadFile("PlayerJson/Quests.txt");
        quest2 = JsonUtility.FromJson<PlayerQuest>(json[0]);


        currentNode = quest.ProgressQuest();

        if (currentNode == null)
        {
            gameObject.SetActive(false);
            return;
        }

        nodeImage.sprite = GameObject.Find("QuestBook").GetComponent<QuestManager>().GetIcon(currentNode.nodeImage);

        //nodeTitle.text = currentNode.nodeTitle;
        //nodeText.text = currentNode.nodeText;
        nodeTitle.text = "";
        nodeText.text = quest2.description;

        if (nodeImage.sprite == null)
            nodeImage.gameObject.SetActive(false);
        PlayerManager.GetInstance().player.questLog.quests.Add(quest2);
        PlayerManager.GetInstance().Save();
    }

    PlayerQuest quest2;
    List<string> json;

    private bool LoadFile(string fileName)
    {
        json = new List<string>();
        // Handle any problems that might arise when reading the text
        try
        {
            string line;
            // Create a new StreamReader, tell it which file to read and what encoding the file
            // was saved as
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            // Immediately clean up the reader after this block of code is done.
            // You generally use the "using" statement for potentially memory-intensive objects
            // instead of relying on garbage collection.
            // (Do not confuse this with the using directive for namespace at the 
            // beginning of a class!)
            using (theReader)
            {
                // While there's lines left in the text file, do this:
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        // Do whatever you need to do with the text line, it's a string now
                        // In this example, I split it into arguments based on comma
                        // deliniators, then send that array to DoStuff()
                        json.Add(line);
                    }
                }
                while (line != null);
                // Done reading, close the reader and return true to broadcast success    
                theReader.Close();
                return true;
            }
        }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }
}