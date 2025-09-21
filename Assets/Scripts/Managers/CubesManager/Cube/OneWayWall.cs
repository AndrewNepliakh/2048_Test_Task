using UnityEngine;

namespace Managers.CubesManager
{
    [RequireComponent(typeof(Collider))]
    public class OneWayWall : MonoBehaviour
    {
        [SerializeField] private Vector3 allowedDirection = Vector3.forward;
        [SerializeField] private Collider _collider;

        private void Awake()
        {
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherAttachedRigidbody = other.attachedRigidbody;
            
            var dot = Vector3.Dot(otherAttachedRigidbody.velocity.normalized, allowedDirection.normalized);

            if (dot <= 0) _collider.isTrigger = false;
        }

        private void OnTriggerExit(Collider other)
        {
            _collider.isTrigger = true;
        }
    }
}