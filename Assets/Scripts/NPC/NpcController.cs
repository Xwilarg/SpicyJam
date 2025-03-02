using SpicyJam.Interaction;
using SpicyJam.Manager;
using SpicyJam.Player;
using System.Collections.Generic;
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

        private string[] _eyesColors = new[] { "blue", "yellow", "brown" };
        private string[] _hairColors = new[] { "blond", "light brown", "dark brown", "red" };
        private string[] _gender = new[] { "female", "male" };
        private string[] _age = new[] { "young", "old" };
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
            _baseColor = _sr.color;
            _attrs.Add("eyesColor", _eyesColors[Random.Range(0, _eyesColors.Length)]);
            _attrs.Add("hairColor", _hairColors[Random.Range(0, _hairColors.Length)]);
            _attrs.Add("gender", _gender[Random.Range(0, _gender.Length)]);
            _attrs.Add("age", _age[Random.Range(0, _age.Length)]);
        }

        private void Start()
        {
            _target = MapManager.Instance.GetRandomNode();
        }

        public string GetDescription()
        {
            return $"This person seem to be a {_attrs["age"]} {_attrs["gender"]} with {_attrs["eyesColor"]} eyes and {_attrs["hairColor"]} hair";
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
