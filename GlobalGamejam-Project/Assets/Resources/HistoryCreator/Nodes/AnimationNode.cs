using UnityEngine;
using XNode;


public class AnimationNode : PassageNode
{
	public GameObject prefabAnimacion;
	
	protected override void Init() {
		base.Init();

		name = "ANIMACION ID " + idNode;
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
	
	public void OnValidate()
	{
		UpdateNodeName();
	}

	private void UpdateNodeName()
	{
		string finalName = prefabAnimacion != null
			? prefabAnimacion.name + " ANIMACION ID " + idNode
			: "ANIMACION ID " + idNode;
		name = finalName;
	}
}