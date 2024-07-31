using UnityEngine;
using UnityEngine.Serialization;
using XNode;


public class DiapositiveNode : PassageNode
{
	public Sprite diapositivaEN;
	public Sprite diapositivaES;
	
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
	
	public void OnValidate()
	{
		UpdateNodeName();
	}

	private void UpdateNodeName()
	{
		string finalName = diapositivaEN != null ? diapositivaEN.name + " DIAPOSITIVA" : "DIAPOSITIVA";
		name = finalName;
	}
}