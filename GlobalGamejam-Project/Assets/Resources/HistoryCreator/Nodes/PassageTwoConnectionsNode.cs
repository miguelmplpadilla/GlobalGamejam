using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using XNode;

[NodeWidth(304)]
public class PassageTwoConnectionsNode : PassageNode 
{
	[Output] public PassageNode exitPassage2;
	
	protected override void Init() 
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);
		
		PassageTwoConnectionsNode fromNode = from.node as PassageTwoConnectionsNode;
		PassageNode toNode = to.node as PassageNode;

		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "passageEntrance" && from.fieldName == "exitPassage2")
			fromNode.exitPassage2 = toNode;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("exitPassage2")) exitPassage2 = null;
	}
}