using UnityEngine;
using XNode;

[CreateNodeMenu("")]
[NodeWidth(304)]
public class PassageNode : Node
{
	[Input] public PassageNode passageEntrance;
	[Space(15)]
	[Output] public DescriptionNode description;
	[Output] public PassageNode decisionIzquierda;

	public DecisionNode.PlayAudio audio;
	
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