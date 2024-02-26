using System;
using System.Collections.Generic;
using Game.Scripts.Board;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Utilities
{
	public class BoardCreator : MonoBehaviour
	{
		[SerializeField] private BoardConfig _config;
		[SerializeField] private TutorialBoardConfig _tutorBoardConfig;
		[SerializeField] private ArrayLayoutForBoards _shape;
		[Space(70)]
		[SerializeField] private TypeBoard _typeBoard;
		[SerializeField] private bool _setupBoard;
	    #if UNITY_EDITOR
		private void OnValidate()
		{
			if(_setupBoard)
			{
				_setupBoard = false;

				switch(_typeBoard)
				{

					case TypeBoard.Base:
						CreateBoard();
						break;
					case TypeBoard.Tutorial:
						CreateTutorialBoard();
						break;
				}

			}
		}
		private void CreateBoard()
		{
			if(_config == null)
			{
				throw new Exception("Add figure config");
			}
			_config.Board.Clear();
			_config.Board = new List<bool>(GetShape());
			EditorUtility.SetDirty(_config);
		}
		private void CreateTutorialBoard()
		{
			if(_tutorBoardConfig == null)
			{
				throw new Exception("Add figure config");
			}
			_tutorBoardConfig.BoardHints.Clear();
			_tutorBoardConfig.BoardHints = new List<bool>(GetShape());
			EditorUtility.SetDirty(_tutorBoardConfig);
		}
		private List<bool> GetShape()
		{
			List<bool> shape = new List<bool>();
			int rows = _shape.Rows.Length;

			for( int i = 0; i < rows; i++ )
			{
				for( int j = 0; j < rows; j++ )
				{
					shape.Add(_shape.Rows[i].Row[j]);
				}
			}
			return shape;
		}

	#endif

		private enum TypeBoard
		{
			Base,
			Tutorial
		}
	}
}