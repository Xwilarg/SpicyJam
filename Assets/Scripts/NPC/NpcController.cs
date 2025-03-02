using SpicyJam.Interaction;
using SpicyJam.Manager;
using SpicyJam.Player;
using UnityEngine;

namespace SpicyJam.NPC
{
    public class NpcController : MonoBehaviour, IInteractible
    {
        [SerializeField]
        private float _speed;

        private Transform _target;
        private Rigidbody2D _rb;

        public int ID => gameObject.GetInstanceID();

        public bool CanInteract => true;

        public void Interact(PlayerController pc)
        {
            throw new System.NotImplementedException();
        }

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
