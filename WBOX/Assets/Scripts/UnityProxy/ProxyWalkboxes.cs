using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProxyWalkboxes : MonoBehaviour
{
    //meta header object
    public MSV6 msv6;
    public MSV5 msv5;
    public MTRE mtre;

    public string mName { get { return gameObject.name; } }

    [HideInInspector] public GameObject mTris_parent;
    public List<ProxyTri> mTris;

    [HideInInspector] public GameObject mVerts_parent;
    public List<ProxyVert> mVerts;

    [HideInInspector] public GameObject mNormals_parent;
    public List<ProxyNormal> mNormals;

    [HideInInspector] public GameObject mQuads_parent;
    public List<ProxyQuad> mQuads;

    public void ExportWalkbox()
    {
        var path = EditorUtility.SaveFilePanel("Export WBOX", "", mName, "wbox");

        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
        {
            Walkboxes newWalkboxes = GetResult();

            if (msv6 != null)
            {
                msv6.mDefaultSectionChunkSize = (uint)newWalkboxes.GetByteSize();
                msv6.GetByteData(writer);
            }
            else if (msv5 != null)
            {
                msv5.mDefaultSectionChunkSize = (uint)newWalkboxes.GetByteSize();
                msv5.GetByteData(writer);
            }
            else if (mtre != null)
            {
                mtre.GetByteData(writer);
            }

            newWalkboxes.WriteBinaryData(writer);
        }

        EditorUtility.RevealInFinder(path);
    }

    public Walkboxes GetResult()
    {
        Walkboxes newWalkboxes = new Walkboxes();

        newWalkboxes.mName_BlockSize = (uint)(8 + mName.Length); //mName Block Size [4 bytes] //mName block size (size + string len value + string)
        newWalkboxes.mName = mName; //mName [x bytes]

        //---------------------------mTris--------------------------
        newWalkboxes.mTris_Size = mTris.Count; //mTris DCArray Size [4 bytes]
        newWalkboxes.mTris = new Tri[mTris.Count];

        for (int i = 0; i < newWalkboxes.mTris.Length; i++)
        {
            newWalkboxes.mTris[i] = mTris[i].GetResult();
        }

        newWalkboxes.mTris_Capacity = 8;

        for (int i = 0; i < mTris.Count; i++)
        {
            newWalkboxes.mTris_Capacity += mTris[i].GetResult().GetByteSize();
        }

        //---------------------------mVerts--------------------------
        newWalkboxes.mVerts_Size = mVerts.Count; //mVerts DCArray Size [4 bytes]
        newWalkboxes.mVerts = new Vert[mVerts.Count];

        for (int i = 0; i < newWalkboxes.mVerts.Length; i++)
        {
            newWalkboxes.mVerts[i] = mVerts[i].GetResult();
        }

        newWalkboxes.mVerts_Capacity = 8;

        for (int i = 0; i < mVerts.Count; i++)
        {
            newWalkboxes.mVerts_Capacity += mVerts[i].GetResult().GetByteSize();
        }

        //---------------------------mNormals--------------------------
        newWalkboxes.mNormals_Size = mNormals.Count; //mNormals DCArray Size [4 bytes]
        newWalkboxes.mNormals = new Vector3[mNormals.Count];

        for (int i = 0; i < newWalkboxes.mNormals.Length; i++)
        {
            newWalkboxes.mNormals[i] = mNormals[i].GetResult();
        }

        newWalkboxes.mNormals_Capacity = 8;

        for (int i = 0; i < mNormals.Count; i++)
        {
            newWalkboxes.mNormals_Capacity += mNormals[i].GetResult().GetByteSize();
        }

        //---------------------------mQuads--------------------------
        newWalkboxes.mQuads_Size = mQuads.Count; //mNormals DCArray Size [4 bytes]
        newWalkboxes.mQuads = new Quad[mQuads.Count];

        for (int i = 0; i < newWalkboxes.mQuads.Length; i++)
        {
            newWalkboxes.mQuads[i] = mQuads[i].GetResult();
        }

        newWalkboxes.mQuads_Capacity = 8;

        for (int i = 0; i < mQuads.Count; i++)
        {
            newWalkboxes.mQuads_Capacity += mQuads[i].GetResult().GetByteSize();
        }

        return newWalkboxes;
    }
}
