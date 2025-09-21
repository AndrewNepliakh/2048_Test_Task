using System;
using Zenject;
using Factories;
using UnityEngine;
using Managers.SaveManager;
using Implementations.Game;
using Managers.UserManager;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Managers.CubesManager
{
    public class CubesManager : ICubesManager
    {
        [Inject] private ILevelManager _levelManager;
        [Inject] private IObjectFactory<Cube> _cubeFactory;
        [Inject] private ISaveManager _saveManager;
        [Inject] private IUserManager _userManager;

        private readonly List<Cube> _activeCubes = new();
        
        public int ActiveCubesCount => _activeCubes.Count;

        public event Action OnCubeMergedEvent;

        public async Task<Cube> SpawnCube(CubeData cubeData)
        {
            var cube = await _cubeFactory.Spawn(
                assetId: Constants.CubeAssetId,
                position: cubeData.Position,
                rotation: Quaternion.identity,
                parent: _levelManager.CurrentLevel?.transform);

            cube.Initialize(cubeData);
            _activeCubes.Add(cube);

            cube.OnCubeMergedEvent += OnCubeMerged;

            return cube;
        }

        public void HideCube(Cube cube)
        {
            cube.OnCubeMergedEvent -= OnCubeMerged;
            
            _activeCubes.Remove(cube);
            _cubeFactory.Hide(cube);
        }

        public void ClearAllCubes()
        {
            foreach (var cube in _activeCubes)
            {
                cube.OnCubeMergedEvent -= OnCubeMerged;
                _cubeFactory.Hide(cube);
            }

            _activeCubes.Clear();
            _cubeFactory.ClearPool();
        }

        public void Dispose() => ClearAllCubes();

        private void OnCubeMerged(CubeData cubeData)
        {
            _userManager.CurrentUser.GameData.SetCurrentMergeValue(cubeData.Rate);
            _saveManager.Save();
            
            OnCubeMergedEvent?.Invoke();
        }
    }
}