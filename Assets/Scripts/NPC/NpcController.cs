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

        [SerializeField]
        private GameObject _vampireMark, _innocentMark, _brokenMark;

        private Transform _target;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private CircleCollider2D _coll;
        private Color _baseColor;

        public bool CanInteract => true;
        public GameObject GameObject => gameObject;

        private bool _isVampire;
        public bool IsVampire
        {
            set
            {
                _isVampire = value;
                if (value)
                {
                    WasBitten = true;
                }
            }
            get => _isVampire;
        }
        public bool IsPriest { set; get; }
        private bool _wasMolested;
        public bool WasMolested
        {
            set
            {
                _wasMolested = value;
                _brokenMark.SetActive(_wasMolested);
            }
            get => _wasMolested;
        }
        public bool WasBitten { set; get; }

        private readonly string[] _eyesColors = new[] { "blue", "yellow", "brown" };
        private readonly string[] _hairColors = new[] { "blond", "light brown", "dark brown", "red" };
        private readonly string[] _gender = new[] { "female", "male" };
        private readonly string[] _age = new[] { "young", "old" };

        private readonly string[] _pronouns = new[] { "her", "his" };
        private readonly Dictionary<string, string> _attrs = new();

        private float _redirectTimer;
        private const float RedirectTimerRef = .5f;

        private GameObject _meetingArea;

        private MarkType _markType;
        public MarkType MarkType
        {
            set
            {
                _markType = value;
                _vampireMark.SetActive(false);
                _innocentMark.SetActive(false);

                if (_markType == MarkType.InnocentMark) _innocentMark.SetActive(true);
                else if (_markType == MarkType.VampireMark) _vampireMark.SetActive(true);
            }
            get => _markType;
        }

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

            _redirectTimer = RedirectTimerRef;
        }

        private void Start()
        {
            _target = MapManager.Instance.GetRandomNode();

            if (IsVampire)
            {
                _triggerArea.OnTriggerEnter.AddListener(c =>
                {
                    if (c.TryGetComponent<NpcController>(out var npcC) && npcC.IsPriest && Random.Range(0, 3) < 2)
                    {
                        RedirectOppositeDirection();
                    }
                });
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("MeetingArea"))
            {
                if (IsVampire && collision.gameObject.GetComponent<MeetingArea>().DoesContainsPriest)
                {
                    // There is a priest at this meeting so we don't stop!
                    return;
                }
                _meetingArea = collision.gameObject;
                _meetingArea.GetComponent<MeetingArea>().Register(this);
            }
        }

        /// <summary>
        /// Called from vampires when a priest join a meeting they are in
        /// </summary>
        public void FleeFromMeeting()
        {
            _meetingArea = null;
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
                str.AppendLine($"From {_attrs["pronouns"]} outfits, you can guess this person is a priest");
            }
            else if (WasMolested)
            {
                str.AppendLine($"This person was broken by you and is now helping you");
            }
            if (WasBitten)
            {
                str.AppendLine($"From the mark on {_attrs["pronouns"]} neck, you can guess this person was bitten");
            }
            if (MarkType == MarkType.VampireMark)
            {
                str.AppendLine("You marked this target as a potential vampire");
            }
            else if (MarkType == MarkType.InnocentMark)
            {
                str.AppendLine("You marked this target as a potential innocent");
            }
            return str.ToString();
        }

        private void Update()
        {
            if (!GameManager.Instance.CanPlay || _meetingArea != null)
            {
                _rb.linearVelocity = Vector2.zero;
            }
            else if (Vector2.Distance(_target.position, transform.position) < .1f)
            {
                _target = MapManager.Instance.GetRandomNode();
                _rb.linearVelocity = Vector2.zero;
                _redirectTimer = RedirectTimerRef;
            }
            else
            {
                _rb.linearVelocity = (_target.position - transform.position).normalized * _speed;
            }

            if (GameManager.Instance.CanPlay && !IsVampire && !IsPriest)
            {
                _redirectTimer -= Time.deltaTime;
                if (_redirectTimer <= 0f)
                {
                    _redirectTimer = RedirectTimerRef;
                    if (Random.Range(0, 100) == 0)
                    {
                        _target = MapManager.Instance.GetRandomNode();
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (IsVampire)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, _coll.radius);
            }
            else if (IsPriest)
            {
                Gizmos.color = new Color(1f, 0.078f, 0.576f);
                Gizmos.DrawSphere(transform.position, _coll.radius);
                Gizmos.DrawWireSphere(transform.position, _triggerArea.GetComponent<CircleCollider2D>().radius);
            }

            if (WasMolested)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position, _coll.radius);
            }
            if (WasBitten)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _coll.radius);
            }
        }
    }

    public enum MarkType
    {
        None,
        VampireMark,
        InnocentMark
    }
}
