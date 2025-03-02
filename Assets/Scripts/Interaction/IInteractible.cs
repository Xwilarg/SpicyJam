using SpicyJam.Player;

namespace SpicyJam.Interaction
{
    public interface IInteractible
    {
        public int ID { get; }

        public void Interact(PlayerController pc);
        public bool CanInteract { get; }
    }
}