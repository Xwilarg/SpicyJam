using SpicyJam.NPC;
using UnityEngine;

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

        private void Awake()
        {
            Instance = this;
            _storyContainer.SetActive(false);
        }

        public void ShowDescription(NpcController npc)
        {
            _storyContainer.SetActive(true);
            _display.ToDisplay = npc.GetDescription();
        }

        public void CloseStory()
        {
            _storyContainer.SetActive(false);
        }
    }
}