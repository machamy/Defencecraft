using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static bool _isThief;
    static PathRequestManager instance;
    PathFinding pathfinding;

    bool isProcessingPath = false;

    void Start()
    {
        instance = this;

        print(instance.name);

        pathfinding = GetComponent<PathFinding>();
    }
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback, bool IsThief)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        _isThief = IsThief;
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext(IsThief);
    }

    void TryProcessNext(bool IsThief)
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, IsThief);

        }
    }
    
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;

        TryProcessNext(_isThief);
    }
    struct PathRequest { 
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
