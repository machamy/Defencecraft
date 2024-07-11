using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 각종 싱글턴 같은 씬별로 초기화 해줘야할 것들 초기화
/// </summary>
public class EditorInitializer : MonoBehaviour
{
    #if UNITY_EDITOR
    public List<GameObject> Managers;
    [FormerlySerializedAs("activate")] public bool doActivate = false;
    public static bool activated = false;
    public void Awake()
    {
        if (!doActivate || activated)
            return;
        var array = GameObject.FindGameObjectsWithTag("seperater");
        GameObject ManagerSeperater;
        if (array.Length > 0 || array.Any(e => e.name.ToLower().Contains("manager")))
            ManagerSeperater = array.First((o => o.name.ToLower().Contains("manager")));
        else
            ManagerSeperater = new GameObject("--- Manager ---");
        
        foreach (var manager in Managers)
        {
            if(GameObject.Find(manager.name))
                continue;
            GameObject obj = Instantiate(manager);
            obj.name = manager.name;
            obj.transform.SetSiblingIndex(ManagerSeperater.transform.GetSiblingIndex() + 1);
        }

        activated = true;
    }
    #endif
}
