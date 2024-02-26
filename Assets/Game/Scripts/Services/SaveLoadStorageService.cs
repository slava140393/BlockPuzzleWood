using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Services
{
	public class SaveLoadStorageService : MonoBehaviour
	{
		private const string Item = "Item";
		private IStorageService _storageService;
		private List<ISavableData> _savableDatas;
		private GameStorageItem _item;

		public void Initialize(List<ISavableData> savableDatas)
		{
			_savableDatas = savableDatas;
			_storageService = new JsonSaveLoadService();
			_item = new GameStorageItem();
		}


		public void OnStartGame()
		{
			FigureController.OnAnyFigureDroppedOnBoard += Save;

		}
		public bool Load()
		{
			GameStorageItem item = _storageService.Load<GameStorageItem>(Item);

			if(item == null)
			{
				return false;
			}

			_item = item;

			foreach (ISavableData savableData in _savableDatas)
			{
				if(!savableData.OnLoad(_item))
				{
					return false;
				}
			}
			return true;
		}
		private void Save()
		{
			foreach (ISavableData data in _savableDatas)
			{
				data.OnSave(_item);
			}
			_storageService.Save(Item, _item);
		}

	}


}