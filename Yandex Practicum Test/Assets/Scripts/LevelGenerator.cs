using System.Collections;
using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class LevelGenerator : MonoBehaviour
    {
        public enum SectionType : byte
        {
            None = 0,
            SafeSection = 1,
            LetSection = 2
        };
        
        [Header("Section Prefabs:")]
        [SerializeField] private GameObject _letSectionPrefab;
        [SerializeField] private GameObject _safeSectionPrefab;

        [Header("Entity Prefabs:")]
        [SerializeField] private GameObject _collectablePrefab;

        [Header("Params:")]
        [SerializeField] private Transform _levelObjectsRoot;
        [SerializeField] private Transform _sectionSpawnPoint;
        [SerializeField] private Transform _collectableSpawnPositionUpBorder;
        [SerializeField] private Transform _collectableSpawnPositionBottomBorder;
        [SerializeField][Range(0, 1)] private float _collectableAppearingChance;

        public int LetNumberInSection { get; set; }

        private float SectionSpawnPositionX => _sectionSpawnPoint.position.x;
        private float CollectableSpawnPositionUpBorder => _collectableSpawnPositionUpBorder.position.y;
        private float CollectableSpawnPositionBottomBorder => _collectableSpawnPositionBottomBorder.position.y;

        private Vector3 NextSectionSpawnPosition
        {
            get
            {
                if (_lastSectionType is SectionType.None)
                {
                    return _sectionSpawnPoint.position;
                }
                return new Vector3(
                _lastSection.SectionEndX,
                _sectionSpawnPoint.position.y,
                _sectionSpawnPoint.position.z);
            }
        }

        private ILevelSection _lastSection;
        private SectionType _lastSectionType;
        private Coroutine _generationRoutine;

        public void ClearLevel()
        {
            int i = 0;
            GameObject[] allChildren = new GameObject[_levelObjectsRoot.childCount];
            foreach (Transform child in _levelObjectsRoot)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                Destroy(child);
            }
            _lastSection = null;
            _lastSectionType = SectionType.None;
        }

        public void StartGeneration()
        {
            _generationRoutine = StartCoroutine(GenerationRoutine());
        }

        public void StopGeneration()
        {
            StopCoroutine(_generationRoutine);
        }

        private void SpawnSafeSection()
        {
            GameObject safeSectionObject = Instantiate(_safeSectionPrefab, _levelObjectsRoot);
            SafeSection safeSection = safeSectionObject.GetComponent<SafeSection>();
            safeSection.ConnectWithPreviousSection(NextSectionSpawnPosition);
            _lastSection = safeSection;
            _lastSectionType = SectionType.SafeSection;
        }

        private void SpawnLetSection()
        {
            GameObject safeSectionObject = Instantiate(_letSectionPrefab, _levelObjectsRoot);
            LetSection letSection = safeSectionObject.GetComponent<LetSection>();
            letSection.ConnectWithPreviousSection(NextSectionSpawnPosition);
            letSection.SpawnLet(LetNumberInSection);
            _lastSection = letSection;
            _lastSectionType = SectionType.LetSection;
        }

        private void SpawnCollectable()
        {
            if (Random.value <= _collectableAppearingChance)
            {
                _ = Instantiate(
                    _collectablePrefab,
                    GetRandomCollectablePosition(),
                    Quaternion.identity,
                    _levelObjectsRoot);
            }
        }

        private void SpawnNextSection()
        {
            switch (_lastSectionType)
            {
                case SectionType.None:
                case SectionType.SafeSection:
                    SpawnLetSection();
                    SpawnCollectable();
                    break;

                case SectionType.LetSection:
                    SpawnCollectable();
                    SpawnSafeSection();
                    break;
            }
        }

        private Vector3 GetRandomCollectablePosition()
        {
            return new Vector3(
                Random.Range(_lastSection.SectionStartX, _lastSection.SectionEndX),
                Random.Range(CollectableSpawnPositionBottomBorder, CollectableSpawnPositionUpBorder));
        }

        private IEnumerator GenerationRoutine()
        {
            SpawnNextSection();
            while (true)
            {
                if (_lastSection.SectionEndX <= SectionSpawnPositionX)
                {
                    SpawnNextSection();
                }
                yield return null;
            }
        }
    }
}
