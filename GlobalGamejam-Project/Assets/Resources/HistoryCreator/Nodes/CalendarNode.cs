using XNode;

public class CalendarNode : PassageNode
{
	public int edadAnterior = 0;
	public int edadSiguiente = 0;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (from.node != this) return;
	}
	
	public void OnValidate()
	{
		UpdateNodeName();
	}

	private void UpdateNodeName()
	{
		string finalName = edadAnterior != 0 && edadSiguiente != 0
			? edadAnterior + " - " + edadSiguiente + " CALENDARIO"
			: "CALENDARIO";
		name = finalName;
	}
}