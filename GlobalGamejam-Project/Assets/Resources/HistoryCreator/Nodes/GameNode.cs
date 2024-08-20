using UnityEngine;

public class GameNode : PassageTwoConnectionsNode
{
	public GameObject prefabGame;
	
	public void OnValidate()
	{
		UpdateNodeName();
	}

	private void UpdateNodeName()
	{
		string finalName = prefabGame != null ? prefabGame.name + " GAME" : "GAME";
		name = finalName;
	}
}