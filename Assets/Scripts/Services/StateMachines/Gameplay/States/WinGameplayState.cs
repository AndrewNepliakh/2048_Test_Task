using Zenject;
using Controllers;
using System.Threading.Tasks;
using Managers.CubesManager;

namespace Services.StateMachines
{
    public class WinGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Win;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private WinGameplayStateController _controller;
        [Inject] private ICubesManager _cubesManager;
        public async Task Enter(ChangeStateData changeStateData = null)
        {
            _controller.OnStateComplete += StateCompleteHandler;
            await _controller.Init();
        }

        public void Exit()
        {
            _controller.OnStateComplete -= StateCompleteHandler;
        }
    
        private void StateCompleteHandler()
        {
            _cubesManager.ClearAllCubes();
            _gameplayStateMachine.ChangeState(GameplayStates.Setup);
        }

        public void Update(float deltaTime)
        {
        }
    }
}