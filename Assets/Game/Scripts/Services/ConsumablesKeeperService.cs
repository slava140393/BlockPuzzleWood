using System;

namespace Game.Scripts.Services
{
	public class ConsumablesKeeperService : ISavableData
	{
		public Action<int> OnRotateBadgesCountChanged;
		public Action<int> OnKeysCountChanged;
		public Action<bool> OnStartRotateFigureMode;
		private bool _isRotateFigureButtonsActive;

		private int _rotateBadgesCount;
		private int _keysCount;
		public ConsumablesKeeperService()
		{
			FigureController.OnAnyFigureDroppedToLockedHolder += CalculateKeys;
			FigureController.OnUsedAnyRotatedFigure += CalculateRotateBadges;
		}

		public void OnStartGame(int keys, int rotateBadges)
		{
			_keysCount = keys;
			OnKeysCountChanged?.Invoke(_keysCount);

			_rotateBadgesCount = rotateBadges;
			OnRotateBadgesCountChanged?.Invoke(_rotateBadgesCount);
		}


		public void OnRotateFigureButtonPressed()
		{
			if(_rotateBadgesCount > 0)
			{
				_isRotateFigureButtonsActive = !_isRotateFigureButtonsActive;
				OnStartRotateFigureMode?.Invoke(_isRotateFigureButtonsActive);
			}
		}
		public void UnregisterCallbacks()
		{
			FigureController.OnAnyFigureDroppedToLockedHolder -= CalculateKeys;
		}
		private void CalculateKeys()
		{
			_keysCount -= 1;
			OnKeysCountChanged?.Invoke(_keysCount);
		}
		private void CalculateRotateBadges(bool isRotated)
		{
			if(isRotated)
			{
				_rotateBadgesCount -= 1;
				OnRotateBadgesCountChanged?.Invoke(_rotateBadgesCount);
			}

			if(_isRotateFigureButtonsActive)
			{
				OnRotateFigureButtonPressed();
			}

		}

		public void OnSave(GameStorageItem item)
		{
			item.Rotate = _rotateBadgesCount;
			item.Key = _keysCount;
		}
		public bool OnLoad(GameStorageItem item)
		{
			OnStartGame(item.Key, item.Rotate);
			return true;
		}
	}
}