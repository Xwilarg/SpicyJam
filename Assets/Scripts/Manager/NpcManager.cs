using SpicyJam.NPC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpicyJam.Manager
{
    public class NpcManager : MonoBehaviour
    {
        public static NpcManager Instance { private set; get; }

        [SerializeField]
        private float _spawnAreaRadius;

        [SerializeField]
        private GameObject _npcPrefab;

        [SerializeField]
        private int _spawnCount, _vampireCount, _priestCount, _startBittenCount;

        [SerializeField]
        private GameObject _meetingPrefab;

        private float _meetingAreaTimer;
        private float MeetingAreaTimerRef = 2f;

        private List<NpcController> _npcs = new();

        public int VampireLefts => _npcs.Count(x => x.IsVampire);

        private void Awake()
        {
            Instance = this;

            for (int i = 0; i < _spawnCount; i++)
            {
                var go = Instantiate(_npcPrefab, Random.insideUnitCircle * _spawnAreaRadius, Quaternion.identity);
                _npcs.Add(go.GetComponent<NpcController>());
            }

            Assert.IsTrue(_vampireCount + _priestCount <= _spawnCount);
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
            while (ids.Count < _vampireCount + _priestCount)
            {
                var id = Random.Range(0, _npcs.Count);
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                    _npcs[id].IsPriest = true;
                }
            }
            while (ids.Count < _vampireCount + _priestCount + _startBittenCount) // We bit some random innocents
            {
                var id = Random.Range(0, _npcs.Count);
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                    _npcs[id].WasBitten = true;
                }
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.CanPlay) return;

            _meetingAreaTimer -= Time.deltaTime;
            if (_meetingAreaTimer <= 0f)
            {
                _meetingAreaTimer = MeetingAreaTimerRef;
                Instantiate(_meetingPrefab, Random.insideUnitCircle * _spawnAreaRadius, Quaternion.identity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Vector3.zero, _spawnAreaRadius);
        }

        public void Kill(NpcController npc)
        {
            _npcs.Remove(npc);
            Destroy(npc.gameObject);
        }

        public bool IsThereVampireLeft
            => _npcs.Any(x => x.IsVampire);
    }
}