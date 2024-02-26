using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Services;
using Game.Scripts.Utilities;
using UnityEngine;

namespace Game.Scripts.Board
{
	public class BoardController : MonoBehaviour, ISavableData
	{
		public static Action<int> OnLinesRemove;
		private const int BoardSize = 100;
		private const int SideLenght = 10;
		[SerializeField] private FlexibleGridLayout _grid;
		[SerializeField] private GameObject _container;
		[SerializeField] private List<Cell> _currentStateBoard = new List<Cell>();

		private Cell[,] _rows = new Cell[10, 10];
		private Cell[,] _columns = new Cell[10, 10];

		private Coroutine _checkRoutine;
		private List<int> _checkRowList = new List<int>();
		private List<int> _checkColumnList = new List<int>();

		private CalculateScoreService _calculateScoreService;

		public void InitializeBoard()
		{
			for( int i = 0; i < _currentStateBoard.Count; i++ )
			{
				_currentStateBoard[i].SetCellPosition(i / SideLenght, i % SideLenght);
				_rows[i / SideLenght, i % SideLenght] = _currentStateBoard[i];
				_columns[i % SideLenght, i / SideLenght] = _currentStateBoard[i];
				_currentStateBoard[i].OnCellFilled += OnCellFilled;
			}

		}

		public void OnStartGame(List<bool> loadedBoard = null)
		{
			for( int i = 0; i < _currentStateBoard.Count; i++ )
			{
				CellState state;

				if(loadedBoard != null && loadedBoard.Count == BoardSize)
				{
					state = loadedBoard[i] ? CellState.Fill : CellState.Empty;
				}
				else
				{
					state = CellState.Empty;
				}
				_currentStateBoard[i].SetCellState(state);
			}
		}

		public void OnTutorialStart(List<bool> board, bool isHint)
		{
			for( int i = 0; i < _currentStateBoard.Count; i++ )
			{
				if(board[i])
				{
					_currentStateBoard[i].SetCellState(isHint ? CellState.Hint : CellState.Fill, true);
				}
				else
				{
					_currentStateBoard[i].SetCellState(CellState.Empty, false);
				}

			}
		}

		public Vector2 GetGridSize() => _grid.CellSize;
		private void OnCellFilled(int x, int y)
		{
			_checkRowList.Add(x);
			_checkColumnList.Add(y);

			if(_checkRoutine == null)
			{
				_checkRoutine = StartCoroutine(CheckBoard());
			}
		}
		private IEnumerator CheckBoard()
		{
			yield return new WaitForEndOfFrame();
			int linesToRemove = 0;
			linesToRemove += RowsToRemove();
			linesToRemove += ColumnsToRemove();

			_checkColumnList.Clear();

			if(linesToRemove > 0)
			{
				OnLinesRemove?.Invoke(linesToRemove);
			}
			_checkRoutine = null;
		}
		private int RowsToRemove()
		{
			_checkRowList = _checkRowList.Distinct().ToList();

			for( int i = _checkRowList.Count() - 1; i >= 0; i-- )
			{
				if(!CheckRow(_checkRowList[i]))
				{
					_checkRowList.RemoveAt(i);
				}
			}

			int rowsToRemove = _checkRowList.Count;

			if(rowsToRemove > 0)
			{
				StartCoroutine(RemoveRowLines());
			}
			return rowsToRemove;

		}
		private int ColumnsToRemove()
		{
			_checkColumnList = _checkColumnList.Distinct().ToList();

			for( int i = _checkColumnList.Count() - 1; i >= 0; i-- )
			{
				if(!CheckColumn(_checkColumnList[i]))
				{
					_checkColumnList.RemoveAt(i);
				}
			}
			int columnsToRemove = _checkColumnList.Count;

			if(columnsToRemove > 0)
			{
				StartCoroutine(RemoveColumnLines());
			}
			return columnsToRemove;
		}

		private bool CheckRow(int row)
		{
			for( int i = 0; i < SideLenght; i++ )
			{
				if(_rows[row, i].IsEmpty)
					return false;
			}
			return true;
		}
		private bool CheckColumn(int column)
		{
			for( int i = 0; i < SideLenght; i++ )
			{
				if(_columns[column, i].IsEmpty)
					return false;
			}
			return true;
		}

		private IEnumerator RemoveRowLines()
		{
			for( int i = 0; i < SideLenght; i++ )
			{
				foreach (int row in _checkRowList)
				{
					_rows[row, i].Remove();

				}
			}
			_checkRowList.Clear();
			yield return null;
		}

		private IEnumerator RemoveColumnLines()
		{
			for( int i = 0; i < SideLenght; i++ )
			{
				foreach (int column in _checkColumnList)
				{
					_columns[column, i].Remove();

				}
			}
			_checkColumnList.Clear();
			yield return null;
		}
		public void OnSave(GameStorageItem item)
		{

			if(item.Board != null)
			{
				item.Board.Clear();
			}

			item.Board = _currentStateBoard.Select(cell => !cell.IsEmpty).ToList();
		}
		public bool OnLoad(GameStorageItem item)
		{
			if(item == null)
				return false;

			List<bool> board = new List<bool>(item.Board);
			OnStartGame(board);
			return true;
		}

	#if UNITY_EDITOR
		public bool FillBoard;


		private void OnValidate()
		{
			if(_container != null && FillBoard)
			{
				FillBoard = false;
				_currentStateBoard = _container.GetComponentsInChildren<Cell>().ToList();
				float sqrRt = Mathf.Sqrt(_currentStateBoard.Count);
				int rows = Mathf.CeilToInt(sqrRt);

				for( int i = 0; i < _currentStateBoard.Count; i++ )
				{
					_currentStateBoard[i].SetCellPosition(i / rows, i % rows);
					_currentStateBoard[i].gameObject.name = $"Cell[{i / rows} {i % rows}]";
				}
			}
		}
	#endif



	}

}