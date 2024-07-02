using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scirpts.Ingame.UI
{
    public class BaseUI : MonoBehaviour
    {

        [SerializeField] private bool isFocused;
        [SerializeField] private bool removeUIOnFocusClicked = true;

        private GameObject go_instance;
        
        public bool IsFocused
        {
            get => isFocused;
            set => isFocused = value;
        }

        public bool RemoveUIFocusClicked
        {
            get => removeUIOnFocusClicked;
            set => removeUIOnFocusClicked = value;
        }
    }
}