using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _02.Scirpts.Ingame.HUD.Button
{
    public class BaseButton : MonoBehaviour, IPointerClickHandler
    {
        
        [SerializeField] protected UnityEvent OnClickEvent;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickEvent.Invoke();
            
        }
    }
}