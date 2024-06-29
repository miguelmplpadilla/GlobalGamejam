using System;
using UnityEngine;
using XNode;

public class DecisionNode : PassageTwoConnectionsNode
{
	public Card card;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	
	[Serializable]
	public class Card
	{
		public Decisions decisiones;
		public Texts textos;
		public PlayAudio audio;
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