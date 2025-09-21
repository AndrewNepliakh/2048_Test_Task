using System;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Managers
{
    public class LevelManager : ILevelManager
    {
        [Inject] private IAssetsManager _assetsManager;

        private Level _currentLevel;
        
        private readonly Dictionary<Type, Level> _levelsPool = new();

        public Level CurrentLevel => _currentLevel;
        
        public async Task InstantiateLevel<T>(LevelsArguments levelArgs) where T : Level
        {
           
            if (_currentLevel != null) _currentLevel.gameObject.SetActive(false);
            
            if (_levelsPool.TryGetValue(typeof(T), out var pooledLevel))
            {
                _currentLevel = pooledLevel;
                _currentLevel.gameObject.SetActive(true);
            }
            else
            {
                
                var levelGo = await _assetsManager.InstantiateWithDi(
                    typeof(T).ToString(),
                    position: Vector3.zero,
                    rotation: Quaternion.identity,
                    parent: null);

                _currentLevel = levelGo.GetComponent<T>();
                
                _levelsPool[typeof(T)] = _currentLevel;
            }
            
            _currentLevel.Init(levelArgs);
        }
        
        public void ClearAllLevels()
        {
            foreach (var kvp in _levelsPool)
            {
                var level = kvp.Value;
                if (level != null)
                {
                    level.gameObject.SetActive(false);
                    _assetsManager.ReleaseInstance(level.gameObject);
                }
            }

            _levelsPool.Clear();
            _currentLevel = null;
        }
        
        public void Dispose()
        {
            ClearAllLevels();
        }
    }
}
