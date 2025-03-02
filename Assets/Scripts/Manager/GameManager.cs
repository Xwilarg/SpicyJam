using UnityEngine;

namespace SpicyJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public bool CanPlay => !StoryManager.Instance.IsStoryShown;
    }
}