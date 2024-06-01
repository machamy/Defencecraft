using UnityEngine;

namespace _02.Scirpts.Dialogue.Event
{
    public abstract class AbstractDialogueEventSO : ScriptableObject
    {
        public abstract void Invoke();
    }
}