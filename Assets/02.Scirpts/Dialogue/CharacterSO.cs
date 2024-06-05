using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Dictionary;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 캐릭터의 스프라이트, 표정등을 저장하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "Scriptable Object/Dialogue/Character")]
public class CharacterSO : ScriptableObject
{
    private static Dictionary<string, CharacterSO> _characterDict = new Dictionary<string, CharacterSO>();
    public static CharacterSO GetBySOName(string name) => _characterDict[name];

    public Sprite fullSprite;

    [SerializeField]private SerializableDict<Face, Sprite> faceDict;

    private void Awake()
    {
        _characterDict.Add(name,this);
    }

    public Sprite GetFace(Face face)
    {
        Sprite result = faceDict.Get(face);
        return result;
    }
}

public enum Face
{
    None = 0,
    Idle = 1,
    Angry = 2,
    Smile = 3,
    Wow = 4,
    Frightened = 5,
    Gonran = 6,
    Biyeol = 7,
    Tired = 8,
    Serious = 9,
    
    All = 16
}