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

        public bool IsInDialogue { set; private get; }

        public bool CanPlay => !IsInDialogue;
    }
}