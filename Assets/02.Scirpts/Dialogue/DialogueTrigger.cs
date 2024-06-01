using UnityEngine;

namespace _02.Scirpts.Dialogue
{
    /// <summary>
    /// 대화를 시작할 수 있도록 하는 클래스.
    /// 이 클래스를 쓰지 않아도 사용은 가능
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        public DialogueScriptSO[] scripts;
        public DialogueController Controller;

        public void Trigger(int idx)
        {
            if (Controller == null)
            {
                Controller = FindObjectOfType<DialogueController>();
                /*
                 * 작성중
                 */
            }
        }
    }
}