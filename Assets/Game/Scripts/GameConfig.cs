using Game.Scripts.Board;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Game/Configs/GameConfig" , fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
	    public BoardConfig BoardConfig;
	    public int InitKeysCount;
	    public int InitRotateBadgesCount;
    }

}
