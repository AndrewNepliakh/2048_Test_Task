using UI;
using Zenject;
using UnityEngine;
using Services.StateMachines;

namespace Implementations.Game.Controllers
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private IUIManager _uiManager;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        [SerializeField] private Canvas _mainCanvas;

        [Inject]
        private void Instantiation(
            SetupGameplayState setupGameplayState, 
            AimGameplayState aimGameplayState,
            ResultGameplayState resultGameplayState,
            WinGameplayState winGameplayState)
        {
            _uiManager.Init(_mainCanvas);
            _uiManager.ShowHUDWindow<HUDWindow>();
            
            _gameplayStateMachine.AddState(setupGameplayState);
            _gameplayStateMachine.AddState(aimGameplayState);
            _gameplayStateMachine.AddState(resultGameplayState);
            _gameplayStateMachine.AddState(winGameplayState);

            _gameplayStateMachine.ChangeState(GameplayStates.Setup);
        }
    }
}