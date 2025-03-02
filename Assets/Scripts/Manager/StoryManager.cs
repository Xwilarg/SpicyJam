using SpicyJam.NPC;
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
                b.interactable = !npc.IsPriest;
            }
        }

        public void Kill()
        {
            if (_currentNpc.IsVampire) Debug.Log("You killed a vampire!");
            else Debug.Log("The person you killed wasn't a vampire");
            NpcManager.Instance.Kill(_currentNpc);
        }

        public void CloseStory()
        {
            _storyContainer.SetActive(false);
        }
    }
}