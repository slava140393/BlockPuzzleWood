using System.IO;
using UnityEngine;

namespace Game.Scripts.Services
{
	public class JsonSaveLoadService : IStorageService
	{
		private IStorageService _storageServiceImplementation;

		public void Save(string key, object data)
		{
			string json = JsonUtility.ToJson(data);
			PlayerPrefs.SetString(key, json);
		}
		public T Load<T>(string key)
		{
			string toJson = PlayerPrefs.GetString(key);
			return JsonUtility.FromJson<T>(toJson);
		}
	}

}