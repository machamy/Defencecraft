using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

namespace _02.Scirpts.Ingame
{
    public class ConstructManager : MonoBehaviour
    {
        public bool isConstructMode = false; //ConstructMode 일때만 Update문 진행

        private bool isBuildable = true; // 마우스를 땐 위치의 건설 가능 여부

        private bool canBuild = false; //UI요소 위에서 클릭시 생성되지 않게끔하는 변수

        //건설하고자 하는 건물의 정보
        [SerializeField] public GameObject[] buildingPrefab = new GameObject[4];
        public int buildingTypeIndex = 0; // 0은 x 1은 타워 2는 금고 3은 벽

        private GameObject building; //설치하고자 하는 건물 오브잭트
        SpriteRenderer buildingSpriteRenderer;

        public GameObject buildableTile; //건설 가능여부를 알려주는 발판 
        public Sprite[] buildableTileImage = new Sprite[2];
        int buildableTileIndex = 0; //0은 설치 불가, 1은 설치 가능

        AbstractConstruct buildingScript; //건설하고자 하는 건물의 스크립트

        public Vector3 mouseToPlanePos; //바닥과 마우스가 만나는 위치를 담는 벡터 (5의 배수로만 존재)

        [SerializeField] public Button[] buildingSettingButtons = new Button[3];

        World worldscript;
        Animator animator;

        private void Awake()
        {
            worldscript = GetComponent<World>();
        }


        void ActivateConstructMode()
        {
            isConstructMode = true;
            //화면 톤 다운
            UIManager.Instance.SetCanvasToneDown();
            //버튼 모양 변경

            //게임 속도 조정
            GameManager.Instance.SetGameSpeed(0.1f);
        }

        void DeActivateConstructMode()
        {
            isConstructMode = false;
            //화면 톤 다운
            UIManager.Instance.SetCanvasToneUp();
            //버튼 모양 변경

            //게임 속도 조정
            GameManager.Instance.SetGameSpeed(1f);
        }

        public void TowerBuildBtn()
        {
            if (isConstructMode && buildingTypeIndex == 1) //버튼이 한번 더 클릭되었을 때 건설모드 종료
            {
                DeActivateConstructMode();
                buildingSettingButtons[1].OnDeselect(null);
                return;
            }

            if(!isConstructMode)
                ActivateConstructMode();

            buildingSettingButtons[1].OnSelect(null);
            buildingTypeIndex = 1;
        }

        public void SafeBuildBtn()
        {
            if (isConstructMode && buildingTypeIndex == 2) //버튼이 한번 더 클릭되었을 때 건설모드 종료
            {
                DeActivateConstructMode();
                buildingSettingButtons[2].OnDeselect(null);
                return;
            }

            if (!isConstructMode)
                ActivateConstructMode();

            buildingSettingButtons[2].OnSelect(null);
            buildingTypeIndex = 2;
        }

        public void WallBuildBtn()
        {
            if (isConstructMode && buildingTypeIndex == 3) //버튼이 한번 더 클릭되었을 때 건설모드 종료
            {
                DeActivateConstructMode();
                buildingSettingButtons[0].OnDeselect(null);
                return;
            }

            if (!isConstructMode)
                ActivateConstructMode();

            buildingSettingButtons[0].OnSelect(null);
            buildingTypeIndex = 3;
        }

        void UpdateMousePosition()
        {
            //마우스 위치 불러오기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f)); //땅에 닿는 위치가 y 높이는 0.2f

            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
                mouseToPlanePos = ray.GetPoint(rayDistance);

            //5의 배수로만 이동하게끔 수정
            mouseToPlanePos = new Vector3(Mathf.Round((mouseToPlanePos.x) / 5f) * 5, Mathf.Round(mouseToPlanePos.y), Mathf.Round((mouseToPlanePos.z)/ 5f) * 5);
        }

        void SetTileConstruct(int x, int y, GameObject building)
        {
            //건설 불가 지역으로 설정
            for (int i = 0; i < buildingScript.size[0]; i++)
            {
                for (int j = 0; j < buildingScript.size[1]; j++)
                {
                    //타일정보 받아오기
                    Tile tile = worldscript.GetTile(x + i, y + j);

                    //건설 불가 지역으로 설정
                    tile.Construct = buildingScript;
                }
            }
        }

