using System;
using System.Collections.Generic;

namespace Services.StateMachines
{
    public abstract class StateMachine<T> where T : Enum
    {
        public T CurrentState => (_currentState != null) ? _currentState.State : default;
        public T PreviousState => (_previousState != null) ? _previousState.State : default;
        public event Action<IState<T>> OnChangeState;
        public event Action<IState<T>> OnAfterChangeState;

        protected IState<T> _currentState;
        protected IState<T> _previousState;

        protected readonly Dictionary<T, IState<T>> _states = new();

        public StateMachine() { }
        
        public void AddState(IState<T> state)
        {
            _states.Add(state.State, state);
        }

        public void ChangeState(T newState)
        {
            _currentState?.Exit();

            _previousState = _currentState;
            _currentState = _states[newState];
            OnChangeState?.Invoke(_currentState);

            _currentState.Enter();

            OnAfterChangeState?.Invoke(_currentState);
        }
        
        public void ChangeState(T newState, ChangeStateData changeStateData)
        {
            _currentState?.Exit();

            _previousState = _currentState;
            _currentState = _states[newState];
            OnChangeState?.Invoke(_currentState);

            _currentState.Enter(changeStateData);

            OnAfterChangeState?.Invoke(_currentState);
        }

        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }

        public void ExitAllStates()
        {
            foreach (var state in _states.Values) state.Exit();
        }
    }
}