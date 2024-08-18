using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DescriptionNode : Node
{
	[Input] public DescriptionNode input;
	[Space(15)]
	[TextArea(3, 20)] public string description;
}