        void CheckTile(int tilenum_x, int tilenum_z, int[] size)
        {
            isBuildable = true;
            //해당 좌표에 타일이 있다면 tile 정보 불러오기
            if ((tilenum_x >= 0 && tilenum_x + size[0] < 21) && (tilenum_z >= 0 && tilenum_z + size[1] < 21))
            {
                for (int i = 0; i<size[0]; i++)
                {
                    for (int j = 0; j<size[1]; j++)
                    {
                        //타일정보 받아오기
                        Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                        //건설가능한 지역인지 확인
                        if (!tile.IsConstructable)
                        {
                            isBuildable = false;
                            break;
                        }
                    }

                    if (!isBuildable)
                    {
                        break;
                    }
                }
            }
            else //타일이 없다면 파괴
            {
                isBuildable = false;
            }
        }

        private void Update()
        {
            if (isConstructMode)
            {

                //마우스 내려갈때 ConstructPrefab 생성
                if (Input.GetMouseButtonDown(0))
                {
                    //UI 요소 안에 마우스가 있으면 리턴
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    building = Instantiate(buildingPrefab[buildingTypeIndex]);
                    buildableTile.SetActive(true);

                    buildingScript = building.GetComponent<AbstractConstruct>();
                    //고스트 빌딩은 빌딩 상테
                    buildingSpriteRenderer = building.GetComponent<SpriteRenderer>();
                    Color color = buildingSpriteRenderer.color;
                    color.a = 0.5f;
                    buildingSpriteRenderer.color = color;

                    canBuild = true;
                }

                //마우스 드래그중 반투명 건물이 마우스 따라다니게 하기
                if (Input.GetMouseButton(0) && canBuild)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    UpdateMousePosition();
                    //반투명 건물의 위치 변경
                    building.transform.position = new Vector3(mouseToPlanePos.x + 2.5f, 1f, mouseToPlanePos.z + 2.5f);
                    buildableTile.transform.position = new Vector3(mouseToPlanePos.x+2.5f, 1f, mouseToPlanePos.z+2.5f);

                    int tilenum_x = Mathf.RoundToInt(mouseToPlanePos.x) / 5;
                    int tilenum_z = Mathf.RoundToInt(mouseToPlanePos.z) / 5;

                    //CheckTile(tilenum_x, tilenum_z, buildingScript.size);

                    //건설 가능 여부 이미지 변경
                    if(isBuildable && buildableTileIndex == 0)
                    {
                        buildableTile.GetComponent<SpriteRenderer>().sprite = buildableTileImage[1];
                        buildableTileIndex = 1;
                    }else if(!isBuildable && buildableTileIndex == 1)
                    {
                        buildableTile.GetComponent<SpriteRenderer>().sprite = buildableTileImage[0];
                        buildableTileIndex = 0;
                    }

                }

                //마우스를 땠을때
                if (Input.GetMouseButtonUp(0) && canBuild)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    //건설 가능 여부 이미지 비활성화
                    buildableTile.SetActive(false);
                    canBuild = false;

                    //좌표 불러오기
                    // int tilenum_x = Mathf.RoundToInt(mouseToPlanePos.x) / 5;
                    // int tilenum_z = Mathf.RoundToInt(mouseToPlanePos.z) / 5;

                    //타일 정보 확인
                    //CheckTile(tilenum_x, tilenum_z, buildingScript.size);

                    //건설 가능 판별이 났다면 건설
                    if (isBuildable)
                    {

                        Color color = buildingSpriteRenderer.color;
                        color.a = 1f;
                        buildingSpriteRenderer.color = color;

                        animator = buildingScript.GetComponent<Animator>();
                        animator.SetBool("Isinstall", true);

                        Tile tile = worldscript.TileFromWorldPoint(mouseToPlanePos);

                        SetTileConstruct(tile.getTileX(), tile.getTileY(), building);

                        
                    }
                    else
                    {
                        Destroy(building);
                    }

                    ;
                }
            }
            else
                return;
        }

    }
}
