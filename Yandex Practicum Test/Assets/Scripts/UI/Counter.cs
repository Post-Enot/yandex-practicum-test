using TMPro;
using UnityEngine;

namespace YandexTest.Runner
{
    public sealed class Counter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        public void UpdateCounter(int count)
        {
            _counter.text = count.ToString();
        }
    }
}
