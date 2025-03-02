using InflationPotion.SO;
using SpicyJam.Interaction;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpicyJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("Player feet, used as player center point")]
        private Transform _feet;

        [SerializeField]
        private PlayerInfo _pInfo;

        [SerializeField]
        private TriggerArea _triggerArea;

        private Vector2 _mov;

        private Camera _cam;
        private Rigidbody2D _rb;
        private IInteractible _interactionTarget; // TODO: Store as list

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            //_anim = GetComponentInChildren<Animator>();

            _cam = Camera.main;

            _triggerArea.OnTriggerEvent.AddListener((coll) =>
            {
                if (coll.TryGetComponent<IInteractible>(out var i))
                {
                    _interactionTarget = i;
                }
            });
            _triggerArea.OnTriggerExit.AddListener((coll) =>
            {
                if (coll.TryGetComponent<IInteractible>(out var i) && i.ID == _interactionTarget.ID)
                {
                    _interactionTarget = null;
                }
            });
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _mov * _pInfo.PlayerSpeed;
        }

        private void Update()
        {
            _triggerArea.transform.position = _feet.transform.position + GetAttackDir(_cam, _pInfo, out _);
        }

        private Vector3 GetAttackDir(Camera cam, PlayerInfo playerInfo, out Vector3 pos)
        {
            pos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.z = 0f;

            Gizmos.color = Color.blue;
            var dir = (pos - transform.position).normalized * playerInfo.AttackRange;
            return dir;
        }

        private void OnDrawGizmos()
        {
            var info = _pInfo;
            var cam = Camera.main;
            Gizmos.DrawWireSphere(_feet.transform.position + GetAttackDir(cam, info, out _), info.AttackSize);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();

            //_anim.SetBool("IsMoving", _mov.magnitude > 0f);
            if (_mov.magnitude > 0f)
            {
                //_anim.SetFloat("X", _mov.x);
                //_anim.SetFloat("Y", _mov.y);
            }
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                _mov = Vector2.zero;
                if (_interactionTarget != null && _interactionTarget.CanInteract)
                {
                    _interactionTarget.Interact(this);
                }
            }
        }
    }
}