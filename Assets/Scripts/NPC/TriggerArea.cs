using UnityEngine;
using UnityEngine.Events;

namespace SpicyJam.Player
{
    public class TriggerArea : MonoBehaviour
    {
        public UnityEvent<Collider2D> OnTriggerEvent { get; } = new();
        public UnityEvent<Collider2D> OnTriggerExit { get; } = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEvent.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExit.Invoke(collision);
        }
    }
}