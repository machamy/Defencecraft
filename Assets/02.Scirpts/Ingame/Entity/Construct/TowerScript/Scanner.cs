using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Scanner : MonoBehaviour
{
    public float scanRange = 10f;
    public LayerMask targetLayer;
    public Collider[] targets;
    public Transform nearestTarget;

    public void FixedUpdate()
    {
        targets = Physics.OverlapSphere(transform.position, scanRange, targetLayer);
        nearestTarget = GetNearest();
        Debug.Log(targets);
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (Collider target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
