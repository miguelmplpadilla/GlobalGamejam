using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using XNode;

[NodeWidth(304)]
public class StoryNode : Node
{
	public string name;
	public string id;

	public string escritor;

	[Output] public PassageNode startPassage;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);
		
		StoryNode fromNode = from.node as StoryNode;
		PassageNode toNode = to.node as PassageNode;

		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "passageEntrance" && from.fieldName == "startPassage")
			fromNode.startPassage = toNode;
	}
}