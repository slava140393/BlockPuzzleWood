using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Board
{
	[CreateAssetMenu(menuName = "Game/Configs/BoardConfig" , fileName = "BoardConfig")]
    public class BoardConfig : ScriptableObject
    {
	    public List<bool> Board = new List<bool>();//Empty && Filled cells
    }

}
