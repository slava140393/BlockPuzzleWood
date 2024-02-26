using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Services
{
	public class UIService : MonoBehaviour
	{
		public static Action<bool> OnMuteButtonClicked;
		public static Action OnRestartButtonClicked;
		[field: SerializeField] private TMP_Text _bestScoreText;
		[field: SerializeField] private TMP_Text _currentScoreText;

		[field: SerializeField] private Button _rotateFiguresButton;
		private bool _isRotateFigureButtonPressed;

		[field: SerializeField] private TMP_Text _countKeysText;
		[field: SerializeField] private TMP_Text _countRotateBadgesText;

		[field: SerializeField] private Button _menuButton;
		[field: SerializeField] private Button _muteButton;
		[field: SerializeField] private CanvasGroup _mute;
		[field: SerializeField] private CanvasGroup _unmute;
		[field: SerializeField] private Button _restartButton;
		[field: SerializeField] private CanvasGroup _menu;
		


		private CalculateScoreService _calculateScoreService;
		private ConsumablesKeeperService _consumablesKeeperService;
		private bool isMute = false;

		public void Initialize(CalculateScoreService calculateScoreService, ConsumablesKeeperService consumablesKeeperService)
		{
			_calculateScoreService = calculateScoreService;
			_calculateScoreService.OnCurrentScoreUpdated += UpdateCurrentScore;
			_calculateScoreService.OnBestScoreUpdated += UpdateBestScore;
			_calculateScoreService.OnTakeNewRecord += ShowNewRecordPopupWindow;

			_consumablesKeeperService = consumablesKeeperService;
			_consumablesKeeperService.OnRotateBadgesCountChanged += UpdateRotateBadgesCount;
			_consumablesKeeperService.OnKeysCountChanged += UpdateKeysCount;


			_rotateFiguresButton.onClick.AddListener(_consumablesKeeperService.OnRotateFigureButtonPressed);
			
			_menuButton.onClick.AddListener(ToggleMenu);
			_muteButton.onClick.AddListener(()=>OnMuteButtonClicked?.Invoke(!isMute));
			_muteButton.onClick.AddListener(ChangeMuteButton);
			
			_restartButton.onClick.AddListener(()=>OnRestartButtonClicked?.Invoke());
			
		}
		private void ToggleMenu()
		{
			ToggleCanvasGroup(_menu);
		}
		private void ChangeMuteButton()
		{
			ToggleCanvasGroup(_mute);
			ToggleCanvasGroup(_unmute);
		}
		private void ToggleCanvasGroup(CanvasGroup group)
		{
			if(group.blocksRaycasts==false)
			{
				group.blocksRaycasts = true;
				group.alpha = 1;
			}
			else
			{
				group.blocksRaycasts = false;
				group.alpha = 0;
			}
		}

		private void UpdateCurrentScore(int score) => _currentScoreText.text = score.ToString();
		private void UpdateBestScore(int score) => _bestScoreText.text = score.ToString();
		private void ShowNewRecordPopupWindow()
		{

		}

		private void UpdateRotateBadgesCount(int count) => _countRotateBadgesText.text = count.ToString();
		private void UpdateKeysCount(int count) => _countKeysText.text = count.ToString();


		private void OnDisable()
		{
			_calculateScoreService.OnCurrentScoreUpdated -= UpdateCurrentScore;
			_calculateScoreService.OnBestScoreUpdated -= UpdateBestScore;
			_calculateScoreService.OnTakeNewRecord -= ShowNewRecordPopupWindow;

			_consumablesKeeperService.OnRotateBadgesCountChanged -= UpdateRotateBadgesCount;
			_consumablesKeeperService.OnKeysCountChanged -= UpdateKeysCount;
			
			_rotateFiguresButton.onClick.RemoveListener(_consumablesKeeperService.OnRotateFigureButtonPressed);
			
			_menuButton.onClick.RemoveListener(ToggleMenu);
			_muteButton.onClick.RemoveListener(()=>OnMuteButtonClicked?.Invoke(!isMute));
			_muteButton.onClick.RemoveListener(ChangeMuteButton);
			
			_restartButton.onClick.RemoveListener(()=>OnRestartButtonClicked?.Invoke());
		}
	}

}