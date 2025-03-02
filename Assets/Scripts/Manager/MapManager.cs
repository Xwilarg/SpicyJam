using UnityEngine;

namespace SpicyJam.Manager
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { private set; get; }

        [SerializeField]
        private Transform[] _nodes;

        private void Awake()
        {
            Instance = this;
        }

        public Transform GetRandomNode()
            => _nodes[Random.Range(0, _nodes.Length)];
    }
}