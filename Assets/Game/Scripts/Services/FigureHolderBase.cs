using System;
using System.Collections.Generic;
using Game.Scripts.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Services
{
	public abstract class FigureHolderBase : MonoBehaviour
	{
		[SerializeField] protected Button _rotateFigureButton;
		[SerializeField] protected CanvasGroup _buttonCanvasGroup;
		protected List<FigureOrientationShape> _shapes=new List<FigureOrientationShape>();
		protected Figure _figure;
		protected int _shapeIndex;
		protected ConsumablesKeeperService _consumablesKeeperService;


		public virtual void InitializeHolder(Figure figure, Canvas canvas, Vector2 figureSize, ConsumablesKeeperService consumablesKeeperService)
		{
			_consumablesKeeperService = consumablesKeeperService;
			_figure = figure;
			_figure.Initialize(canvas, figureSize);
			_consumablesKeeperService.OnStartRotateFigureMode +=ShowRotateFigureButton;
			_rotateFigureButton.onClick.AddListener(RotateFigure);
			ShowRotateFigureButton(false);

		}

		public void ShowRotateFigureButton(bool isActive)
		{
			if(IsActiveFigure && _shapes.Count > 1)
			{
				_buttonCanvasGroup.blocksRaycasts = isActive;
				_buttonCanvasGroup.alpha = isActive ? 1f : 0f;
			}
		}

		private void RotateFigure()
		{
			if(_shapes.Count > 1)
			{
				_shapeIndex = (_shapeIndex + 1) % _shapes.Count;
				_figure.UpdateFigure(_shapes[_shapeIndex].FigureShape, this.transform, true);
			}
		}
		public bool IsActiveFigure => _figure.gameObject.activeSelf;
		public FigureSavableData GetData()
		{
			return new FigureSavableData()
			{
				Shapes = new List<FigureOrientationShape>(_shapes), Orientaion = _shapeIndex
			};
		}

		private void OnDisable()
		{
			_consumablesKeeperService.OnStartRotateFigureMode -=ShowRotateFigureButton;
		}
	}
}