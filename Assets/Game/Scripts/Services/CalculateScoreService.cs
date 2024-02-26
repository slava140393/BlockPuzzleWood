using System;
using System.Collections.Generic;
using Game.Scripts.Board;

namespace Game.Scripts.Services
{
	public class CalculateScoreService : ISavableData
	{
		public Action<int> OnCurrentScoreUpdated;
		public Action<int> OnBestScoreUpdated;
		public Action OnTakeNewRecord;
		private int _currentScore;
		private int _bestScore;

		public CalculateScoreService()
		{
			BoardController.OnLinesRemove += UpdateScore;
		}

		public void OnStartGame(int bestScore = 0, int currentScore = 0)
		{
			_currentScore = currentScore;
			OnCurrentScoreUpdated?.Invoke(_currentScore);
			_bestScore = bestScore;
			OnBestScoreUpdated?.Invoke(_bestScore);
		}


		private void UpdateScore(int removedLinesCount)
		{
			int scoreToAdd = CalculateScore(removedLinesCount);

			if(_bestScore > _currentScore)
			{
				if(_bestScore - _currentScore + scoreToAdd >= 0)
				{
					_currentScore += scoreToAdd;
					OnCurrentScoreUpdated?.Invoke(_currentScore);
				}
				else
				{
					UpdateEqualValues(scoreToAdd);
					OnTakeNewRecord?.Invoke();
				}
			}
			else
			{
				UpdateEqualValues(scoreToAdd);
			}

		}
		private void UpdateEqualValues(int scoreToAdd)
		{
			_currentScore += scoreToAdd;
			_bestScore = _currentScore;
			OnCurrentScoreUpdated?.Invoke(_currentScore);
			OnBestScoreUpdated?.Invoke(_bestScore);
		}
		private int CalculateScore(int removedLinesCount)
		{
			return removedLinesCount;
		}


		public void UnregisterCallbacks()
		{
			BoardController.OnLinesRemove -= UpdateScore;
		}

		public void OnSave(GameStorageItem item)
		{
			item.Best = _bestScore;
			item.Current = _currentScore;

		}
		public bool OnLoad(GameStorageItem item)
		{
			OnStartGame(item.Best, item.Current);
			return true;
		}
	}
}