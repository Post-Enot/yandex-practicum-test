using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class SafeSection : MonoBehaviour, ILevelSection
    {
        [Header("Params:")]
        [SerializeField] private Transform _sectionStart;
        [SerializeField] private Transform _sectionEnd;

        public float SectionStartX => _sectionStart.position.x;
        public float SectionEndX => _sectionEnd.position.x;
        public float SectionPositionX => transform.position.x;

        public void ConnectWithPreviousSection(Vector3 previousSectionEndPosition)
        {
            float deltaX = transform.position.x - SectionStartX;
            transform.position = new Vector3(
                previousSectionEndPosition.x + deltaX,
                previousSectionEndPosition.y,
                previousSectionEndPosition.z);
        }
    }
}
