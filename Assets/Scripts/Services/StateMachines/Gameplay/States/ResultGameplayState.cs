using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services.StateMachines
{
    public class ResultGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Result;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private ResultGameplayStateController _controller;

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
            _gameplayStateMachine.ChangeState(GameplayStates.Aim);
        }

        public void Update(float deltaTime)
        {
        }
    }
}