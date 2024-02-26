using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Figures
{
	[CreateAssetMenu(menuName = "Game/Configs/AllFigures", fileName = "AllFiguresConfig")]
	public class AllFigureConfigs : ScriptableObject
	{
		public Figure FigurePrefab;
		public List<FigureConfig> FigureConfigs;
	}
}

	


