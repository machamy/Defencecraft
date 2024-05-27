using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

namespace _02.Scirpts.Ingame
{
    public class ConstructManager : MonoBehaviour
    {
        public bool isConstructMode = true; //ConstructMode 일때만 Update문 진행

        private bool isBuildable = true; // 마우스를 땐 위치의 건설 가능 여부

        //건설하고자 하는 건물의 정보
        [SerializeField] public GameObject[] buildingPrefab = new GameObject[4];
        public int buildingTypeIndex = 0;

        private GameObject ghostbuilding; //드래그 시 마우스를 따라가는 반투명 가이드 건물

        AbstractConstruct buildingScript; //건설하고자 하는 건물의 스크립트

        public Vector3 mouseToPlanePos; //바닥과 마우스가 만나는 위치를 담는 벡터 (5의 배수로만 존재)

        World worldscript;

        private void Awake()
        {
            worldscript = GetComponent<World>();
        }

        void GetMousePosition()
        {
            //마우스 위치 불러오기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, 0.2f, 0f)); //땅에 닿는 위치가 y 높이는 0.2f

            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
                mouseToPlanePos = ray.GetPoint(rayDistance);

            //5의 배수로만 이동하게끔 수정
            mouseToPlanePos = new Vector3(Mathf.Round(mouseToPlanePos.x / 5) * 5, Mathf.Round(mouseToPlanePos.y), Mathf.Round(mouseToPlanePos.z / 5) * 5);
        }

        void SetTileConstruct(int x, int y, GameObject building)
        {
            //건설 불가 지역으로 설정
            for (int i = x; i < buildingScript.size[0]; i++)
            {
                for (int j = y; j < buildingScript.size[0]; j++)
                {
                    //타일정보 받아오기
                    Tile tile = worldscript.GetTile(x + i, y + j);

                    //건설 불가 지역으로 설정
                    tile.Construct = building.GetComponent<AbstractConstruct>();
                    Debug.Log(tile.Construct);
                }
            }
        }


        private void Start()
        {

        }

        private void Update()
        {
            if (isConstructMode)
            {
                //마우스 내려갈때 ConstructPrefab 생성
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("마우스 내려감");            
                    ghostbuilding = Instantiate(buildingPrefab[buildingTypeIndex]);
                   
                    buildingScript = ghostbuilding.GetComponent<AbstractConstruct>();
                    //새로 클릭 할때마다 초기화
                    isBuildable = true;
                
                }

                //마우스 드래그중 반투명 건물이 마우스 따라다니게 하기
                if (Input.GetMouseButton(0))
                {
                    GetMousePosition();
                    //반투명 건물의 위치 변경
                    ghostbuilding.transform.position = mouseToPlanePos;
                }

                //마우스를 땠을때
                if (Input.GetMouseButtonUp(0))
                {
                    //좌표 불러오기
                    int tilenum_x = Mathf.RoundToInt(mouseToPlanePos.x) / 5;
                    int tilenum_z = Mathf.RoundToInt(mouseToPlanePos.z) / 5;

                    //해당 좌표에 타일이 있다면 tile 정보 불러오기
                    if ((tilenum_x >= 0 && tilenum_x + buildingScript.size[0] < 21) && (tilenum_z >= 0 && tilenum_z + buildingScript.size[0] < 21))
                    {
                        for (int i = tilenum_x; i < buildingScript.size[0]; i++)
                        {
                            for (int j = tilenum_z; j < buildingScript.size[0]; j++)
                            {
                                //타일정보 받아오기
                                Tile tile = worldscript.GetTile(i, j);

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

                    //건설 가능 판별이 났다면 건설
                    if (isBuildable)
                    {
                        //건설
                        GameObject building = Instantiate(buildingPrefab[buildingTypeIndex], new Vector3(mouseToPlanePos.x, 0, mouseToPlanePos.z), Quaternion.Euler(0f, 0f, 0f));

                        Tile tile = worldscript.GetTile(tilenum_x, tilenum_z);

                        SetTileConstruct(tilenum_x, tilenum_z, building);
                    }
                    else
                    {
                        Destroy(ghostbuilding);
                    }
                }
            }
        }

    }
}
