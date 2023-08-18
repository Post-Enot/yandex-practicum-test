using System;
using UnityEngine;
using UnityEngine.Events;
using YandexTest.Runner.Input;

using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace YandexTest.Runner
{
    public sealed class RunSession : MonoBehaviour
    {
        [Header("Component References:")]
        [SerializeField] private MainHero _mainHero;
        [SerializeField] private LevelGenerator _levelGenerator;

        [Header("Params:")]
        [SerializeField] private Transform _mainHeroSpawnPoint;

        [Header("Events:")]
        [SerializeField] private UnityEvent<int> _collectablesCountChanged;
        [SerializeField] private UnityEvent _runStarted;
        [SerializeField] private UnityEvent _runCompleted;

        public event Action<int> CollectablesCountChanged;
        public event Action RunStarted;
        public event Action RunCompleted;

        private Vector3 MainHeroSpawnPoint => _mainHeroSpawnPoint.position;
        private int _collectablesCount;

        private InputActions _inputActions;

        private void Awake()
        {
            _inputActions = new InputActions();
            _mainHero.transform.position = MainHeroSpawnPoint;
            SubscribeOnStartGameInputEvents();
            SubscribeOnMainHeroEvents();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void SubscribeOnStartGameInputEvents()
        {
            _inputActions.Gameplay.AddForce.started += HandleStartGameInputEvent;
        }

        private void SubscribeOnMainHeroEvents()
        {
            _mainHero.CollectableCollected += HandleMainHeroCollectableCollectedEvent;
            _mainHero.Crashed += HandleMainHeroCrashedEvent;
        }

        private void SetCollectablesCount(int collectablesCount)
        {
            _collectablesCount = collectablesCount;
            _levelGenerator.LetNumberInSection = _collectablesCount + 1;
            InvokeCollectablesCountChanged(_collectablesCount);
        }

        private void HandleMainHeroCrashedEvent()
        {
            CompleteRun();
        }

        private void HandleMainHeroCollectableCollectedEvent()
        {
            SetCollectablesCount(_collectablesCount + 1);
        }

        private void UnsubscribeFromStartGameInputEvents()
        {
            _inputActions.Gameplay.AddForce.started -= HandleStartGameInputEvent;
        }

        private void StartRun()
        {
            _mainHero.StartMovement();
            _levelGenerator.StartGeneration();
            InvokeRunStartedEvent();
            SetCollectablesCount(0);
        }

        private void CompleteRun()
        {
            _levelGenerator.StopGeneration();
            _levelGenerator.ClearLevel();
            _mainHero.StopMovement();
            SetCollectablesCount(0);
            InvokeRunCompletedEvent();
            _mainHero.transform.position = MainHeroSpawnPoint;
            SubscribeOnStartGameInputEvents();
        }

        private void InvokeCollectablesCountChanged(int collectablesCount)
        {
            _collectablesCountChanged?.Invoke(collectablesCount);
            CollectablesCountChanged?.Invoke(collectablesCount);
        }

        private void HandleStartGameInputEvent(InputContext context)
        {
            UnsubscribeFromStartGameInputEvents();
            StartRun();
        }

        private void InvokeRunStartedEvent()
        {
            _runStarted?.Invoke();
            RunStarted?.Invoke();
        }

        private void InvokeRunCompletedEvent()
        {
            _runCompleted?.Invoke();
            RunCompleted?.Invoke();
        }
    }
}
