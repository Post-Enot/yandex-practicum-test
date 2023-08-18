using System.Collections;
using UnityEngine;
using YandexTest.Runner.Input;

using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace YandexTest.Runner
{
    [DisallowMultipleComponent]
    public sealed class MainHeroController : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] private float _speed;

        [Header("Component References:")]
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private InputActions _inputActions;
        private Coroutine _addForceRoutine;

        private void Awake()
        {
            _inputActions = new InputActions();
            SubscribeOnInputEvents();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void OnDestroy()
        {
            UnsubscribeFromInputEvents();
        }

        private void SubscribeOnInputEvents()
        {
            _inputActions.Gameplay.AddForce.started += AddForce_started;
            _inputActions.Gameplay.AddForce.canceled += AddForce_canceled;
        }

        private void UnsubscribeFromInputEvents()
        {
            _inputActions.Gameplay.AddForce.started += AddForce_started;
            _inputActions.Gameplay.AddForce.canceled += AddForce_canceled;
        }

        private void AddForce()
        {
            Vector2 force = new(0, _speed);
            _rigidbody2D.AddForce(force);
        }

        private void AddForce_started(InputContext context)
        {
            _addForceRoutine = StartCoroutine(AddForceRoutine());
        }

        private void AddForce_canceled(InputContext context)
        {
            StopCoroutine(_addForceRoutine);
        }

        private IEnumerator AddForceRoutine()
        {
            while (true)
            {
                AddForce();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
