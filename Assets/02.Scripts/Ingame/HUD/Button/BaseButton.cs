using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _02.Scirpts.Ingame.HUD.Button
{
    public class BaseButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        
        [SerializeField] protected UnityEvent<PointerEventData> OnClickEvent;
        [SerializeField] protected UnityEvent<PointerEventData> OnPointerDownEvent;
        [SerializeField] protected UnityEvent<PointerEventData> OnPointerUpEvent;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent.Invoke(eventData);
        }
    }
}