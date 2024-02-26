using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Figures;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Utilities
{
	public class FigureCreator : MonoBehaviour
	{
		[SerializeField] private FigureConfig _config;
		[SerializeField] private ArrayLayoutForFigures _shape;
		[SerializeField] private FigureOrientation _orientation;
	    #if UNITY_EDITOR
		public bool SetupFigure;
		private void OnValidate()
		{
			if(SetupFigure)
			{
				SetupFigure = false;

				if(_config == null)
				{
					throw new Exception("Add figure config");
				}

				if(_config.Shape == null || _config.Shape.Count == 0)
				{
					_config.Shape = new List<FigureOrientationShape>();
					_config.Shape.Add(new FigureOrientationShape(_orientation, GetShape()));
				}
				else
				{
					_config.Shape.Clear();
					FigureOrientationShape figureOrientationShape = _config.Shape.SingleOrDefault(s => s.Orientation == _orientation);

					if(figureOrientationShape.FigureShape == null)
					{
						_config.Shape.Add(new FigureOrientationShape(_orientation, GetShape()));
					}
					else
					{
						figureOrientationShape.FigureShape = new List<bool>(GetShape());
					}
				}
				_config.Shape.OrderBy(x => (int)(x.Orientation));
				EditorUtility.SetDirty(_config);
			}
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
	}
}