using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _02.Scirpts.Ingame.UI
{
    public class DifficultyUI : BaseUI
    {

        [SerializeField] private GameObject easyGameObject;
        [SerializeField] private GameObject normalGameObject;
        [SerializeField] private GameObject hardGameObject;
        
        private GameManager.Difficulty _selectedDiff;

        public void OnEnable()
        {
            _selectedDiff = GameManager.Difficulty.Easy;
            easyGameObject.GetComponent<Button>().Select();
        }

        public void ClickedEnterButton()
        {
            UIManager.Instance.MoveScene("MapScene");
            // TODO : 난이도 설정
        }

        public void setDifficulty()
        {

            GameObject current = EventSystem.current.currentSelectedGameObject;

            if (current == easyGameObject)
                _selectedDiff = GameManager.Difficulty.Easy;
            else if (current == normalGameObject)
                _selectedDiff = GameManager.Difficulty.Normal;
            else
                _selectedDiff = GameManager.Difficulty.Hard;

        }
    }
}