using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProxyVert : MonoBehaviour
{
    public uint mFlags;
    public UnityEngine.Vector3 mPos { get { return transform.position; } }

    public Vert GetResult()
    {
        Vert newVert = new Vert();

        newVert.mFlags = new Flags()
        { 
            mFlags = mFlags
        };

        newVert.mPos = new Vector3()
        {
            x = mPos.x,
            y = mPos.y,
            z = mPos.z
        };

        return newVert;
    }

    public int GetVertexIndex()
    {
        return transform.GetSiblingIndex();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.05f);

        //Handles.Label(transform.position, transform.name);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.Label(transform.position, transform.name);
    }
}
