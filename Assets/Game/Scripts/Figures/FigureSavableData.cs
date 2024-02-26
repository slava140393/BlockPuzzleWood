using System;
using System.Collections.Generic;

namespace Game.Scripts.Figures
{
	[Serializable]
	public class FigureSavableData
	{
		public List<FigureOrientationShape> Shapes;
		public int Orientaion;
	}
}
