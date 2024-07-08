public class GameNode : PassageTwoConnectionsNode
{
	public string id;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	
	public void OnValidate()
	{
		UpdateNodeName();
	}

	private void UpdateNodeName()
	{
		string finalName = !string.IsNullOrEmpty(id) ? id + " GAME ID " + idNode : "GAME ID " + idNode;
		name = finalName;
	}
}