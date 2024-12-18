using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SpaceScavengers
{
    public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event UnityAction OnPressStart;
        public event UnityAction OnPressStop;

        public void OnPointerDown(PointerEventData eventData) => OnPressStart?.Invoke();

        public void OnPointerUp(PointerEventData eventData) => OnPressStop?.Invoke();

        private void OnDisable()
        {
            OnPressStart = null;
            OnPressStop = null;
        }
    }
}