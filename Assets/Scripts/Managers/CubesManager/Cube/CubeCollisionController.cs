using Zenject;
using UnityEngine;

namespace Managers.CubesManager
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeCollisionController : MonoBehaviour
    {
        [Inject] private ICubesManager _cubesManager;

        [SerializeField] private float minImpactForce = 2f;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Cube _cube;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Cube") ||
                _rigidbody.velocity.sqrMagnitude < minImpactForce * minImpactForce)
                return;

            if (!collision.gameObject.TryGetComponent(out Cube otherCube))
                return;

            if (otherCube.CubeData.CubeState == CubeState.Puck)
                return;

            if (!collision.gameObject.TryGetComponent(out CubeMergeController targetCubeController))
                return;

            if (targetCubeController.TryMerge(_cube.CubeData))
            {
                _cubesManager.HideCube(_cube);
            }
        }
    }
}