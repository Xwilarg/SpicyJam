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

        private int _strikeCount = 0;

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
            NpcManager.Instance.Kill(_currentNpc);
            if (_currentNpc.IsVampire)
            {
                if (!NpcManager.Instance.IsThereVampireLeft)
                {
                    // Victory
                }
            }
            else
            {
                _strikeCount++;
                if (_strikeCount == 3)
                {
                    // Defeat
                }
            }
            CloseStory();
        }

        public void Molest()
        {
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