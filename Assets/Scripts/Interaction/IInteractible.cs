using SpicyJam.Player;

namespace SpicyJam.Interaction
{
    public interface IInteractible
    {
        public void Interact(PlayerController pc);
        public bool CanInteract { get; }
    }
}