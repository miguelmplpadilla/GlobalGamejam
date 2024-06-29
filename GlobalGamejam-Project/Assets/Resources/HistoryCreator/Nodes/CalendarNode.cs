using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(160)]
public class CalendarNode : PassageNode
{
	public int edadAnterior;
	public int edadSiguiente;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (from.node != this) return;
	}
}