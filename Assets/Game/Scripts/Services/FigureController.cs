using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Figures;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Game.Scripts.Services
{
	public class FigureController : MonoBehaviour, ISavableData
	{
		public static Action OnAnyFigureDroppedToLockedHolder;
		public static Action OnAnyFigureDroppedOnBoard;
		public static Action OnAnyFigureDroppedBack;
		public static Action<bool> OnUsedAnyRotatedFigure;
		private const int MaximumFigures = 4;
		[SerializeField] private List<FigureHolder> _figureHolders;
		[SerializeField] private LockedFigureHolder _lockedHolder;
		[SerializeField] private RectTransform _container;
		[SerializeField] private Canvas _mainCanvas;
		private PoolFigure _poolFigure;
		private List<FigureConfig> _figureConfigs;
		private Vector2 _figureSize;
		private ConsumablesKeeperService _consumablesKeeperService;

		public void Initialize(AllFigureConfigs allFigureConfigs, Vector2 figureSize, ConsumablesKeeperService consumablesKeeperService)
		{
			_figureConfigs = new List<FigureConfig>(allFigureConfigs.FigureConfigs);
			_poolFigure = new PoolFigure(allFigureConfigs.FigurePrefab, MaximumFigures, _container);
			_figureSize = figureSize;
			_consumablesKeeperService = consumablesKeeperService;
			_consumablesKeeperService.OnStartRotateFigureMode += OnStartRotateFigureMode;

			InitializeFigureHolders();
			InitializeLockedHolder();
		}


		public void OnStartGame(List<FigureSavableData> figureShapes = null)
		{
			if(figureShapes != null)
			{
				for( int i = 0; i < figureShapes.Count; i++ )
				{
					if(_figureHolders.Count == i)
					{
						_lockedHolder.SetupNewFigure(figureShapes[i].Shapes, 0);
						return;
					}
					SetupFigureInHolder(_figureHolders[i], figureShapes[i].Shapes, figureShapes[i].Orientaion);
				}

			}
			else
			{
				foreach (FigureHolder figureHolder in _figureHolders)
				{
					SetupFigureInHolder(figureHolder);
				}
				_lockedHolder.ClearFigure();
			}
			foreach (FigureHolder figureHolder in _figureHolders)
			{
				figureHolder.ShowRotateFigureButton(false);
			}
			_lockedHolder.ShowRotateFigureButton(false);
		}

		public Transform GetFigureHolder(int holder)
		{
			if(_figureHolders.Count > holder)
			{
				return _figureHolders[holder].transform;
			}
			return null;
		}

		private void InitializeFigureHolders()
		{
			foreach (FigureHolder holder in _figureHolders)
			{
				Figure figure = CreateFigure();
				holder.InitializeHolder(figure, _mainCanvas, _figureSize, _consumablesKeeperService);
				holder.OnFigureDroppedOnLockedHolder += CopyFigureToLockedHolder;
				figure.OnUseRotatedFigure += b => OnUsedAnyRotatedFigure?.Invoke(b);
				figure.OnDropOnBoard += () => OnAnyFigureDroppedOnBoard?.Invoke();
				figure.OnDropBack += () => OnAnyFigureDroppedBack?.Invoke();
			}
		}
		private void InitializeLockedHolder()
		{
			Figure figure = CreateFigure();
			_lockedHolder.InitializeHolder(figure, _mainCanvas, _figureSize, _consumablesKeeperService);
			figure.OnUseRotatedFigure += b => OnUsedAnyRotatedFigure?.Invoke(b);
			figure.OnDropOnBoard += () => OnAnyFigureDroppedOnBoard?.Invoke();
			figure.OnDropBack += () => OnAnyFigureDroppedBack?.Invoke();
		}
		private void CopyFigureToLockedHolder(List<FigureOrientationShape> shapes, int shapeIndex)
		{
			if(shapes.Count > shapeIndex)
			{
				_lockedHolder.SetupNewFigure(shapes, shapeIndex);
			}
			OnAnyFigureDroppedToLockedHolder?.Invoke();
		}
		private void OnStartRotateFigureMode(bool isActive)
		{
			foreach (FigureHolder holder in _figureHolders)
			{
				holder.ShowRotateFigureButton(isActive);
			}
			_lockedHolder.ShowRotateFigureButton(isActive);
		}
		private void CreateNewFigures()
		{
			foreach (FigureHolder holder in _figureHolders)
			{
				if(holder.IsActiveFigure)
					return;
			}

			foreach (FigureHolder holder in _figureHolders)
			{
				SetupFigureInHolder(holder);
			}
		}
		private Figure CreateFigure()
		{
			Figure figure = _poolFigure.GetFreeElement();
			figure.gameObject.SetActive(true);
			figure.OnDropOnBoard = CreateNewFigures;
			return figure;
		}

		private void SetupFigureInHolder(FigureHolder holder, List<FigureOrientationShape> shapes = null, int shapeIndex = 0)
		{
			List<FigureOrientationShape> newShapes = shapes == null ? GetRandomFigureShape() : shapes;
			holder.SetupNewFigure(newShapes, shapeIndex);
		}
		private List<FigureOrientationShape> GetRandomFigureShape()
		{
			int rand = Random.Range(0, _figureConfigs.Count);
			return _figureConfigs.ElementAt(rand).Shape;
		}

		private void OnDisable()
		{
			foreach (Figure figure in _poolFigure.GetAllElements())
			{
				figure.OnDropOnBoard -= CreateNewFigures;
				figure.OnUseRotatedFigure -= b => OnUsedAnyRotatedFigure?.Invoke(b);
				figure.OnDropOnBoard -= () => OnAnyFigureDroppedOnBoard?.Invoke();
				figure.OnDropBack -= () => OnAnyFigureDroppedBack?.Invoke();
			}
		}



		public void OnSave(GameStorageItem item)
		{
			item.Figure1 = _figureHolders[0].GetData();
			item.Figure2 = _figureHolders[1].GetData();
			item.Figure3 = _figureHolders[2].GetData();

			item.Locked = _lockedHolder.GetData();
		}
		public bool OnLoad(GameStorageItem item)
		{
			List<FigureSavableData> figures = new List<FigureSavableData>();
			figures.Add(item.Figure1);
			figures.Add(item.Figure2);
			figures.Add(item.Figure3);
			figures.Add(item.Locked);
			OnStartGame(figures);
			return true;
		}
	}

}