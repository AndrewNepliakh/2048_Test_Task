using UI;
using System;
using Zenject;
using System.Threading.Tasks;
using Implementations.Game.Signals;

namespace Controllers
{
    public class WinGameplayStateController : IGameplayStateController
    {
        [Inject] private IUIManager _uiManager;
        [Inject] private SignalBus _signalBus;
        
        public event Action OnStateComplete;
        
        public Task Init()
        {
            _signalBus.Subscribe<NextLevelSignal>(OnNextLevelSignal);
            return Task.CompletedTask;
        }

        private void OnNextLevelSignal()
        {
            OnStateComplete?.Invoke();
            _uiManager.HideCurrentPopup();
            _signalBus.Unsubscribe<NextLevelSignal>(OnNextLevelSignal);
        }
    }
}