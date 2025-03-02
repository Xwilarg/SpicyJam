using InflationPotion.SO;
using SpicyJam.Interaction;
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

        private Vector2 _mov;

        private Camera _cam;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            //_anim = GetComponentInChildren<Animator>();

            _cam = Camera.main;
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _mov * _pInfo.PlayerSpeed;
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
                //_interactionTarget.Interact(this);
            }
        }
    }
}