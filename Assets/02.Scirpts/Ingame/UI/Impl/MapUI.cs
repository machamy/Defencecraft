using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _02.Scirpts.Ingame.UI
{
    public class MapUI : BaseUI
    {

        private int _selectedStage;
        private GameManager.Difficulty _selectedDiff;

        public void StageClicked(int i)
        {
            _selectedStage = i;
            UIManager.Instance.AddUI(UIManager.UIPrefabType.UI_DifficultySelect);
        }

    }
}