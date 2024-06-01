using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Dialogue.Event;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// 각 대화의 대본이 저장되는 SO
/// </summary>
[CreateAssetMenu(fileName = "NewDialogueScript", menuName = "Scriptable Object/Dialogue/DialogueScript")]
public class DialogueScriptSO : ScriptableObject
{
    public List<DialogueMessage> Script;
    public int Length => Script.Count;

}

/// <summary>
/// 대화 하나하나의 정보
/// </summary>
[Serializable]
public class DialogueMessage
{
    public string name;
    public CharacterSO Character;
    public string text;
    
    /// <summary>
    /// 해당 대화에 진입할시 별도의 함수가 실행되야한다면 이벤트 작성
    /// </summary>
    public AbstractDialogueEventSO enterEvent;
}




