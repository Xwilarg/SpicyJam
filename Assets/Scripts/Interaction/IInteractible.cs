using SpicyJam.Player;
using UnityEngine;

namespace SpicyJam.Interaction
{
    public interface IInteractible
    {
        public void Interact(PlayerController pc);
        public bool CanInteract { get; }
        public GameObject GameObject { get; }

        public void ToggleHighlight(bool value);
    }
}