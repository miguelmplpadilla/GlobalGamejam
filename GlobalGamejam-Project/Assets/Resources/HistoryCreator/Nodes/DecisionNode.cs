using System;
using UnityEngine;

public class DecisionNode : PassageTwoConnectionsNode
{
	public Card card;
	
	protected override void Init() {
		base.Init();
		
		name = "DECISION ID "+idNode;
	}
	
	[Serializable]
	public class Card
	{
		public Decisions decisionesEs;
		public Decisions decisionesEn;
		public Texts textos;
	}

	[Serializable]
	public class Texts
	{
		[TextArea(3, 20)] public string textoES;
		[TextArea(3, 20)] public string textoEN;
	}

	[Serializable]
	public class Decisions
	{
		[TextArea(3, 20)] public string decisionIzquierda = "";
		[TextArea(3, 20)] public string decisionDerecha = "";
	}

	[Serializable]
	public class PlayAudio
	{
		public enum TypeSound
		{
			SFX, LOCUTION, MUSIC
		}
		
		public TypeSound typeSound = 0;
		public string soundName = "";
		public bool loop = false;
	}
}