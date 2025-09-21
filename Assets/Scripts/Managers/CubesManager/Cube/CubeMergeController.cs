using DG.Tweening;
using UnityEngine;

namespace Managers.CubesManager
{
    [RequireComponent(typeof(Cube))]
    public class CubeMergeController : MonoBehaviour
    {
        [SerializeField] private Cube _cube;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _jumpForce = 5.0f;
        [SerializeField] private float _scaleFactor = 1.3f;

        private Tween _tween;

        public bool TryMerge(CubeData cubeData)
        {
            if (_cube.CubeData.Rate != cubeData.Rate) return false;

            DoScale();
            Jump();
            
            var newCubeData = new CubeData(
                _cube.CubeData.Rate * 2,
                _cube.CubeData.Position,
                CubeState.Target
            );
            
            _cube.OnCubeMerged(newCubeData);

            return true;
        }
    
        private void Jump()
        {
            if (_rigidbody == null) return;

            var velocity = _rigidbody.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            _rigidbody.velocity = velocity;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        private void DoScale()
        {
            if (_cube == null) return;

            if (_tween.IsActive()) _tween.Kill();

            var cubeTransform = _cube.transform;

            _tween = cubeTransform
                .DOScale(_scaleFactor, 0.1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    cubeTransform.DOScale(1f, 0.1f).SetEase(Ease.InQuad);
                });
        }
    }
}