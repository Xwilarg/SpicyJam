using System.Collections.Generic;
using UnityEngine;

namespace SpicyJam.NPC
{
    public class MeetingArea : MonoBehaviour
    {
        private float _destroyTimer;

        private readonly List<NpcController> _npcs = new();

        private void Awake()
        {
            _destroyTimer = Random.Range(1f, 3f);
        }

        private void Update()
        {
            _destroyTimer -= Time.deltaTime;
            if (_destroyTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
        }

        public void Register(NpcController npc)
        {
            _npcs.Add(npc);
        }
    }
}