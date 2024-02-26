using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Board;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Figures
{
	public class Figure : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		private const int CountElements = 25;
		public Action OnDropBack;
		public Action OnDropOnBoard;
		public Action OnDropOnLockedHolder;
		public Action<bool> OnUseRotatedFigure;
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private RectTransform _takerCast;
		[SerializeField] private Vector2 _offset;
		[SerializeField] private float _offsetScale = 1f;
		[SerializeField] private float _duration = .3f;

		private List<FigureElement> _activeElements;
		private Vector2 _startPosition;
		private float _startScale = .5f;
		private Coroutine _takeFigureRoutine;
		private Canvas _canvas;
		private List<FigureElement> _allElements;
		private bool _isDrag;

		public void Initialize(Canvas canvas, Vector2 figureSize)
		{
			if(_allElements == null || _allElements.Count != CountElements)
			{
				_allElements = GetComponentsInChildren<FigureElement>().ToList();
			}
			_rectTransform.sizeDelta = figureSize * 5f;
			_takerCast.sizeDelta = figureSize * 3;
			_canvas = canvas;
		}

		public bool IsRotated { get; private set; }
		public void UpdateFigure(List<bool> figureShape, Transform parent, bool isRotated = false)
		{
			if(_allElements.Count == figureShape.Count)
			{
				for( int i = 0; i < _allElements.Count; i++ )
				{
					_allElements[i].SetupElement(figureShape[i]);
				}
			}
			else
				throw new Exception($"Size of figure ({_allElements.Count}) not equal size of config shape table ({figureShape.Count})");
			IsRotated = isRotated;
			gameObject.SetActive(true);
			SetStartRectTransform(parent);
			_activeElements = _allElements.FindAll(x => x.IsActive);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_canvasGroup.blocksRaycasts = false;
			_startPosition = _rectTransform.anchoredPosition;
			_takeFigureRoutine = StartCoroutine(TakeDropFigure(_startPosition, _startScale, _startPosition + _offset, _offsetScale));
		}
		public void OnPointerUp(PointerEventData eventData)
		{
			_canvasGroup.blocksRaycasts = true;
			_isDrag = false;
			StopCoroutine(_takeFigureRoutine);

			if(CheckLockedFigureHolder())
			{
				DisactiveFigure();
				return;
			}

			if(!CheckUnderElements(CellState.Fill))
			{
				StartCoroutine(TakeDropFigure(_startPosition + _offset, _offsetScale, _startPosition, _startScale));
				OnDropBack?.Invoke();
			}

			else
			{
				OnUseRotatedFigure?.Invoke(IsRotated);
				DisactiveFigure();
			}
		}
		private bool CheckLockedFigureHolder()
		{
			foreach (FigureElement element in _activeElements)
			{
				if(element.CastLockedHolder())
				{
					OnDropOnLockedHolder?.Invoke();
					return true;
				}
			}
			return false;
		}
		private void DisactiveFigure()
		{
			foreach (FigureElement element in _activeElements)
			{
				element.SetupElement(false);
			}
			gameObject.SetActive(false);
			OnDropOnBoard?.Invoke();
		}
		public void OnBeginDrag(PointerEventData eventData)
		{

		}
		public void OnDrag(PointerEventData eventData)
		{
			_isDrag = true;
			_rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
			CheckRayCastUnderElements(CellState.Pre);

		}
		public void Clear()
		{
			foreach (FigureElement element in _allElements)
			{
				element.SetupElement(false);
			}
		}

		private IEnumerator TakeDropFigure(Vector2 oldPosition, float oldScale, Vector2 newPosition, float newScale)
		{

			bool inOffsetScale = false;
			float counter = 0;

			while(!inOffsetScale)
			{
				counter += Time.deltaTime;

				if(!_isDrag)
					ApplyPosition(oldPosition, newPosition, counter / _duration);
				inOffsetScale = ApplyScale(oldScale, newScale, counter / _duration);
				yield return null;
			}
			yield return null;
		}

		private bool ApplyPosition(Vector2 oldPosition, Vector2 newPosition, float duration)
		{
			if(Vector2.Distance(_rectTransform.anchoredPosition, newPosition) < 0.01f)
			{
				return true;
			}
			_rectTransform.anchoredPosition = Vector2.Lerp(oldPosition, newPosition, duration);
			return false;
		}
		private bool ApplyScale(float startScale, float newScale, float duration)
		{
			if(Math.Abs(_rectTransform.localScale.x - newScale) < 0.01f)
			{
				return true;
			}
			float scaleFactor = Mathf.Lerp(startScale, newScale, duration);

			_rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			return false;
		}

		private bool CheckUnderElements(CellState cellState)
		{
			foreach (FigureElement element in _activeElements)
			{
				if(!element.AboveEmptyCell)
				{
					MarkElements(CellState.Empty);
					return false;
				}
			}
			MarkElements(cellState);
			return true;
		}

		private void CheckRayCastUnderElements(CellState cellState)
		{
			foreach (FigureElement element in _activeElements)
			{
				if(!element.CastCellsUnder())
				{
					MarkElements(CellState.Empty);
					return;
				}
			}
			MarkElements(cellState);
		}
		private void MarkElements(CellState cellState)
		{
			foreach (FigureElement element in _activeElements)
			{
				element.MarkCell(cellState);
			}
		}
		private void SetStartRectTransform(Transform parent)
		{
			_rectTransform.SetParent(parent, false);
			_rectTransform.anchoredPosition = Vector2.zero;
			_startPosition = _rectTransform.anchoredPosition;
			_rectTransform.localScale = new Vector3(_startScale, _startScale, _startScale);
		}
	}
}