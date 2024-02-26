using System;
using System.Collections.Generic;

namespace Game.Scripts.Figures
{
	[Serializable]
	public struct FigureOrientationShape
	{
		public FigureOrientation Orientation;
		public List<bool> FigureShape;

		public FigureOrientationShape(FigureOrientation orientation, List<bool> figureShape)
		{
			Orientation = orientation;
			FigureShape = new List<bool>(figureShape);
		}
	}
}