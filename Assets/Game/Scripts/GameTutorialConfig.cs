using System.Collections.Generic;
using Game.Scripts.Board;
using Game.Scripts.Figures;
using UnityEngine;

namespace Game.Scripts
{
	[CreateAssetMenu(menuName = "Game/Configs/GameTutorialConfig" , fileName = "GameTutorialConfig")]
	public class GameTutorialConfig : ScriptableObject
	{
		public TutorialBoardConfig BoardConfig;
		public List<FigureConfig> StartFigures;
		public int InitKeysCount;
		public int InitRotateBadgesCount;
	}
}