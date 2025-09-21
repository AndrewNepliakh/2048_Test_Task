using UI;
using System;
using Zenject;
using Managers.SaveManager;
using Managers.UserManager;
using Managers.CubesManager;
using System.Threading.Tasks;
using Services.StateMachines;
using UnityEngine.SceneManagement;
using Implementations.Game.Signals;

namespace Managers
{
    public class GameManager : IGameManager, IInitializable, IDisposable
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private IAssetsManager _assetsManager;
        [Inject] private ICubesManager _cubesManager;
        [Inject] private IUserManager _userManager;
        [Inject] private ISaveManager _saveManager;
        [Inject] private IUIManager _uiManager;
        
        [Inject] private GameConfig _gameConfig;
        [Inject] private SignalBus _signalBus;

        private bool _isOnWin;
        
        public event Action OnGameWin;

        public bool IsOnWin => _isOnWin;
        
        public void Initialize()
        {
            _cubesManager.OnCubeMergedEvent += OnCubeMerged;
            _signalBus.Subscribe<NextLevelSignal>(OnNextLevelSignal);
        }
        
        public async Task LoadScene(string sceneKey, LoadSceneMode mode)
        {
            await _assetsManager.LoadScene(sceneKey, mode);
        }

        public async Task UnloadScene(string sceneKey)
        {
            await _assetsManager.UnloadScene(sceneKey);
        }

        public void Dispose()
        {
            _cubesManager.OnCubeMergedEvent -= OnCubeMerged;
            _signalBus.Unsubscribe<NextLevelSignal>(OnNextLevelSignal);
        }
        
        private void OnNextLevelSignal() => _isOnWin = false;

        private void OnCubeMerged()
        {
            CheckForWin();
        }

        private void CheckForWin()
        {
            var currentMergeValue = _userManager.CurrentUser.GameData.CurrentMergeValue;
            var requiredMergeValue = _userManager.CurrentUser.GameData.RequiredMergeValue;

            if (currentMergeValue != requiredMergeValue) return;
            
            _isOnWin = true;
            
            _userManager.CurrentUser.GameData.LevelUp();
            _userManager.CurrentUser.GameData.SetCurrentMergeValue(0);
            _userManager.CurrentUser.GameData.SetRequiredMergeValue(
                _gameConfig.GetRequiredMergeValueByLevel(_userManager.CurrentUser.GameData.Level));
                
            _saveManager.Save();

            _uiManager.ShowPopup<WinPopup>();
            
            _gameplayStateMachine.ChangeState(GameplayStates.Win);
        }
    }
}