using System.Collections.Generic;
using UnityEngine;

namespace _02.Scirpts.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueScript1-1", menuName = "ScriptableObject/DialogueScript/Script1-1")]
    public class DialogueScript1_1 : DialogueScriptSO
    {
        /*
         *
         * 1-1 (캠페인1 처음 하려고 하면 출력)
         * 배경: 왕궁
         * 왕(겁에 질린 표정) : 마왕이 부활하고 말다니 이게 무슨 일인가!? 대신!
         * 대신(기본) : 폐하, 준동한 마왕군은 지금 브로드웰 협곡에까지 들이 닥치고 있습니다. 속히 방어해야 합니다⋯!
         * 왕(깜작 놀란 표정) : 브로드웰 협곡이라니! 그곳이 뚫리면 우리 왕국의 수도까지 너른 평원뿐이지 않나!
         * 대신(곤란한 표정) : 예, 폐하, 마왕군이 브로드웰 협곡을 넘는다면 수도까지 3일이면 도달 할 것입니다⋯.
         * 왕(기본) : 여기 누구! 브로드웰 협곡을 수비할 용자는 없는가!
         * 마법사 속마음(기본) : 브로드웰 협곡은 지형을 믿고 그간 방비를 소홀히 해온 곳이지. 하지만 아무리 지형이 좋다 한들, 보고된 마왕군의 규모가 사실이라면 속수무책일 수 밖에 없을거야. 사실상 죽으러 가는 곳이겠군⋯
         * 대신(비열한 표정) : 아무래도 마법사가 적임일 것 같습니다.
         * 마법사(깜짝 놀란 표정) : 뭣!
         * 왕(웃는 표정) : 오호 마법사 네가 할 수 있겠느냐?
         * 마법사(기본) : 아, 저, 그, 아뢰옵기 황공하오나 신은 평생을 마법 수련을 위해 책상 앞에서 보내었습니다. 전장을 지휘하는데에는 능력이 미치지 못함을 고려해주시옵소서⋯.
         * (기본) : 그런가⋯.
         * 대신(비열한 표정) : 폐하, 아니옵니다. 마법사는 비록 전장에 직접 서본 경험은 없겠으나, 백면서생 답게 책상 앞에 앉아서도 신문 한 장으로 세상사를 꿰뚫고 있으니 브로드웰 협곡을 맏기기에 충분할 것입니다⋯!
         * 왕(웃는 표정) : (고민하다가) 마법사가 ㅁㅁ협곡에 다녀 오너라!
         */
        [SerializeField] private CharacterSO KingSO;
        [SerializeField] private CharacterSO MageSO;
        [SerializeField] private CharacterSO InsteadSO;
        
        private Standing King;
        private Standing Mage;
        private Standing Instead;
        
        public override IEnumerator<int> ScriptEnumerator(DialogueController controller)
        {
            King = controller.CreateStanding("왕",KingSO);
            Mage = controller.CreateStanding("마법사",MageSO);
            Instead = controller.CreateStanding("대신",InsteadSO);
            
            King.SetFace(Face.Frightened);
            controller.Display(King,"마왕이 부활하고 말다니 이게 무슨 일인가!? 대신!");
            yield return idx++;
            Instead.SetFace(Face.Idle);
            controller.Display(Instead,"폐하, 준동한 마왕군은 지금 브로드웰 협곡에까지 들이 닥치고 있습니다. 속히 방어해야 합니다⋯!");
            yield return idx++;
            King.SetFace(Face.Wow);
            controller.Display(King,"브로드웰 협곡이라니! 그곳이 뚫리면 우리 왕국의 수도까지 너른 평원뿐이지 않나!");
            yield return idx++;
            Instead.SetFace(Face.Gonran);
            controller.Display(Instead,"예, 폐하, 마왕군이 브로드웰 협곡을 넘는다면 수도까지 3일이면 도달 할 것입니다⋯.");
            yield return idx++;
            King.SetFace(Face.Idle);
            controller.Display(King,"여기 누구! 브로드웰 협곡을 수비할 용자는 없는가!");
            yield return idx++;
            Mage.SetFace(Face.Idle);
            controller.Display(Mage,"브로드웰 협곡은 지형을 믿고 그간 방비를 소홀히 해온 곳이지. 하지만 아무리 지형이 좋다 한들, 보고된 마왕군의 규모가 사실이라면 속수무책일 수 밖에 없을거야. 사실상 죽으러 가는 곳이겠군⋯");
            yield return idx++;
            Instead.SetFace(Face.Biyeol);
            controller.Display(Instead,"아무래도 마법사가 적임일 것 같습니다.");
            yield return idx++;
            Mage.SetFace(Face.Wow);
            controller.Display(Mage,"뭣!");
            yield return idx++;
            King.SetFace(Face.Smile);
            controller.Display(King,"오호 마법사 네가 할 수 있겠느냐?");
            yield return idx++;
            Mage.SetFace(Face.Idle);
            controller.Display(Mage,"아, 저, 그, 아뢰옵기 황공하오나 신은 평생을 마법 수련을 위해 책상 앞에서 보내었습니다. 전장을 지휘하는데에는 능력이 미치지 못함을 고려해주시옵소서⋯.");
            yield return idx++;
            King.SetFace(Face.Idle);
            controller.Display(King,"그런가⋯.");
            yield return idx++;
            Instead.SetFace(Face.Biyeol);
            controller.Display(Instead,"폐하, 아니옵니다. 마법사는 비록 전장에 직접 서본 경험은 없겠으나, 백면서생 답게 책상 앞에 앉아서도 신문 한 장으로 세상사를 꿰뚫고 있으니 브로드웰 협곡을 맏기기에 충분할 것입니다⋯!");
            yield return idx++;
            King.SetFace(Face.Smile);
            controller.Display(King,"(고민하다가) 마법사가 ㅁㅁ협곡에 다녀 오너라!");
            yield return idx++;
        }
    }
}