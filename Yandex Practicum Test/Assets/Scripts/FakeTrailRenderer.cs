using System.Collections;
using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class FakeTrailRenderer : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] private int _pointsCount;
        [SerializeField] private float _speed;

        [Header("Component References:")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _target;

        private Coroutine _coroutine;

        public void EnableRenderer()
        {
            ResetPositions();
            _coroutine = StartCoroutine(LineRenderingRoutine());
        }

        public void DisableRenderer()
        {
            ResetPositions();
            StopCoroutine(_coroutine);
        }

        public void ResetPositions()
        {
            _lineRenderer.positionCount = _pointsCount;
            for (int i = 0; i < _lineRenderer.positionCount; i += 1)
            {
                _lineRenderer.SetPosition(i, _target.position);
            }
        }

        private IEnumerator LineRenderingRoutine()
        {
            while (true)
            {
                for (int i = _lineRenderer.positionCount - 2; i >= 0; i -= 1)
                {
                    Vector3 position = _lineRenderer.GetPosition(i);
                    position.x -= _speed;
                    _lineRenderer.SetPosition(i + 1, position);
                }
                _lineRenderer.SetPosition(0, _target.position);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
