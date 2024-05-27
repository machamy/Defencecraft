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
        private bool isbuildable = true;

        private int[,] building_size = { { 3, 3 }, { 2, 2 }, { 2, 2 }, { 2, 2 } };
        private bool[,] isconstructable;

        private int[] world_size;

        private GameObject building;
        public GameObject buildingprefab;

        Vector3 pos;

        

        private void Start()
        {
            World worldscript = GetComponent<World>();

            world_size = new int[2] { worldscript.Width, worldscript.Height };

            //좌표별 isconstructable에 true로 초기화
            for (int i = 0; i < world_size[0]; i++)
            {
                for (int j = 0; j < world_size[1]; j++)
                {
                    isconstructable[i, j] = true;
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
                    building = Instantiate(buildingprefab);

                    //새로 클릭 할때마다 초기화
                    isbuildable = true;
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
                if ((tilenum_x >= 0 && tilenum_x + building_size[buildingType, 0] < 21) && (tilenum_z >= 0 && tilenum_z + building_size[buildingType, 1] < 21))
                {
                    for(int i = 0; i < building_size[buildingType, 0]; i++)
                    {
                        for(int j = 0; j < building_size[buildingType, 1]; j++)
                        {
                            //타일정보 받아오기
                            Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                            Debug.Log(tile.IsConstructable);

                            //건설가능한 지역인지 확인
                            if (!isconstructable[i, j])
                            {
                                isbuildable = false;
                                break;
                            }
                        }

                        if (!isbuildable)
                        {
                            break;
                        }
                    }
                }
                else //타일이 없다면 파괴
                {
                    isbuildable = false;
                }

                //건설 가능 판별이 났다면 건설
                if (isbuildable)
                {
                    //건설

                    //임시로 만들어놓은 건설 알림
                    //Instantiate(buildingprefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab, new Vector3(pos.x + 5, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab, new Vector3(pos.x, pos.y, pos.z + 5), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab, new Vector3(pos.x + 5, pos.y, pos.z + 5), Quaternion.Euler(0, 0, 0));

                    //건설 불가 지역으로 설정
                    for (int i = 0; i < building_size[buildingType, 0]; i++)
                    {
                        for (int j = 0; j < building_size[buildingType, 1]; j++)
                        {
                            //타일정보 받아오기
                            Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                            //건설 불가 지역으로 설정
                            isconstructable[i, j] = false;
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
