using UnityEngine;
using XNode;


public class AnimationNode : PassageNode
{
	public GameObject prefabAnimacion;
	
	protected override void Init() {
		base.Init();
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (from.node != this) return;
		
		if (from.fieldName.Equals("decisionDerecha"))
		{
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);
		}
	}

	private void OnValidate()
	{
		UpdateNodeName();
	}

	private void UpdateNodeName()
	{
		string finalName = prefabAnimacion != null
			? prefabAnimacion.name + " ANIMACION"
			: "ANIMACION";
		name = finalName;
	}
}