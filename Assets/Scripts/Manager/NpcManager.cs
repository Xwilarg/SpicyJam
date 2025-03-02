using SpicyJam.NPC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpicyJam.Manager
{
    public class NpcManager : MonoBehaviour
    {
        [SerializeField]
        private float _spawnAreaRadius;

        [SerializeField]
        private GameObject _npcPrefab;

        [SerializeField]
        private int _spawnCount, _vampireCount;

        private List<NpcController> _npcs = new();

        private void Awake()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                var go = Instantiate(_npcPrefab, Random.insideUnitCircle * _spawnAreaRadius, Quaternion.identity);
                _npcs.Add(go.GetComponent<NpcController>());
            }

            Assert.IsTrue(_vampireCount >= _spawnCount);
            List<int> ids = new();
            while (ids.Count < _vampireCount)
            {
                var id = Random.Range(0, _npcs.Count);
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                    _npcs[id].IsVampire = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Vector3.zero, _spawnAreaRadius);
        }
    }
}