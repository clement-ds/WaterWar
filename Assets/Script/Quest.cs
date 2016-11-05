using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class Quest
{

    [XmlArray("Nodes")]
    [XmlArrayItem("Node")]
    public List<QNode> nodes;

    [XmlAttribute("QuestTitle")]
    string questTitle;

    [System.NonSerialized()]
    public string fileName;

    [System.NonSerialized()]
    private QNode currentNode;

    [System.NonSerialized]
    private int position;




    /**
     * Returns the current node progress in quest and the moves to next node if possible.
     **/
    public QNode ProgressQuest()
    {
        if (nodes != null && position < nodes.Count)
        {
            currentNode = nodes[position];
            position++;
        }
        else
            return null;
        return currentNode;

    }


    /**
     * Loads quest from XML file.
     * name: supply xml file name
     * return: Quest object created from XML file
     **/
    public static Quest LoadQuest(string name)
    {
        var serializer = new XmlSerializer(typeof(Quest));
        var stream = new FileStream("Assets/StreamingAssets/Quests/" + name + ".xml", FileMode.Open);
        var quest = serializer.Deserialize(stream) as Quest;
        stream.Close();

        return quest as Quest;
    }
}