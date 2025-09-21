using TMPro;
using System;
using Zenject;
using UnityEngine;
using Services.StateMachines;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Managers.CubesManager
{
    public class Cube : MonoBehaviour, ICube
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        
        public CubeState CubeState => CubeData.CubeState;
        
        public event Action<CubeData> OnCubeMergedEvent;

        public CubeData CubeData 
        {
            get => _cubeData;
            
            private set
            {
                _cubeData = value;
                OnCubeStateChanged?.Invoke(_cubeData.CubeState);
            }
        }

        public Action<CubeState> OnCubeStateChanged;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private List<TMP_Text> _rateText;

        private ColorGenerator _colorGenerator;
        
        [SerializeField] private CubeData _cubeData;

        private async void OnEnable()
        {
            await WaitUntil(() => _gameplayStateMachine != null);
            
            _gameplayStateMachine.OnAfterChangeState += OnChangeState;
        }

        private void OnChangeState(IState<GameplayStates> state)
        {
            if (state is ResultGameplayState)
            {
                CubeData = new CubeData(_cubeData.Rate, _cubeData.Position, CubeState.Target);
            }
        }

        private void OnDisable()
        {
            _gameplayStateMachine.OnAfterChangeState -= OnChangeState;
        }

        public void Initialize(CubeData cubeData)
        {
            _colorGenerator ??= new ColorGenerator();

            UpdateData(cubeData);
        }
        
        public void UpdateData(CubeData cubeData)
        {
            CubeData = cubeData;
            
            ApplyVisuals();
        }
        
        private void ApplyVisuals()
        {
            var rate = CubeData.Rate;
            _rateText.ForEach(tmp => tmp.text = rate.ToString());
            _meshRenderer.material.color = _colorGenerator.GetRateColor(rate);
        }

        public void OnCubeMerged(CubeData cubeData)
        {
            UpdateData(cubeData);
            
            OnCubeMergedEvent?.Invoke(CubeData);
        }

        private async Task WaitUntil(Func<bool> condition, int checkDelayMs = 10)
        {
            while (!condition())
            {
                await Task.Delay(checkDelayMs);
            }
        }
    }
}