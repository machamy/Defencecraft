using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 대화를 관리하는 클래스
/// </summary>
public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshPro NameTMP;
    [SerializeField] private TextMeshPro TextTMP;

    private bool isTyping;
    public float typeDelay;
    private int currentTypeIdx;

    /// <summary>
    /// 현재 보여주고있는 메시지의 idx
    /// </summary>
    private int msgIdx;
    /// <summary>
    /// 대화 정보
    /// </summary>
    public DialogueScriptSO DialogueScript;
    [HideInInspector]public DialogueMessage CurrentMessage;


    public void Start()
    {
        msgIdx = 0;
    }
    

    /// <summary>
    /// TODO : 다른 작업중일땐 실행되지않도록, 혹은 다른작업이 실행되지 않도록 해야함
    /// </summary>
    public void OnTouch()
    {
        if (isTyping)
        {
            StopType();
            return;
        }
        NextMsg();
    }


    /// <summary>
    /// 다음 메시지로 변경하는 함수
    /// </summary>
    public void NextMsg()
    {
        if (DialogueScript == null)
        {
            Debug.LogWarning("대본이 설정되지 않음");
            return;
        }
        msgIdx++;
        if (msgIdx == DialogueScript.Length)
        {
            OnDialogEnd();
        }
        ShowMsg(DialogueScript.Script[msgIdx]);
    }

    /// <summary>
    /// 해당 메시지를 보여주는 함수
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMsg(DialogueMessage msg)
    {
        CurrentMessage = msg;
        NameTMP.text = msg.name;
        StartCoroutine(TypeRoutine());
    }

    /// <summary>
    /// 모든 대화가 끝났을때 호출되는 함수
    /// </summary>
    public void OnDialogEnd()
    {
        CurrentMessage = null;
    }


    /// <summary>
    /// 타이핑 애니메이션을 멈춘다
    /// </summary>
    public void StopType()
    {
        if(!isTyping)
            return;
        currentTypeIdx = CurrentMessage.text.Length;
    }

    private IEnumerator TypeRoutine()
    {
        currentTypeIdx = 0;
        var wait = new WaitForSeconds(typeDelay);
        while (currentTypeIdx < CurrentMessage.text.Length)
        {
            TextTMP.text = CurrentMessage.text.Substring(0, currentTypeIdx+1);
            currentTypeIdx++;
            yield return wait;
        }

        TextTMP.text = CurrentMessage.text;
        isTyping = false;
    }
}
