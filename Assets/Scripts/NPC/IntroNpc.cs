using SpicyJam.Interaction;
using SpicyJam.Manager;
using SpicyJam.Player;
using UnityEngine;

namespace SpicyJam.NPC
{
    public class IntroNpc : MonoBehaviour, IInteractible
    {
        private SpriteRenderer _sr;

        public bool CanInteract => true;

        public GameObject GameObject => gameObject;
        private Color _baseColor;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _baseColor = _sr.color;
        }

        public void Interact(PlayerController pc)
        {
            StoryManager.Instance.ShowIntro();
        }

        public void ToggleHighlight(bool value)
        {
            _sr.color = value ? Color.green : _baseColor;
        }
    }
}