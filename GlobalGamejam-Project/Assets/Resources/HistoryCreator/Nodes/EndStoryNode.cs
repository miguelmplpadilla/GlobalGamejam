﻿using XNode;

[NodeWidth(304)]
public class EndStoryNode : PassageNode
{
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}