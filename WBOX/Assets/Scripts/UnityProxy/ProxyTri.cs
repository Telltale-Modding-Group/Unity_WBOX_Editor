using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyTri : MonoBehaviour
{
    public EnumMaterial mFootstepMaterial;
    public uint mFlags;
    //public int mNormal;
    public ProxyNormal mNormal;
    public int mQuadBuddy; //references a quad by index
    public float mMaxRadius;

    public List<ProxyVert> mVerts; //SArray<int,3>
    public List<ProxyEdge> mEdgeInfo; //SArray<WalkBoxes::Edge,3>

    public int[] mVertOffsets; //SArray<int,3> 
    public float[] mVertScales; //SArray<float,3>

    public Tri GetResult()
    {
        Tri newTri = new Tri();

        newTri.mFootstepMaterial_BlockSize = 8;
        newTri.mFootstepMaterial = mFootstepMaterial;

        newTri.mFlags = new Flags()
        {
            mFlags = mFlags
        };

        //newTri.mNormal = mNormal;
        newTri.mNormal = mNormal.GetNormalIndex();
        newTri.mQuadBuddy = mQuadBuddy;
        newTri.mMaxRadius = mMaxRadius;

        newTri.mVerts_BlockSize = (uint)(4 + (4 * mVerts.Count));
        newTri.mVerts = new int[mVerts.Count];

        for (int i = 0; i < mVerts.Count; i++)
        {
            newTri.mVerts[i] = mVerts[i].GetVertexIndex();
        }

        newTri.mEdgeInfo_BlockSize = 4;
        newTri.mEdgeInfo = new Edge[mEdgeInfo.Count];

        for (int i = 0; i < mEdgeInfo.Count; i++)
        {
            Edge newEdge = mEdgeInfo[i].GetResult();

            newTri.mEdgeInfo[i] = newEdge;

            newTri.mEdgeInfo_BlockSize += newEdge.GetByteSize();
        }

        newTri.mVertOffsets_BlockSize = (uint)(4 + (4 * mVertOffsets.Length));
        newTri.mVertOffsets = mVertOffsets;

        newTri.mVertScales_BlockSize = (uint)(4 + (4 * mVertScales.Length));
        newTri.mVertScales = mVertScales;

        return newTri;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for(int i = 0; i < mVerts.Count; i++)
        {
            Gizmos.DrawWireSphere(mVerts[i].mPos, 0.15f);
        }
    }
}
