using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class QNode
{

    [XmlElementAttribute("NodeTitle")]
    public string nodeTitle;

    [XmlElementAttribute("NodeText")]
    public string nodeText;

    [XmlElementAttribute("NodeImage")]
    public string nodeImage;

    public override string ToString()
    {
        return nodeTitle + " \n" + nodeText + " \n" + nodeImage;
    }
}