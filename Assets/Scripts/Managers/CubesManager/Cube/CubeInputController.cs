using Implementations.Game.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Managers.CubesManager
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeInputController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;

        [SerializeField] private Cube _cube;

        [SerializeField] private InputActionReference _pressAction;

        [SerializeField] private InputActionReference _positionAction;

        [SerializeField] private float _dragSensitivity = 0.01f;

        [SerializeField] private float _forwardImpulse = 10f;
        [SerializeField] private float _minX = -2f;
        [SerializeField] private float _maxX = 2f;

        [SerializeField] private LayerMask _cubeLayerMask;

        private Rigidbody _rigidbody;
        private Camera _mainCamera;

        private bool _isInitialized;
        private bool _isDragging;
        private Vector2 _lastPointerPos;

        private void OnEnable()
        {
            _cube.OnCubeStateChanged += OnCubeStateChanged;

            _rigidbody = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;

            _pressAction.action.Enable();
            _positionAction.action.Enable();

            _pressAction.action.started += OnPressStarted;
            _pressAction.action.canceled += OnPressCanceled;
        }

        private void OnDisable()
        {
            _isInitialized = false;

            _pressAction.action.started -= OnPressStarted;
            _pressAction.action.canceled -= OnPressCanceled;

            _cube.OnCubeStateChanged -= OnCubeStateChanged;
            
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.Sleep();
            }
        }

        private void OnCubeStateChanged(CubeState state) => _isInitialized = true;

        private void OnPressStarted(InputAction.CallbackContext ctx)
        {
            _lastPointerPos = _positionAction.action.ReadValue<Vector2>();

            var ray = _mainCamera.ScreenPointToRay(_lastPointerPos);

            if (!Physics.Raycast(ray, out var hit, 100f, _cubeLayerMask)) return;

            if (hit.collider == null || hit.collider.gameObject != gameObject) return;
            
            _isDragging = true;
        }

        private void OnPressCanceled(InputAction.CallbackContext ctx)
        {
            if (_cube.CubeState == CubeState.Target) return;
            if (!_isDragging) return;

            _isDragging = false;

            _rigidbody.WakeUp();
            _rigidbody.AddForce(Vector3.forward * _forwardImpulse, ForceMode.Impulse);

            _signalBus.AbstractFire<CubeCollidedSignal>();
        }

        private void Update()
        {
            if (!_isInitialized) return;
            if (_cube.CubeState == CubeState.Target) return;
            if (!_isDragging) return;

            var currentPos = _positionAction.action.ReadValue<Vector2>();
            var deltaX = (currentPos.x - _lastPointerPos.x) * _dragSensitivity;

            var potentialX = transform.position.x + deltaX;
            deltaX = Mathf.Clamp(potentialX, _minX, _maxX) - transform.position.x;

            transform.position += new Vector3(deltaX, 0f, 0f);

            _lastPointerPos = currentPos;
        }
    }
}