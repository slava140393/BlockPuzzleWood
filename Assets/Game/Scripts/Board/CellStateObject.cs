using System;
using UnityEngine;

namespace Game.Scripts.Board
{
	[Serializable]
	public struct CellStateObject
	{
		[field: SerializeField] public GameObject StateObject { get; private set; }
		[field: SerializeField] public CellState State { get; private set; }

		public void Setup(CellState state)
		{
			StateObject.SetActive(State == state);
		}
	}
}