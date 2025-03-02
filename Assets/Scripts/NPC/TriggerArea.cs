using UnityEngine;
using UnityEngine.Events;

namespace SpicyJam.Player
{
    public class TriggerArea : MonoBehaviour
    {
        public UnityEvent<Collider2D> OnTriggerEnter { get; } = new();
        public UnityEvent<Collider2D> OnTriggerExit { get; } = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnter.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExit.Invoke(collision);
        }
    }
}