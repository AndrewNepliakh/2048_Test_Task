using System;
using Zenject;
using System.Threading.Tasks;

namespace Services.StateMachines
{
    public class SetupGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Setup;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private SetupGameplayStateController _controller;

        public Action OnStateComplete;

        public async Task Enter(ChangeStateData changeStateData)
        {
            OnStateComplete += StateCompleteHandler;
            await _controller.Init();
        }

        private void StateCompleteHandler()
        {
            _gameplayStateMachine.ChangeState(GameplayStates.Aim);
        }

        public void Exit()
        {
            OnStateComplete -= StateCompleteHandler;
        }

        public void Update(float deltaTime)
        {
        }
    }
}