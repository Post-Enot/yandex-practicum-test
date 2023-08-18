using UnityEngine;

namespace YandexTest.Runner
{
    public interface ILevelSection
    {
        public float SectionStartX { get; }
        public float SectionEndX { get; }
        public float SectionPositionX { get; }

        public void ConnectWithPreviousSection(Vector3 previousSectionEndPosition);
    }
}
