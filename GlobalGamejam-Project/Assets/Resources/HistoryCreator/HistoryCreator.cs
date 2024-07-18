using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class HistoryCreator : NodeGraph
{
    public List<Node> nodesCreated = new List<Node>();

    public override Node AddNode(Type type)
    {
        Node node = base.AddNode(type);
        nodesCreated.Add(node);

        Debug.Log("Node Added");
        
        return node;
    }

    public override void RemoveNode(Node node)
    {
        base.RemoveNode(node);

        Debug.Log("Node Removed");

        nodesCreated.Remove(node);
    }
}