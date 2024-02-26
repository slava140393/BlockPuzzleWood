using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Board
{
	[CreateAssetMenu(menuName = "Game/Configs/TutorialBoardConfig" , fileName = "TutorialBoardConfig")]
	public class TutorialBoardConfig : BoardConfig
	{
		public List<bool> BoardHints = new List<bool>();//Hint cells for tutorial
	}
}