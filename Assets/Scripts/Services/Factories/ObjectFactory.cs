using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Factories
{
    public class ObjectFactory<T> : IObjectFactory<T> where T : Component
    {
        [Inject] private IAssetsManager _assetsManager;

        private readonly Queue<T> _pool = new();
        
        public async Task<T> Spawn(string assetId, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T obj;

            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
            }
            else
            {
                var objGo = await _assetsManager.InstantiateWithDi(
                    assetId,
                    position,
                    rotation,
                    parent);

                obj = objGo.GetComponent<T>();
            }

            return obj;
        }
        
        public void Hide(T obj)
        {
            if (obj == null) return;

            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
        
        public void ClearPool()
        {
            while (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                if (obj != null)
                    _assetsManager.ReleaseInstance(obj.gameObject);
            }
        }
    }
}
