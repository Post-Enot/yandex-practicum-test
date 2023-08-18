using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class Despawner : MonoBehaviour
    {
        [SerializeField] private float _despawnPositionX;
        [SerializeField] private Transform _targetObject;
        [SerializeField] private GameObject _rootObject;

        private void Update()
        {
            if (_targetObject.position.x <= _despawnPositionX)
            {
                Destroy(_rootObject);
            }
        }
    }
}
