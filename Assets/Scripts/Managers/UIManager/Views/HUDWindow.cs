using TMPro;
using Zenject;
using Managers;
using UnityEngine;
using Managers.UserManager;
using Managers.CubesManager;
using Implementations.Game.Signals;

namespace UI
{
    public class HUDWindow : Window
    {
        [Inject] private ICubesManager _cubesManager;
        [Inject] private IUserManager _userManager;
        [Inject] private IGameManager _gameManager;
        [Inject] private SignalBus _signalBus;

        [SerializeField] private TMP_Text _LevelText;
        [SerializeField] private TMP_Text _userIdText;
        [SerializeField] private TMP_Text _scoreToWinText;
        [SerializeField] private TMP_Text _currentMergeValueText;

        public override void Show(UIViewArguments arguments)
        {
            base.Show(arguments);

            UpdateAppearance();

            _cubesManager.OnCubeMergedEvent += OnCubeMerged;
            
            _signalBus.Subscribe<NextLevelSignal>(OnNextLevelSignal);
        }

        public override void Hide()
        {
            base.Hide();
            
            _cubesManager.OnCubeMergedEvent -= OnCubeMerged;
            
            _signalBus.Unsubscribe<NextLevelSignal>(OnNextLevelSignal);
        }

        private void UpdateAppearance()
        {
            _currentMergeValueText.text = "Current merge value: " + _userManager.CurrentUser.GameData.CurrentMergeValue;
            
            if(_gameManager.IsOnWin) return;
            
            _userIdText.text = "User Id: " + _userManager.CurrentUser.UserId[..5];
            _LevelText.text = "Level: " + _userManager.CurrentUser.GameData.Level;
            _scoreToWinText.text = "Required merge value: " + _userManager.CurrentUser.GameData.RequiredMergeValue;
        }

        private void OnNextLevelSignal() => UpdateAppearance();
        
        private void OnCubeMerged() => UpdateAppearance();
    }
}