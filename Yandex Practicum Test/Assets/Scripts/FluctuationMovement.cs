using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class FluctuationMovement : MonoBehaviour
    {
        [Header("Params:")]
        [SerializeField] private float _amplitude = 1.0f;
        [SerializeField] private float _oscillationPeriodInSeconds = 1.0f;

        private float HalfOscillationPeriodInSeconds => _oscillationPeriodInSeconds / 2;

        private float _upAmplitude;
        private float _bottomAmplitude;
        private float _timer;

        private void Awake()
        {

            _upAmplitude = transform.position.y + _amplitude;
            _bottomAmplitude = transform.position.y - _amplitude;
            _timer = Random.Range(0, _oscillationPeriodInSeconds);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            float time = Mathf.PingPong(_timer, HalfOscillationPeriodInSeconds);
            time /= HalfOscillationPeriodInSeconds;
            float yPosition = Mathf.Lerp(_upAmplitude, _bottomAmplitude, time);
            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
        }
    }
}
