using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyQuad : MonoBehaviour
{
    public List<ProxyVert> mVerts; //SArray<int,4>

    public Quad GetResult()
    {
        Quad newQuad = new Quad();

        newQuad.mVerts = new int[4];

        newQuad.mVerts[0] = mVerts[0].GetVertexIndex();
        newQuad.mVerts[1] = mVerts[1].GetVertexIndex();
        newQuad.mVerts[2] = mVerts[2].GetVertexIndex();
        newQuad.mVerts[3] = mVerts[3].GetVertexIndex();

        return newQuad;
    }
}
