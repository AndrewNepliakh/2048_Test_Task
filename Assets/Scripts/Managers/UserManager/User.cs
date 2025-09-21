using System;
using Newtonsoft.Json;

namespace Managers.UserManager
{
    [Serializable]
    public class User
    {
        [JsonProperty] public string UserId { get; private set; }
        
        [JsonProperty] public GameData GameData { get; private set; }

        public User() : this(null, null) { }
        
        [JsonConstructor]
        public User(string userId, GameData gameData)
        {
            UserId = userId ?? Guid.NewGuid().ToString();
            GameData = gameData ?? new GameData();
        }

        public void UpdateCurrentMergeValue(int currentMergeValue)
        {
            GameData.SetCurrentMergeValue(currentMergeValue);
        }
    }

}