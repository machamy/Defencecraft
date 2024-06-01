using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshPro NameTMP;
    [SerializeField] private TextMeshPro TextTMP;

    private bool isTyping;
    public float typeDelay;
    private int currentTypeIdx;

    private int msgIdx;
    private string toDisplay;
    private string toDisplayName;


    public void Start()
    {
        msgIdx = 0;
    }


    public void NextMsg()
    {
        
    }


    public void StopType()
    {
        if(!isTyping)
            return;
        currentTypeIdx = toDisplay.Length;
    }

    private IEnumerator TypeRoutine()
    {
        currentTypeIdx = 0;
        var wait = new WaitForSeconds(typeDelay);
        while (currentTypeIdx < toDisplay.Length)
        {
            TextTMP.text = toDisplay.Substring(0, currentTypeIdx+1);
            currentTypeIdx++;
            yield return wait;
        }

        TextTMP.text = toDisplay;
        isTyping = false;
    }
}
