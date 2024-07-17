using System;
using Unity.VisualScripting;
using UnityEngine;
using XNode;
using Task = System.Threading.Tasks.Task;

[CreateAssetMenu]
public class HistoryCreator : NodeGraph
{
    public int cantNodeCreated = 0;

    public override Node AddNode(Type type)
    {
        Node node = base.AddNode(type);
        OnNodeCreated(node);
        return node;
    }

    private void OnNodeCreated(Node node)
    {
        if (node is PassageNode)
        {
            PassageNode passageNode = node as PassageNode;
            if (passageNode == null) return;

            cantNodeCreated++;

            passageNode.idNode = cantNodeCreated;

            Debug.Log("Node Created whit id "+passageNode.idNode);
        }
    }
}