using System.Collections.Generic;
using Game.Scripts.Figures;
using UnityEngine;

namespace Game.Scripts.Services
{
	public class LockedFigureHolder : FigureHolderBase
	{
		public override void InitializeHolder(Figure figure, Canvas canvas, Vector2 figureSize, ConsumablesKeeperService consumablesKeeperService)
		{
			base.InitializeHolder(figure, canvas, figureSize, consumablesKeeperService);
			_figure.gameObject.SetActive(false);
			_consumablesKeeperService.OnKeysCountChanged += OnKeysCountChanged;
		}
		public bool CanUseLockedHolder { get; private set; }
		public void SetupNewFigure(List<FigureOrientationShape> shapes, int shapeIndex)
		{
			if(_shapes != null && _shapes.Count > 0)
			{
				_shapes.Clear();
			}
			_shapeIndex = shapeIndex;
			_shapes = new List<FigureOrientationShape>(shapes);

			if(_shapes.Count > _shapeIndex)
			{
				_figure.UpdateFigure(_shapes[_shapeIndex].FigureShape, this.transform);
			}
		}

		public bool IsEmpty => !_figure.gameObject.activeSelf;
		public void ClearFigure() => _figure.Clear();
		private void OnKeysCountChanged(int keys) => CanUseLockedHolder = keys > 0;


		private void OnDisable()
		{
			_consumablesKeeperService.OnKeysCountChanged -= OnKeysCountChanged;
		}
	}
}