using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Managers
{
    public class AssetsManager : IAssetsManager
    {
        [Inject] private readonly DiContainer _diContainer;
        
        private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new();
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new();

        public async Task<T> LoadAsset<T>(string address) where T : Object
        {
            if (_loadedAssets.TryGetValue(address, out var cashedHandle))
            {
                return cashedHandle.Result as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedAssets[address] = handle;
                return handle.Result as T;
            }

            Debug.LogError($"Failed to load asset: {address}");
            
            return null;
        }

        public async Task<GameObject> Instantiate(
            string address, 
            Vector3 position, 
            Quaternion rotation,
            Transform parent = null, 
            bool instantiateInWorldSpace = false)
        {
            var handle = Addressables.InstantiateAsync(address, position, rotation, parent, instantiateInWorldSpace);
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.LogError($"Failed to create object: {address}");
            
            return null;
        }

        public async Task<GameObject> InstantiateWithDi(
            string address, 
            Vector3 position, 
            Quaternion rotation,
            Transform parent = null, 
            bool instantiateInWorldSpace = false)
        {
            var handle = Addressables.InstantiateAsync(address, position, rotation, parent, instantiateInWorldSpace);
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var instance = handle.Result;
                _diContainer.InjectGameObject(instance);
                
                return instance;
            }

            Debug.LogError($"Failed to create object with DI: {address}");
            
            return null;
        }

        public void ReleaseAsset(string address)
        {
            if (!_loadedAssets.TryGetValue(address, out var handle)) return;
            
            Addressables.Release(handle);
            _loadedAssets.Remove(address);
        }

        public void ReleaseInstance(GameObject instance)
        {
            if (instance != null) Addressables.ReleaseInstance(instance);
        }

        public void ReleaseAll()
        {
            foreach (var kvp in _loadedAssets) 
                Addressables.Release(kvp.Value);
            
            _loadedAssets.Clear();
            
            foreach (var kvp in _loadedScenes) 
                Addressables.UnloadSceneAsync(kvp.Value);
            
            _loadedScenes.Clear();
        }

        public async Task LoadScene(
            string address, 
            LoadSceneMode mode = LoadSceneMode.Additive,
            bool activateOnLoad = true)
        {
            if (_loadedScenes.ContainsKey(address))
            {
                Debug.LogWarning($"The scene is already loaded: {address}");
                
                return;
            }

            var handle = Addressables.LoadSceneAsync(address, mode, activateOnLoad);
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedScenes[address] = handle;
            }
            else
            {
                Debug.LogError($"Failed to load scene: {address}");
            }
        }

        public async Task UnloadScene(string address)
        {
            if (_loadedScenes.TryGetValue(address, out var handle))
            {
                var unloadOperationHandle = Addressables.UnloadSceneAsync(handle);
                await unloadOperationHandle.Task;
                
                _loadedScenes.Remove(address);
            }
        }

        public void Dispose() => ReleaseAll();
    }
}