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

        [SerializeField]
        private Button[] _dangerousOptions;

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
            foreach (var b in _dangerousOptions)
            {
                b.interactable = !npc.IsPriest && !npc.WasMolested;
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
            if (_currentNpc.IsVampire) Debug.Log("You molested a vampire and was bitten (gameover)");
            else Debug.Log("You molested a person");
            _currentNpc.WasMolested = true;
            CloseStory();
        }

        public void CloseStory()
        {
            _storyContainer.SetActive(false);
        }
    }
}