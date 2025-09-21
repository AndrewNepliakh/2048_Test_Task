using System;
using Newtonsoft.Json;

namespace Managers
{
    [Serializable]
    public class GameData
    {
        [JsonProperty] public int Level { get; private set; }
        [JsonProperty] public int CurrentMergeValue { get; private set; }
        [JsonProperty] public int RequiredMergeValue { get; private set; }

        public GameData()
        {
            Level = 1;
            CurrentMergeValue = 0;
            RequiredMergeValue = 16;
        }

        public GameData(int level, int currentMergeValue, int requiredMergeValue)
        {
            Level = level;
            CurrentMergeValue = currentMergeValue;
            RequiredMergeValue = requiredMergeValue;
        }
        
        public void SetLevel(int level)
        {
            if(level <= 1) return;

            Level = level;
        }
        
        public void SetRequiredMergeValue(int requiredMergeValue)
        {
            if(requiredMergeValue <= 0) return;

            RequiredMergeValue = requiredMergeValue;
        }

        public void SetCurrentMergeValue(int currentMergeValue)
        {
            if(currentMergeValue <= 0) return;
            
            if (CurrentMergeValue >= currentMergeValue) return;

            CurrentMergeValue = currentMergeValue;
        }

        public void LevelUp() => Level++;
    }
}