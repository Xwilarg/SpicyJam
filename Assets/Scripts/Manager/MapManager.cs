using System.Linq;
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

        public Transform GetRandomNode(System.Func<Transform, bool> predicate)
        {
            var possibles = _nodes.Where(predicate).ToArray();
            if (!possibles.Any())
            {
                return GetRandomNode();
            }
            return possibles[Random.Range(0, possibles.Length)];
        }
    }
}