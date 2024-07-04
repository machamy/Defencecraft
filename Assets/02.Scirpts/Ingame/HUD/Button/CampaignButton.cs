using _02.Scirpts.Ingame.UI;
using UnityEngine;

namespace _02.Scirpts.Ingame.HUD.Button
{
    public class CampaignButton : StateButton
    {

        // 해당 오브젝트가 몇 스테이지의 GameObject인가?
        [SerializeField] private int stageInfo;

        private MapUI _mapUI;

        protected void Start()
        {
            _mapUI = UIManager.Instance.GetUI(UIManager.UIPrefabType.UI_Map).GetComponent<MapUI>();
            OnPointerUpEvent.AddListener((eventData) => OpenDifficultySelect());
        }

        public void OpenDifficultySelect()
        {
            _mapUI.StageClicked(stageInfo);
        }
    }
}