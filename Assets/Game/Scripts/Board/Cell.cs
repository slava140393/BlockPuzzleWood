using System;
using UnityEngine;

namespace Game.Scripts.Board
{
	public class Cell : MonoBehaviour
	{
		private const string PlayAnimTrigger = "Play";
		private static readonly int Play = Animator.StringToHash(PlayAnimTrigger);

		public Action<int, int> OnCellFilled;
		[SerializeField] private CellStateObject[] _cellStateObjects;
		[SerializeField] private BoxCollider2D _collider;
		[SerializeField] private Animator _animator;

		private CellState _currentState;
		private bool _isUsable = true;
		public CellState CurrentState => _currentState;
		public int X { get; private set; }
		public int Y { get; private set; }

		public void SetCellPosition(int x, int y)
		{
			X = x;
			Y = y;
		}
		public void SetCellState(CellState state)
		{
			_isUsable = true;
			foreach (CellStateObject cellStateObject in _cellStateObjects)
			{
				cellStateObject.Setup(state);
			}
			_currentState = state;

			if(state == CellState.Fill)
			{
				OnCellFilled?.Invoke(X, Y);
			}
		}

		public void SetCellState(CellState state, bool isUsable)
		{
			_isUsable = isUsable;

			if(isUsable)
			{
				foreach (CellStateObject cellStateObject in _cellStateObjects)
				{
					cellStateObject.Setup(state);
				}
				_currentState = state;
			}
		}
		public bool IsEmpty => _currentState == CellState.Empty || _currentState == CellState.Hint;
		public bool IsUsable => _isUsable;
		public bool IsPre => _currentState == CellState.Pre;
	
		public void Remove()
		{
			SetCellState(CellState.PreFilled);
			_animator.SetTrigger(Play);
		}

		private void OnEndRemove() => SetCellState(CellState.Empty);
	}

}