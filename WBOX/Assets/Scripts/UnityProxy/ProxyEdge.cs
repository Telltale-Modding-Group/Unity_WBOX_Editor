using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEdge : MonoBehaviour
{
    public ProxyVert mV1;
    public ProxyVert mV2;

    public int mEdgeDest;
    public int mEdgeDestEdge;
    public int mEdgeDir;
    public float mMaxRadius;

    public Edge GetResult()
    {
        Edge newEdge = new Edge();

        newEdge.mV1 = mV1.GetVertexIndex();
        newEdge.mV2 = mV2.GetVertexIndex();

        newEdge.mEdgeDest = mEdgeDest;
        newEdge.mEdgeDestEdge = mEdgeDestEdge;
        newEdge.mEdgeDir = mEdgeDir;
        newEdge.mMaxRadius = mMaxRadius;

        return newEdge;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(mV1.mPos, mV2.mPos);
    }
}
