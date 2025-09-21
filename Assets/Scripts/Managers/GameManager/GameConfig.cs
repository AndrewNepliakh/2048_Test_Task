using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private List<GameConfigData> _levels = new();
    
    public List<GameConfigData> Levels => _levels;

    public int GetRequiredMergeValueByLevel(int level)
    {
        return level > _levels.Count ? _levels[^1].RequiredMergeValue :
            _levels.First(x => x.Level == level).RequiredMergeValue;
    }
}