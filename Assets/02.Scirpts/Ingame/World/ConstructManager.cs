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
        public int buildingType = 0;

        public bool isConstructMode = true;
        private bool isBuildable = true;

        private int[,] buildingSize = { { 3, 3 }, { 2, 2 }, { 2, 2 }, { 2, 2 } };
        private bool[,] isConstructable;

        private int[] worldSize;

        private GameObject building;
        public GameObject[] buildingprefab = new GameObject[4];

        Vector3 pos;

        

        private void Start()
        {
            World worldscript = GetComponent<World>();

            worldSize = new int[2] { worldscript.Width, worldscript.Height };

            //좌표별 isconstructable에 true로 초기화
            for (int i = 0; i < worldSize[0]; i++)
            {
                for (int j = 0; j < worldSize[1]; j++)
                {
                    isConstructable[i, j] = true;
                }
            }
        }

        private void Update()
        {
            //마우스 내려갈때 ConstructPrefab 생성
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("마우스 내려감");

                //건설모드라면 반투명 건물 생성
                if (isConstructMode)
                {
                    building = Instantiate(buildingprefab[buildingType]);

                    //새로 클릭 할때마다 초기화
                    isBuildable = true;
                }
            }

            //마우스 드래그중 반투명 건물이 마우스 따라다니게 하기
            if (Input.GetMouseButton(0))
            {
                //마우스 위치 불러오기
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, 0.2f, 0f)); //땅에 닿는 위치가 y 높이는 0.2f

                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance))
                    pos = ray.GetPoint(rayDistance);

                //5의 배수로만 이동하게끔 수정
                pos = new Vector3(Mathf.Round(pos.x / 5) * 5, Mathf.Round(pos.y), Mathf.Round(pos.z / 5) * 5);
                
                Debug.Log(pos);

                //반투명 건물의 위치 변경
                building.transform.position = pos;
            }

            //마우스를 땠을때
            if(Input.GetMouseButtonUp(0))
            {
                World worldscript = GetComponent<World>();
                Debug.Log(Mathf.RoundToInt(pos.x));

                //좌표 불러오기
                int tilenum_x = Mathf.RoundToInt(pos.x) / 5;
                int tilenum_z = Mathf.RoundToInt(pos.z) / 5;

                //해당 좌표에 타일이 있다면 tile 정보 불러오기
                if ((tilenum_x >= 0 && tilenum_x + buildingSize[buildingType, 0] < 21) && (tilenum_z >= 0 && tilenum_z + buildingSize[buildingType, 1] < 21))
                {
                    for(int i = tilenum_x; i < buildingSize[buildingType, 0]; i++)
                    {
                        for(int j = tilenum_z; j < buildingSize[buildingType, 1]; j++)
                        {
                            //타일정보 받아오기
                            Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                            //건설가능한 지역인지 확인
                            if (!isConstructable[i, j] || !tile.IsConstructable)
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

                    //임시로 만들어놓은 건설 알림
                    //Instantiate(buildingprefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab[buildingType], new Vector3(pos.x + 5, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab[buildingType], new Vector3(pos.x, pos.y, pos.z + 5), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab[buildingType], new Vector3(pos.x + 5, pos.y, pos.z + 5), Quaternion.Euler(0, 0, 0));

                    //건설 불가 지역으로 설정
                    for (int i = tilenum_x; i < buildingSize[buildingType, 0]; i++)
                    {
                        for (int j = tilenum_z; j < buildingSize[buildingType, 1]; j++)
                        {
                            //타일정보 받아오기
                            Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                            //건설 불가 지역으로 설정
                            isConstructable[i, j] = false;
                        }
                    }

                }
                else
                {
                    Destroy(building);
                }

            }
        }

    }
}
