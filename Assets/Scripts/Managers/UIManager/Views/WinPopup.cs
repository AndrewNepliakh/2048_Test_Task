using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Implementations.Game.Signals;

namespace UI
{
    public class WinPopup : Window
    {
        [Inject] private SignalBus _signalBus;

        [SerializeField] private Button _nextLevelButton;
        
        public override void Show(UIViewArguments arguments)
        {
            base.Show(arguments);
            
            _nextLevelButton.onClick.AddListener(OnNextLevelButton);
        }

        private void OnNextLevelButton()
        {
            _signalBus.Fire<NextLevelSignal>();
        }

        public override void Hide()
        {
            base.Hide();
            
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButton);
        }
    }
}