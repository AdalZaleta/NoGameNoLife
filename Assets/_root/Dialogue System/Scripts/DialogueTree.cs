using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Tree", menuName = "Dialogue System/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    List<DialogueNode> nodes;

    DialogueNode currentNode;

    public DialogueTree()
    {
        nodes = new List<DialogueNode>();
    }

    public void Start()
    {
        if(nodes.Count > 0)
        {
            currentNode = nodes[0];
        }
        else
        {
            Debug.LogWarning("Starting dialogue tree with no nodes");
        }
    }

    public void GoToNextNode()
    {
        if (currentNode)
        {
            currentNode.OnNodeExit();
            currentNode = currentNode.nextNode;
            currentNode?.OnNodeEnter();
        }
    }

    public void UpdateDialogue()
    {
        currentNode?.OnNodeStay(Time.deltaTime);
    }

    public void AddNode(DialogueNode _node)
    {
        nodes.Add(_node);
    }
}
