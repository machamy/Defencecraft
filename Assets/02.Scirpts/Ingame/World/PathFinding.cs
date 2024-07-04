using _02.Scirpts.Ingame;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class PathFinding : MonoBehaviour
{
    
    PathRequestManager requestManager;
    World world;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        world = GetComponent<World>();
    }
    
    
    public void StartFindPath(Vector3 startPos, Vector3 tagetPos, bool isThief)
    {
        StartCoroutine(FindPath(startPos, tagetPos, isThief));   
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, bool isThief)
    {
        Stopwatch sw = new Stopwatch();
        //sw.Start();
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        //startpos와 targetpos의 y값을 동일하게 맞춰줘야 한다.
        Tile startTile = world.TileFromWorldPoint(startPos);
        Tile targetTile = world.TileFromWorldPoint(targetPos);

        //##################################
        //여기 무조건 해제해줘야됨, 테스트 중이라 넣어둔거//
        //iswalkable도 맵 외부에서 온다면 제외해도 됨//
        //##################################
        if (startTile.IsWalkable /*&& targetTile.IsConstructed*/)
        {
            Heap<Tile> openSet = new Heap<Tile>(world.MaxSize);
            HashSet<Tile> closedSet = new HashSet<Tile>();
            openSet.Add(startTile);

            while (openSet.Count > 0)
            {
                Tile currentTile = openSet.RemoveFirst();
                
                closedSet.Add(currentTile);

                if (currentTile == targetTile)
                {
                    //sw.Stop();
                    //print("PATH FOUND: " + sw.ElapsedMilliseconds + " ms");

                    pathSuccess = true;
                    break;
                }

                foreach(Tile neighbour in world.GetNeighbours(currentTile))
                {
                    // 건물이 있거나 클로즈셋에 이미 포함되거나 시프가 아닌 경우 패스(시프는 건물을 뛰어넘을 수 있음)
                    if ((neighbour.IsConstructed || closedSet.Contains(neighbour)) && !isThief)
                    {
                        continue;
                    }

                    // obstacle인 경우 시프도 못 건너기에 이차거름망
                    if(!neighbour.IsObstacle)
                    {
                        int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetTile);
                            neighbour.Parent = currentTile;

                            if (!openSet.Contains(neighbour))
                            {
                                //UnityEngine.Debug.Log("last openset adding = " + neighbour.name.ToString());
                                openSet.Add(neighbour);
                            }
                        }
                    }
                    
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startTile, targetTile);

            //way의 높이 일정하게 유지
            for(int i = 0; i <waypoints.Length; i++)
            {
                waypoints[i].y = startPos.y;
            }
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        yield return null;
    }

    Vector3[] RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;
        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
            print(currentTile.name);
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
        
        

    }
    Vector3[] SimplifyPath(List<Tile> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionold = Vector2.zero;

        //reverse전이기에 끝점으로 이동하는 것을 방지하기 위해 2에서 시작
        for (int i = 2; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].getTileX() - path[i].getTileX(), path[i - 1].getTileY() - path[i].getTileY());
            if(directionNew != directionold)
            {
                waypoints.Add(path[i-1].transform.position);
                print(path[i-1].transform.position.ToString());
            }
            directionold = directionNew;
        }
        return waypoints.ToArray();
    }
    

    int GetDistance(Tile TielA, Tile TileB)
    {
        int dstX = Mathf.Abs(TielA.getTileX() - TileB.getTileX());
        int dstY = Mathf.Abs(TielA.getTileY() - TileB.getTileY());

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
