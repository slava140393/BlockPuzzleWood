namespace Game.Scripts.Services
{
	public interface ISavableData
	{
		public void OnSave(GameStorageItem item);
		public bool OnLoad(GameStorageItem item);

	}
}