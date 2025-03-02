using UnityEngine;

namespace SpicyJam.Manager
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }
    }
}