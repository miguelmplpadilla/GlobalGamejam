using System;
using Unity.Collections;
using UnityEngine;
using XNode;

[NodeWidth(304)]
public class PassageNode : Node
{
	[NonSerialized] public int idNode = -1;

	[Input] public PassageNode passageEntrance;
	
	[Output] public PassageNode decisionIzquierda;
	
	protected override void Init() 
	{
		base.Init();

		if (idNode != -1) return;

		if (graph != null)
		{
			HistoryCreator historyCreator = graph as HistoryCreator;
			if (historyCreator == null)
			{
				Debug.Log("Graph is not HistoryCreator");
			}
			historyCreator.cantNodeCreated++;
			idNode = historyCreator.cantNodeCreated;
			return;
		}
		
		Debug.Log("Graph no asignado");
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);
		
		PassageNode fromNode = from.node as PassageNode;
		PassageNode toNode = to.node as PassageNode;

		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "passageEntrance" && from.fieldName == "decisionIzquierda")
			fromNode.decisionIzquierda = toNode;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("passageEntrance")) passageEntrance = null;
		if (port.fieldName.Equals("decisionIzquierda")) decisionIzquierda = null;
	}
}