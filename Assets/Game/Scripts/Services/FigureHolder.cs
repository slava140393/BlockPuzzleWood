using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Figures;
using UnityEngine;

namespace Game.Scripts.Services
{
	public class FigureHolder : FigureHolderBase
	{
		public Action<List<FigureOrientationShape>, int> OnFigureDroppedOnLockedHolder;

		public override void InitializeHolder(Figure figure, Canvas canvas, Vector2 figureSize, ConsumablesKeeperService consumablesKeeperService)
		{
			base.InitializeHolder(figure, canvas, figureSize,consumablesKeeperService);
			figure.OnDropOnLockedHolder += OnDropOnLockedHolder;

		}

		public void SetupNewFigure(List<FigureOrientationShape> shapes, int shapeIndex = 0)
		{
			if(_shapes != null && _shapes.Count > 0)
			{
				_shapes.Clear();
			}
			_shapeIndex = shapeIndex;
			_shapes = new List<FigureOrientationShape>(shapes);
			_figure.UpdateFigure(GetShape(_shapes, _shapeIndex), this.transform);
		}
		private List<bool> GetShape(List<FigureOrientationShape> shapes, int shapeIndex)
		{
			if(shapes.Count > shapeIndex)
			{
				return shapes[shapeIndex].FigureShape;
			}
			_shapeIndex = 0;
			return shapes.First().FigureShape;
		}

		private void OnDropOnLockedHolder()
		{
			OnFigureDroppedOnLockedHolder?.Invoke(_shapes, _shapeIndex);
		}

		private void OnDisable()
		{
			_figure.OnDropOnLockedHolder -= OnDropOnLockedHolder;
		}
	}
}