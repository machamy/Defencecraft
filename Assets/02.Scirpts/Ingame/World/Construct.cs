using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

namespace _02.Scirpts.Ingame
{

    enum buildingType { WALL, TOWER, SAFE }

    public class Construct : MonoBehaviour
    {

        public bool issConstructMode = true;
        private bool isbuildable = true;

        private int[] building_size = { 2, 2 };

        private GameObject building;
        public GameObject buildingprefab;

        Vector3 pos;
        private void Update()
        {
            //���콺 �������� ConstructPrefab ����
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("���콺 ������");


                //�Ǽ������ ������ �ǹ� ����
                if (issConstructMode)
                {
                    building = Instantiate(buildingprefab);

                    //���� Ŭ�� �Ҷ����� �ʱ�ȭ
                    isbuildable = true;
                }
            }

            //���콺 �巡���� ������ �ǹ��� ���콺 ����ٴϰ� �ϱ�
            if (Input.GetMouseButton(0))
            {
                //���콺 ��ġ �ҷ�����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, 0.2f, 0f)); //���� ��� ��ġ�� y ���̴� 0.2f

                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance))
                    pos = ray.GetPoint(rayDistance);

                //5�� ����θ� �̵��ϰԲ� ����
                pos = new Vector3(Mathf.Round(pos.x / 5) * 5, Mathf.Round(pos.y), Mathf.Round(pos.z / 5) * 5);
                
                Debug.Log(pos);

                //������ �ǹ��� ��ġ ����
                building.transform.position = pos;
            }

            //���콺�� ������
            if(Input.GetMouseButtonUp(0))
            {
                World worldscript = GetComponent<World>();
                Debug.Log(Mathf.RoundToInt(pos.x));

                //��ǥ �ҷ�����
                int tilenum_x = Mathf.RoundToInt(pos.x) / 5;
                int tilenum_z = Mathf.RoundToInt(pos.z) / 5;

                //�ش� ��ǥ�� Ÿ���� �ִٸ� tile ���� �ҷ�����
                if ((tilenum_x >= 0 && tilenum_x + building_size[0] < 21) && (tilenum_z >= 0 && tilenum_z + building_size[1] < 21))
                {
                    for(int i = 0; i < building_size[0]; i++)
                    {
                        for(int j = 0; j < building_size[1]; j++)
                        {
                            //Ÿ������ �޾ƿ���
                            Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                            Debug.Log(tile.IsConstructable);

                            //�Ǽ������� �������� Ȯ��
                            if (!tile.IsConstructable)
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
                else //Ÿ���� ���ٸ� �ı�
                {
                    isbuildable = false;
                }

                //�Ǽ� ���� �Ǻ��� ���ٸ� �Ǽ�
                if (isbuildable)
                {
                    //�Ǽ�

                    //�ӽ÷� �������� �Ǽ� �˸�
                    //Instantiate(buildingprefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab, new Vector3(pos.x + 5, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab, new Vector3(pos.x, pos.y, pos.z + 5), Quaternion.Euler(0, 0, 0));
                    Instantiate(buildingprefab, new Vector3(pos.x + 5, pos.y, pos.z + 5), Quaternion.Euler(0, 0, 0));

                    //�Ǽ� �Ұ� �������� ����
                    for (int i = 0; i < building_size[0]; i++)
                    {
                        for (int j = 0; j < building_size[1]; j++)
                        {
                            //Ÿ������ �޾ƿ���
                            Tile tile = worldscript.GetTile(tilenum_x + i, tilenum_z + j);

                            tile.SetUnConstructable();
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
