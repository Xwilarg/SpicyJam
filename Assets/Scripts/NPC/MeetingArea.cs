using SpicyJam.Manager;
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

                foreach (var n in _npcs)
                {
                    NpcController target;

                    if (n.IsVampire) // We are a vampire, we bit someone
                    {
                        var targets = _npcs.Where(x => !x.WasBitten && !x.IsPriest && x.GameObject.GetInstanceID() != n.GameObject.GetInstanceID()).ToArray();

                        if (!targets.Any()) continue;

                        target = targets[Random.Range(0, targets.Length)];
                        target.WasBitten = true;
                        StoryManager.Instance.AttemptLoosingCondition();

                        if (!n.WasMolested) continue; // Vampire was not broken so it doesn't give hints
                    }
                    else if (n.WasMolested) // We are an innocent but was broken, so we tag random targets
                    {
                        var targets = _npcs.Where(x => x.MarkType != MarkType.VampireMark && !x.IsPriest && x.GameObject.GetInstanceID() != n.GameObject.GetInstanceID()).ToArray();

                        if (!targets.Any()) continue;

                        target = targets[Random.Range(0, targets.Length)];
                    }
                    else continue;

                    target.MarkType = MarkType.InnocentMark;
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