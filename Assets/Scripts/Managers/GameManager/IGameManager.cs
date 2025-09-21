using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Managers
{
    public interface IGameManager
    {
        public event Action OnGameWin;

        public bool IsOnWin { get; }

        Task LoadScene(string sceneKey, LoadSceneMode mode);

        Task UnloadScene(string sceneKey);
    }
}