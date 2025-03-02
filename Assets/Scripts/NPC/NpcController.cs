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
        private SpriteRenderer _sr;
        private Color _baseColor;

        public bool CanInteract => true;
        public GameObject GameObject => gameObject;

        public bool IsVampire { set; get; }

        public void Interact(PlayerController pc)
        {
            StoryManager.Instance.ShowDescription(this);
        }

        public void ToggleHighlight(bool value)
        {
            _sr.color = value ? Color.green : _baseColor;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _baseColor = _sr.color;
        }

        private void Start()
        {
            _target = MapManager.Instance.GetRandomNode();
        }

        private void Update()
        {
            if (!GameManager.Instance.CanPlay)
            {
                _rb.linearVelocity = Vector2.zero;
            }
            else if (Vector2.Distance(_target.position, transform.position) < .1f)
            {
                _target = MapManager.Instance.GetRandomNode();
                _rb.linearVelocity = Vector2.zero;
            }
            else
            {
                _rb.linearVelocity = (_target.position - transform.position).normalized * _speed;
            }
        }
    }
}
