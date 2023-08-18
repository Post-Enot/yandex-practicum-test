using System;
using UnityEngine;

namespace YandexTest.Runner
{
    [DisallowMultipleComponent]
    public sealed class MainHero : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private FakeTrailRenderer _fakeTrailRenderer;

        public event Action CollectableCollected;
        public event Action Crashed;

        public void StartMovement()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.WakeUp();
            _fakeTrailRenderer.EnableRenderer();
        }

        public void StopMovement()
        {
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _rigidbody.Sleep();
            _fakeTrailRenderer.DisableRenderer();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Collectable collectable))
            {
                Destroy(collectable.gameObject);
                CollectableCollected?.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Crashed?.Invoke();
        }
    }
}
