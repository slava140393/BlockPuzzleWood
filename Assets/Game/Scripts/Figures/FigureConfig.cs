using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Figures
{
	[CreateAssetMenu(menuName = "Game/Configs/Figure", fileName = "Figure")]
	public class FigureConfig : ScriptableObject
	{
		public List<FigureOrientationShape> Shape;
	}

}