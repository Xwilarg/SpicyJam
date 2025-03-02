using SpicyJam.Interaction;
using SpicyJam.Manager;
using SpicyJam.Player;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpicyJam.NPC
{
    public class NpcController : MonoBehaviour, IInteractible
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private TriggerArea _triggerArea;

        private Transform _target;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private CircleCollider2D _coll;
        private Color _baseColor;

        public bool CanInteract => true;
        public GameObject GameObject => gameObject;

        public bool IsVampire { set; get; }
        public bool IsPriest { set; get; }

        private readonly string[] _eyesColors = new[] { "blue", "yellow", "brown" };
        private readonly string[] _hairColors = new[] { "blond", "light brown", "dark brown", "red" };
        private readonly string[] _gender = new[] { "female", "male" };
        private readonly string[] _age = new[] { "young", "old" };

        private readonly string[] _pronouns = new[] { "her", "his" };
        private readonly Dictionary<string, string> _attrs = new();

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
            _coll = GetComponent<CircleCollider2D>();

            _baseColor = _sr.color;
            _attrs.Add("eyesColor", _eyesColors[Random.Range(0, _eyesColors.Length)]);
            _attrs.Add("hairColor", _hairColors[Random.Range(0, _hairColors.Length)]);
            var gender = Random.Range(0, _gender.Length);
            _attrs.Add("gender", _gender[gender]);
            _attrs.Add("pronouns", _pronouns[gender]);
            _attrs.Add("age", _age[Random.Range(0, _age.Length)]);
        }

        private void Start()
        {
            _target = MapManager.Instance.GetRandomNode();

            if (IsVampire)
            {
                _triggerArea.OnTriggerEnter.AddListener(c =>
                {
                    if (c.TryGetComponent<NpcController>(out var npcC) && npcC.IsPriest)
                    {
                        RedirectOppositeDirection();
                    }
                });
            }
        }

        public void RedirectOppositeDirection()
        {
            _target = MapManager.Instance.GetRandomNode(x => Vector2.Dot(_rb.linearVelocity.normalized, (x.position - transform.position)) < 0f);
        }

        public string GetDescription()
        {
            StringBuilder str = new();
            str.AppendLine($"This person seem to be a {_attrs["age"]} {_attrs["gender"]} with {_attrs["eyesColor"]} eyes and {_attrs["hairColor"]} hair");
            if (IsPriest)
            {
                str.AppendLine($"From {_attrs["gender"]} outfits, you can guess this person is a priest\n(Vampires will always being close to a priest)");
            }
            return str.ToString();
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

        private void OnDrawGizmos()
        {
            if (IsVampire) Gizmos.color = Color.red;
            else if (IsPriest) Gizmos.color = new Color(1f, 0.078f, 0.576f);
            else return;

            Gizmos.DrawSphere(transform.position, _coll.radius);

            if (IsPriest)
            {
                Gizmos.DrawWireSphere(transform.position, _triggerArea.GetComponent<CircleCollider2D>().radius);
            }
        }
    }
}
