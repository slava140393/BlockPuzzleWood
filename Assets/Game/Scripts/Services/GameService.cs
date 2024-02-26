using System;
using System.Collections.Generic;
using Game.Scripts.Board;
using Game.Scripts.Figures;
using UnityEngine;

namespace Game.Scripts.Services
{
	public class GameService : MonoBehaviour
	{
		[SerializeField] private GameConfig _baseConfig;
		[SerializeField] private AllFigureConfigs _allFigureConfigs;
		[SerializeField] private FigureController _figureController;
		[SerializeField] private BoardController _boardController;
		[SerializeField] private UIService _uiService;
		[SerializeField] private Tutorial _tutorial;
		[SerializeField] private SaveLoadStorageService _saveLoadStorageService;
		[SerializeField] private AudioService _audioService;

		private CalculateScoreService _calculateScoreService;
		private ConsumablesKeeperService _consumablesKeeperService;


		private List<ISavableData> _savableDatas=new List<ISavableData>();


		private void Start()
		{
			// PlayerPrefs.DeleteAll();
			// PlayerPrefs.Save();
			CreateServices();

			if(!_saveLoadStorageService.Load())
			{
				_tutorial.OnStartTutorial();
				_calculateScoreService.OnStartGame();
				Tutorial.OnEndTutorials += StartGame;
			}

		}
		private void CreateServices()
		{
			_boardController.InitializeBoard();
			_calculateScoreService = new CalculateScoreService();
			_consumablesKeeperService = new ConsumablesKeeperService();
			_figureController.Initialize(_allFigureConfigs, _boardController.GetGridSize(), _consumablesKeeperService);
			_uiService.Initialize(_calculateScoreService, _consumablesKeeperService);
			_tutorial.InitializeTutorial(_consumablesKeeperService, _boardController, _figureController);
			_audioService.Initialize();
			
			_savableDatas.Add(_boardController);
			_savableDatas.Add(_calculateScoreService);
			_savableDatas.Add(_consumablesKeeperService);
			_savableDatas.Add(_figureController);
			_saveLoadStorageService.Initialize(_savableDatas);

			UIService.OnRestartButtonClicked += StartGame;
		}

		private void StartGame()
		{
			_calculateScoreService.OnStartGame();
			_consumablesKeeperService.OnStartGame(_baseConfig.InitKeysCount, _baseConfig.InitRotateBadgesCount);
			_boardController.OnStartGame();
			_figureController.OnStartGame();
			_saveLoadStorageService.OnStartGame();
		}


		private void OnDisable()
		{
			_calculateScoreService.UnregisterCallbacks();
			_consumablesKeeperService.UnregisterCallbacks();
			UIService.OnRestartButtonClicked -= StartGame;

		}

	}
}