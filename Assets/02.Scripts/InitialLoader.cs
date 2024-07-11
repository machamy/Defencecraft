using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts;
using UnityEngine;

/// <summary>
/// 게임 실행후 싱글턴과 데이터 로드
/// </summary>
public class InitialLoader : MonoBehaviour
{
    [SerializeField] private SettingsSO _settingsSo;
    private void Awake()
    {
        GameManager gm = GameManager.Instance;
        gm.SetSettingSO(_settingsSo);
    }
}
