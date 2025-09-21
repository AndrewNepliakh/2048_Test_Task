using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Managers
{
    public interface IAssetsManager : IDisposable
    {
        Task<T> LoadAsset<T>(string address) where T : UnityEngine.Object;

        Task<GameObject> Instantiate(
            string address,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            bool instantiateInWorldSpace = false);

        Task<GameObject> InstantiateWithDi(
            string address,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            bool instantiateInWorldSpace = false);

        void ReleaseAsset(string address);
        void ReleaseInstance(GameObject instance);
        void ReleaseAll();
        
        Task LoadScene(
            string address,
            LoadSceneMode mode = LoadSceneMode.Additive,
            bool activateOnLoad = true);

        Task UnloadScene(string address);
    }
}