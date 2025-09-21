using System.Threading.Tasks;
using UnityEngine;

namespace Factories
{
    public interface IObjectFactory<T> where T : Component
    {
        Task<T> Spawn(string assetId, Vector3 position, Quaternion rotation, Transform parent = null);
        void Hide(T obj);
        void ClearPool();
    }
}