using UnityEngine;
using XNode;


public class AnimationNode : PassageNode
{
	public string id;
	
	protected override void Init() {
		base.Init();
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (from.node != this) return;
		
		if (from.fieldName.Equals("exitPassage2"))
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
		string finalName = !string.IsNullOrEmpty(id) ? id + " DIAPOSITIVA" : "DIAPOSITIVA";
		name = finalName;
	}
}