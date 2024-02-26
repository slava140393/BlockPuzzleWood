using System;
using System.Collections.Generic;
using Game.Scripts.Figures;
using UnityEngine;

namespace Game.Scripts.Services
{
	[Serializable]
	public class GameStorageItem
	{
		[SerializeField] public List<bool> Board;
		[SerializeField] public FigureSavableData Figure1;
		[SerializeField] public FigureSavableData Figure2;
		[SerializeField] public FigureSavableData Figure3;
		[SerializeField] public FigureSavableData Locked;
		[SerializeField] public int Best;
		[SerializeField] public int Current;
		[SerializeField] public int Rotate;
		[SerializeField] public int Key;
		
	}
}