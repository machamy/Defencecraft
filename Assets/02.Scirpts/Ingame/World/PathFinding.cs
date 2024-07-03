using _02.Scirpts.Ingame;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using System;

public class PathFinding : MonoBehaviour
{
    
    PathRequestManager requestManager;
    World world;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        world = GetComponent<World>();
    }
    
    
    public void StartFindPath(Vector3 startPos, Vector3 tagetPos)
    {
        StartCoroutine(FindPath(startPos, tagetPos));   
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;


        Tile startTile = world.TileFromWorldPoint(startPos);
        Tile targetTile = world.TileFromWorldPoint(targetPos);

        if (startTile.IsWalkable && targetTile.IsConstructed)
        {
            Heap<Tile> openSet = new Heap<Tile>(world.MaxSize);
            HashSet<Tile> closedSet = new HashSet<Tile>();
            openSet.Add(startTile);


            while (openSet.Count > 0)
            {
                Tile currentTile = openSet.RemoveFirst();
                closedSet.Add(currentTile);
                //Debug.Log("closedsetzone = " + currentTile.name.ToString());

                if (currentTile == targetTile)
                {
                    sw.Stop();
                    print("PATH FOUND: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    //Debug.Log("do040 = " + currentTile.name.ToString());

                    yield break;
                }

                foreach(Tile neighbour in world.GetNeighbours(currentTile))
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                    {
                        //Debug.Log("neightbour isn't here");

                        continue;
                    }
                    //Debug.Log("do141 = " + currentTile.name.ToString());
                    int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetTile);
                        neighbour.Parent = currentTile;

                        if (!openSet.Contains(neighbour))
                        {
                            //Debug.Log("last openset adding = " + neighbour.name.ToString());
                            openSet.Add(neighbour);
                        }

                    
                    }
                }
            }
        }
        
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startTile, targetTile);
            requestManager.FinishedProcessingPath(waypoints, pathSuccess); 

        }
    }

    Vector3[] RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
            //Debug.Log(currentTile.ToString());
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        
        

    }
    Vector3[] SimplifyPath(List<Tile> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionold = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].getTileX() - path[i].getTileX(), path[i - 1].getTileY() - path[i].getTileY());
            if(directionNew != directionold)
            {
                waypoints.Add(path[i].transform.position);
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
