using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _02.Scirpts.Dialogue
{
    
    // [CreateAssetMenu(fileName = "newDialogueScript", menuName = "ScriptableObject/DialogueScript/CreateScriptA")]
    public abstract class DialogueScriptSO: ScriptableObject
    {
        protected int idx = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>현재 출력중인 메시지의 idx</returns>
        public abstract IEnumerator<int> ScriptEnumerator(DialogueController controller);
    }
}