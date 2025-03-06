using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpicyJam.NPC
{
    public class MeetingArea : MonoBehaviour
    {
        private float _destroyTimer;

        private readonly List<NpcController> _npcs = new();

        public bool DoesContainsPriest => _npcs.Any(x => x.IsPriest);

        private void Awake()
        {
            _destroyTimer = Random.Range(1f, 3f);
        }

        private void Update()
        {
            _destroyTimer -= Time.deltaTime;
            if (_destroyTimer <= 0f)
            {
                if (_npcs.Any(x => x.IsVampire))
                {
                    var possibleTargets = _npcs.Where(x => !x.WasBitten).ToArray();
                    if (possibleTargets.Any())
                    {
                        possibleTargets[Random.Range(0, possibleTargets.Length)].WasBitten = true;
                    }
                }
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

            if (npc.IsPriest) // Priest joined the meeting, all vampires must flee
            {
                foreach (var vamp in _npcs.Where(x => x.IsVampire))
                {
                    vamp.FleeFromMeeting();
                }
                _npcs.RemoveAll(x => x.IsVampire);
            }
        }
    }
}