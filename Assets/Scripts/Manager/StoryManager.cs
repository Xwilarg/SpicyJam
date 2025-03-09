using SpicyJam.NPC;
using SpicyJam.Story;
using UnityEngine;
using UnityEngine.UI;

namespace SpicyJam.Manager
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance { private set; get; }

        public bool IsStoryShown => _storyContainer.activeInHierarchy;

        [SerializeField]
        private GameObject _storyContainer;

        [SerializeField]
        private TextDisplay _display;

        [SerializeField, Tooltip("Options that you can't do on VIP people (like the priest)")]
        private Button[] _dangerousOptions;

        [SerializeField]
        private Button _molestButton;

        private NpcController _currentNpc;

        private void Awake()
        {
            Instance = this;
            _storyContainer.SetActive(false);
        }

        public void ShowDescription(NpcController npc)
        {
            _currentNpc = npc;
            _storyContainer.SetActive(true);
            _display.ToDisplay = npc.GetDescription();

            if (npc.IsPriest)
            {
                foreach (var b in _dangerousOptions)
                {
                    b.interactable = false;
                }
            }
            else
            {
                foreach (var b in _dangerousOptions)
                {
                    b.interactable = true;
                }
                _molestButton.interactable = !npc.WasMolested;
            }
        }

        public void UpdateText(string text)
        {
            _display.ToDisplay = text ?? _currentNpc.GetDescription();
        }

        public void Kill()
        {
            if (_currentNpc.IsVampire) Debug.Log("You killed a vampire!");
            else Debug.Log("The person you killed wasn't a vampire (gameover)");
            NpcManager.Instance.Kill(_currentNpc);
            CloseStory();
        }

        public void Molest()
        {
            if (_currentNpc.IsVampire) Debug.Log("You forced yourself on a vampire and was bitten (gameover)");
            else Debug.Log("You forced yourself on someone");
            _currentNpc.WasMolested = true;
            CloseStory();
        }

        public void Unmark()
        {
            _currentNpc.MarkType = MarkType.None;
        }

        public void MarkAsVampire()
        {
            _currentNpc.MarkType = MarkType.VampireMark;
        }

        public void MarkAsInnocent()
        {
            _currentNpc.MarkType = MarkType.InnocentMark;
        }

        public void CloseStory()
        {
            _storyContainer.SetActive(false);
        }
    }
}