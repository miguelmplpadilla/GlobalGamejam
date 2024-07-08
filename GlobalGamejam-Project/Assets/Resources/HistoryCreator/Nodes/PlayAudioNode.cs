using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(250)]
public class PlayAudioNode : PassageNode
{
	public DecisionNode.PlayAudio audio;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (from.node != this) return;
	}
}