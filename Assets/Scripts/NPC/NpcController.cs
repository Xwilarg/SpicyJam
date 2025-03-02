using SpicyJam.Manager;
using UnityEngine;

namespace SpicyJam.NPC
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        private Transform _target;
        private Rigidbody2D _rb;

        private void Start()
        {
            _target = MapManager.Instance.GetRandomNode();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Vector2.Distance(_target.position, transform.position) < .1f)
            {
                _target = MapManager.Instance.GetRandomNode();
            }
            else
            {
                _rb.linearVelocity = (_target.position - transform.position).normalized * _speed;
            }
        }
    }
}
