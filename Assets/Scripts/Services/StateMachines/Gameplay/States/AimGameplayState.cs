using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services.StateMachines
{
    public class AimGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Aim;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private AimGameplayStateController _controller;

        public async Task Enter(ChangeStateData changeStateData = null)
        {
            _controller.OnStateComplete += StateCompleteHandler;
            await _controller.Init();
        }

        private void StateCompleteHandler()
        {
            _gameplayStateMachine.ChangeState(GameplayStates.Result);
        }

        public void Exit()
        {
            _controller.OnStateComplete -= StateCompleteHandler;
        }

        public void Update(float deltaTime)
        {
        }
    }
}