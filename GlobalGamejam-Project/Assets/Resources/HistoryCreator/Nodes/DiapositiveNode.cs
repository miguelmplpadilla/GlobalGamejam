using UnityEngine;
using XNode;


public class DiapositiveNode : PassageNode
{
	public Sprite spriteDiapositiva;
	
	protected override void Init() {
		base.Init();

		name = "DIAPOSITIVA ID " + idNode;
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
		string finalName = spriteDiapositiva != null ? spriteDiapositiva.name + " DIAPOSITIVA ID " + idNode : "DIAPOSITIVA ID " + idNode;
		name = finalName;
	}
}