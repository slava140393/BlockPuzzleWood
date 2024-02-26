using System;

namespace Game.Scripts.Services
{
	public interface IStorageService
	{
		public void Save(string key, object data);
		T Load<T>(string key);
	}
}