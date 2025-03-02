using UnityEngine;

namespace SpicyJam.Manager
{
    public class NpcManager : MonoBehaviour
    {
        [SerializeField]
        private float _spawnAreaRadius;

        [SerializeField]
        private GameObject _npcPrefab;

        [SerializeField]
        private int _spawnCount;

        private void Awake()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                Instantiate(_npcPrefab, Random.insideUnitCircle * _spawnAreaRadius, Quaternion.identity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Vector3.zero, _spawnAreaRadius);
        }
    }
}