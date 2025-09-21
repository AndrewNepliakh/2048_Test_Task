using System;
using UnityEngine;

[Serializable]
public class GameConfigData
{
    [SerializeField] private int _level;
    [SerializeField] private int _requiredMergeValue;
    
    public int Level => _level;
    public int RequiredMergeValue => _requiredMergeValue;
}