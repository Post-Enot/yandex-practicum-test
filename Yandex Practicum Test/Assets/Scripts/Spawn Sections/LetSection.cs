using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class LetSection : MonoBehaviour, ILevelSection
    {
        [Header("Prefabs:")]
        [SerializeField] private GameObject _letPrefab;

        [Header("Section Borders:")]
        [SerializeField] private Transform _sectionStart;
        [SerializeField] private Transform _sectionEnd;

        [Header("Let spawn borders:")]
        [SerializeField] private Transform _letSpawnLeftDownCorner;
        [SerializeField] private Transform _letSpawnRightUpCorner;

        public float SectionStartX => _sectionStart.position.x;
        public float SectionEndX => _sectionEnd.position.x;
        public float SectionPositionX => transform.position.x;

        private Vector2 LetSpawnLeftDownCorner => _letSpawnLeftDownCorner.transform.position;
        private Vector2 LetSpawnRightUpCorner => _letSpawnRightUpCorner.transform.position;

        public void SpawnLet(int letCount)
        {
            for (int i = 0; i < letCount; i += 1)
            {
                Vector3 position = GetRandomLetPosition();
                _ = Instantiate(_letPrefab, position, Quaternion.identity, transform);
            }
        }

        public void ConnectWithPreviousSection(Vector3 previousSectionEndPosition)
        {
            float deltaX = transform.position.x - SectionStartX;
            transform.position = new Vector3(
                previousSectionEndPosition.x + deltaX,
                previousSectionEndPosition.y,
                previousSectionEndPosition.z);
        }

        private Vector3 GetRandomLetPosition()
        {
            return new Vector2(
                Random.Range(LetSpawnLeftDownCorner.x, LetSpawnRightUpCorner.x),
                Random.Range(LetSpawnLeftDownCorner.y, LetSpawnRightUpCorner.y));
        }
    }
}
