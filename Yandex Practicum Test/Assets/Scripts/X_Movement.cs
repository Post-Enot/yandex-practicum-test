using UnityEngine;

namespace YandexTest.Runner
{
    [DisallowMultipleComponent]
    public sealed class X_Movement : MonoBehaviour
    {
        [SerializeField] private float _xSpeed;


        private void Update()
        {
            float xDeltaMovement = _xSpeed * Time.deltaTime;
            transform.position = new Vector3(
                transform.position.x - xDeltaMovement,
                transform.position.y,
                transform.position.z);
        }
    }
}
