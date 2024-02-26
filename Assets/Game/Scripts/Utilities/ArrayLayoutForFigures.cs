namespace Game.Scripts.Utilities
{
	[System.Serializable]
	public class ArrayLayoutForFigures
	{
		public RowData[] Rows = new RowData[5]; //Grid of 5x5
	}
	[System.Serializable]
	public struct RowData
	{
		public bool[] Row;
	}
}