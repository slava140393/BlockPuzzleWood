using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Board;
using Game.Scripts.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Services
{
	public class Tutorial : MonoBehaviour
	{
		public static Action OnEndTutorials;
		[field: SerializeField] private RectTransform _hintHolder;
		[SerializeField] private List<GameTutorialConfig> _gameTutorialConfigs;
		[field: SerializeField] private RectTransform _handHint;
		[field: SerializeField] private CanvasGroup _handHintCanvasGroup;
		[field: SerializeField] private Button _rotateButton;
		[field: SerializeField] private Button _currentRotateButton;
		[field: SerializeField] private CanvasGroup _handHintCanvasGroup2;
		private bool _onTutorialEnded;
		private int _indexTutorial;
		private ConsumablesKeeperService _consumablesKeeperService;
		private BoardController _boardController;
		private FigureController _figureController;
		private Coroutine _handHintRoutine;
		private float _duration = 1.2f;
		private Vector2 _endHandHintPosition;
		private Vector2 _startHandHintPosition;

		private float _startTime;
		private float _waitTime = 1f;
		public void InitializeTutorial(ConsumablesKeeperService consumablesKeeperService, BoardController boardController, FigureController figureController)
		{
			_figureController = figureController;
			_boardController = boardController;
			_consumablesKeeperService = consumablesKeeperService;
			_indexTutorial = 0;
			ShowTutorialBars(false);
		}

		public void OnStartTutorial()
		{
			if(_gameTutorialConfigs.Count > _indexTutorial)
			{
				BoardController.OnLinesRemove += OnEndTutorial;
				ShowTutorialBars(true);
				_startTime = Time.time;
				_onTutorialEnded = true;
			}
		}

		private void Update()
		{
			if(Time.time < _startTime + _waitTime)
			{
				return;
			}

			if(_onTutorialEnded)
			{
				_onTutorialEnded = false;

				SetupFields();
			}
		}
		private void SetupFields()
		{
			GameTutorialConfig config = _gameTutorialConfigs[_indexTutorial];
			_consumablesKeeperService.OnStartGame(config.InitKeysCount, config.InitRotateBadgesCount);
			_boardController.OnTutorialStart(new List<bool>(config.BoardConfig.Board), false);
			_boardController.OnTutorialStart(new List<bool>(config.BoardConfig.BoardHints), true);
			CalculateStartPosition();
			_endHandHintPosition = CalculateCenterFigure(new List<bool>(config.BoardConfig.BoardHints));
			GetFigureShapes(config);
			_figureController.OnStartGame(GetFigureShapes(config));
			_handHintRoutine = StartCoroutine(MoveHintHand(_startHandHintPosition, _endHandHintPosition));

		}
		private List<FigureSavableData> GetFigureShapes(GameTutorialConfig config)
		{
			List<FigureSavableData> datas = new List<FigureSavableData>();

			foreach (FigureConfig figure in config.StartFigures)
			{
				datas.Add(new FigureSavableData()
				{
					Shapes = new List<FigureOrientationShape>(figure.Shape), Orientaion = 0
				});
			}
			return datas;
		}

		private void CalculateStartPosition()
		{
			_handHint.SetParent(_figureController.GetFigureHolder(0));
			_handHint.anchoredPosition = Vector2.zero;
			_handHint.SetParent(_hintHolder);
			_startHandHintPosition = _handHint.anchoredPosition;
		}
		private void OnEndTutorial(int obj)
		{
			_indexTutorial++;

			if(_gameTutorialConfigs.Count > _indexTutorial)
			{
				_startTime = Time.time;
				_onTutorialEnded = true;

				if(_indexTutorial == 1)
				{
					StartSecondTutorial();
				}
			}
			else
			{
				ShowTutorialBars(false);
				OnDisable();
				OnEndTutorials?.Invoke();
				this.enabled = false;

			}
		}
		private void StartSecondTutorial()
		{
			_handHintCanvasGroup.alpha = 0;
			(_handHintCanvasGroup2.transform as RectTransform).SetParent(_rotateButton.transform);
			(_handHintCanvasGroup2.transform as RectTransform).anchoredPosition = Vector2.zero;
			(_handHintCanvasGroup2.transform as RectTransform).SetParent(_hintHolder);
			_handHintCanvasGroup2.alpha = 1;
			_rotateButton.onClick.AddListener(SetHandHintSecond);
			_currentRotateButton.onClick.AddListener(HideHandHint);
		}
		private void HideHandHint()
		{
			_handHintCanvasGroup2.alpha = 0;
			_handHintCanvasGroup.alpha = 1;
			StartCoroutine(MoveHintHand(_startHandHintPosition, _endHandHintPosition));
		}
		private void SetHandHintSecond()
		{
			(_handHintCanvasGroup2.transform as RectTransform).SetParent(_currentRotateButton.transform);
			(_handHintCanvasGroup2.transform as RectTransform).anchoredPosition = Vector2.zero;
			(_handHintCanvasGroup2.transform as RectTransform).SetParent(_hintHolder);
		}



		private void ShowTutorialBars(bool isShow)
		{

			_handHintCanvasGroup.blocksRaycasts = false;
			_handHintCanvasGroup.alpha = isShow ? 1f : 0f;

			_handHintCanvasGroup2.alpha = 0;

			_hintHolder.gameObject.SetActive(isShow);
			this.enabled = isShow;
		}



		private IEnumerator MoveHintHand(Vector2 start, Vector2 end)
		{
			float counter = 0;


			while(!_onTutorialEnded)
			{
				counter += Time.deltaTime;

				if(ApplyPosition(start, end, counter / _duration))
				{
					_handHint.anchoredPosition = start;
					counter = 0;
					yield return new WaitForSeconds(0.2f);
				}

				yield return null;
			}
			yield return null;
		}

		private bool ApplyPosition(Vector2 oldPosition, Vector2 newPosition, float duration)
		{
			if(Vector2.Distance(_handHint.anchoredPosition, newPosition) < 0.01f)
			{
				return true;
			}
			_handHint.anchoredPosition = Vector2.Lerp(oldPosition, newPosition, duration);
			return false;
		}

		private Vector2 CalculateCenterFigure(List<bool> boardHint)
		{
			int x = 0, y = 0;
			int count = 0;

			for( int i = 0; i < boardHint.Count; i++ )
			{
				if(boardHint[i])
				{
					x += i / 10;
					y += i % 10;
					count++;
				}
			}

			if(count == 0)
			{
				return Vector2.zero;
			}
			return new Vector2(x / count, y / count);
		}

		private void OnDisable()
		{
			BoardController.OnLinesRemove -= OnEndTutorial;
			_rotateButton.onClick.RemoveListener(SetHandHintSecond);
			_currentRotateButton.onClick.RemoveListener(HideHandHint);

			if(_handHintRoutine != null)
			{
				StopCoroutine(_handHintRoutine);
			}
		}
	}
}