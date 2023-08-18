using System.Collections;
using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class GravityRadius : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] private float _speed;

        [Header("Component References:")]
        [SerializeField] private Transform _transform;

        private Coroutine _attractionRoutine;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out MainHero _))
            {
                _attractionRoutine = StartCoroutine(AttractionRoutine(collision.transform));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out MainHero _))
            {
                StopCoroutine(_attractionRoutine);
            }
        }

        private IEnumerator AttractionRoutine(Transform target)
        {
            while (true)
            {
                float speed = _speed * Time.fixedDeltaTime;
                Vector2 newPosition = Vector2.MoveTowards(
                    _transform.position,
                    target.position,
                    _speed * Time.fixedDeltaTime);
                _transform.position = newPosition;
                yield return null;
            }
        }
    }
}
