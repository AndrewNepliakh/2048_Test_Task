using System;
using Zenject;
using UnityEngine;
using Managers.CubesManager;
using System.Threading.Tasks;
using Implementations.Game.Signals;

namespace Controllers
{
    public class AimGameplayStateController : IGameplayStateController
    {
        [Inject] private ICubesManager _cubesManager;
        [Inject] private SignalBus _signalBus;

        public event Action OnStateComplete;

        public async Task Init()
        {
            await InitCubes();
            _signalBus.Subscribe<CubeCollidedSignal>(HandleCubeCollision);
        }
    
        private async Task InitCubes()
        {
            CubeData[] cubeDatas =
            {
                new(new Vector3(0.0f, 0.6f, 0.0f), CubeState.Puck)
            };
            
            foreach (var cubeData in cubeDatas)
                await _cubesManager.SpawnCube(cubeData);
        }

        private void HandleCubeCollision()
        {
            _signalBus.Unsubscribe<CubeCollidedSignal>(HandleCubeCollision);
            OnStateComplete?.Invoke();
        }
    }
}