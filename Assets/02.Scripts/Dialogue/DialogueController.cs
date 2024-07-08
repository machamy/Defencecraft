using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Dialogue;
using TMPro;
using UnityEngine;

/// <summary>
/// 대화를 관리하는 클래스
/// </summary>
public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NameTMP;
    [SerializeField] private TextMeshProUGUI TextTMP;

    // [SerializeField] private CharacterSO defaultCharacterSo;

    public bool isActive = false;

    /// <summary>
    /// 자체 터치 상호작용 활성화 여부
    /// </summary>
    public bool isTouchEnabled = true;
    private bool isTyping;
    /// <summary>
    /// 타이핑 애니메이션 딜레이
    /// </summary>
    public float typeDelay;
    private int currentTypeIdx;

    /// <summary>
    /// 현재 보여주고있는 메시지의 idx
    /// </summary>
    private int msgIdx;
    /// <summary>
    /// 대화 정보 == 대본
    /// </summary>
    public DialogueScriptSO DialogueScriptSo = null;
    /// <summary>
    /// 대화 반복자
    /// </summary>
    private IEnumerator<int> _dialogueEnumerator = null;

    /// <summary>
    /// 스탠딩 풀
    /// </summary>
    [SerializeField]private StandingPoolSO _standingPoolSo;

    public void Awake()
    {
        _standingPoolSo.SetParent(this.transform);
        _standingPoolSo.init(2);
        _standingDict = new Dictionary<string, Standing>();
        msgIdx = 0;
    }

    public void Start()
    {
        
    }


    /// <summary>
    /// 이전프레임 터치했는지
    /// </summary>
    private bool isTouched = false;

    public void Update()
    {
        // Debug.Log($"{isTouchEnabled} && {Input.touchCount} {Input.GetMouseButton(0)}");
        if (isTouchEnabled)
            if(Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                if (isTouched) // 꾹 누르는거 무시
                    return;
                
                if (!isActive)
                {
                    StartDialogue();
                    isActive = true;
                    return;
                }
                OnTouch();
                isTouched = true;
            }
            else
            {
                isTouched = false;
            }
    }

    /// <summary>
    /// TODO : 다른 작업중일땐 실행되지않도록, 혹은 다른작업이 실행되지 않도록 해야함
    /// </summary>
    private void OnTouch()
    {
        if (isTyping)
        {
            StopType();
            return;
        }
        NextAction();
    }

    /// <summary>
    /// 해당 대화를 시작한다.
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(DialogueScriptSO dialogue = null)
    {
        if (dialogue is not null)
        {
            DialogueScriptSo = dialogue;
        }
        if (DialogueScriptSo is null)
        {
            Debug.LogWarning("대본이 설정되지 않음");
            return;
        }
        Debug.Log($"[{this.GetType().Name}] Dialogue({DialogueScriptSo.name}) Started");
        _dialogueEnumerator = DialogueScriptSo.ScriptEnumerator(this);
    }

    
    /// <summary>
    /// 다음 메시지로 변경하는 함수
    /// </summary>
    private void NextAction()
    {
        if (_dialogueEnumerator.MoveNext())// 다음 대화를 실행한다
        {
            msgIdx = _dialogueEnumerator.Current;
            return; // early return
        }
        // 더이상 대화가 없으면 종료한다.
        OnDialogEnd();
    }

    #region ScriptInterface
    /// <summary>
    /// 해당 메시지를 보여주는 함수
    /// </summary>
    /// <param name="msg"></param>
    public void Display(string name = "", string msg = "")
    {
        NameTMP.text = name;
        StartCoroutine(TypeRoutine(msg));
    }

    public void Display(Standing standing, string msg = "") => Display(standing.name, msg);

    private Dictionary<string, Standing> _standingDict;

    
    /// <summary>
    /// 해당 이름을 가진 스탠딩을 가져온다.
    /// 없을경우 Character는 임시 캐릭터로 넣어진다
    /// </summary>
    /// <param name="name">가져올 이름</param>
    /// <returns></returns>
    public Standing GetStanding(string name)
    {
        Standing result;
        if (!_standingDict.TryGetValue(name, out result))
        {
            result = _standingPoolSo.Get();
            result.name = name;
            // result.CharacterSO = defaultCharacterSo;
        }
        return result;
    }

    public Standing CreateStanding(string name, CharacterSO characterSo)
    {
        Standing result;
        result = _standingPoolSo.Get();
        result.name = name;
        result.CharacterSO = characterSo;
        return result;
    }
    #endregion
    
    

    /// <summary>
    /// 모든 대화가 끝났을때 호출되는 함수
    /// </summary>
    private void OnDialogEnd()
    {
        // 대본 지움
        _dialogueEnumerator = null;
        DialogueScriptSo = null;
        
        // 스탠딩 제거
        foreach (var standingsValue in _standingDict.Values)
        {
            _standingPoolSo.Return(standingsValue);
        }
        _standingDict.Clear();
    }


    /// <summary>
    /// 타이핑 애니메이션을 멈춘다
    /// </summary>
    private void StopType()
    {
        if(!isTyping)
            return;
        isTyping = false;
    }

    private IEnumerator TypeRoutine(string msg)
    {
        currentTypeIdx = 0;
        var wait = new WaitForSeconds(typeDelay);
        isTyping = true;
        while (currentTypeIdx < msg.Length && isTyping)
        {
            TextTMP.text = msg.Substring(0, currentTypeIdx+1);
            currentTypeIdx++;
            yield return wait;
        }
        
        TextTMP.text = msg;
        isTyping = false;
        yield return null;
    }
}
