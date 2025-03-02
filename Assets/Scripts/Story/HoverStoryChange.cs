using SpicyJam.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpicyJam.Story
{
    public class HoverStoryChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, TextArea]
        private string _displayText;

        public void OnPointerEnter(PointerEventData eventData)
        {
            StoryManager.Instance.UpdateText(_displayText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StoryManager.Instance.UpdateText(null);
        }
    }
}