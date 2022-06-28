using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// Main walkboxes object.
/// </summary>
public class Walkboxes
{
    public uint mName_BlockSize;
    public string mName;

    public uint mTris_Capacity;
    public int mTris_Size;
    public Tri[] mTris; //DCArray<WalkBoxes::Tri>

    public uint mVerts_Capacity;
    public int mVerts_Size;
    public Vert[] mVerts; //DCArray<WalkBoxes::Vert>

    public uint mNormals_Capacity;
    public int mNormals_Size;
    public Vector3[] mNormals; //DCArray<Vector3>

    public uint mQuads_Capacity;
    public int mQuads_Size;
    public Quad[] mQuads; //DCArray<WalkBoxes::Quad>

    public Walkboxes () { }

    public Walkboxes(BinaryReader reader)
    {
        mName_BlockSize = reader.ReadUInt32(); //mName Block Size [4 bytes] //mName block size (size + string len)
        mName = ByteFunctions.ReadString(reader); //mName [x bytes]

        //---------------------------mTris--------------------------
        mTris_Capacity = reader.ReadUInt32(); //mTris DCArray Capacity [4 bytes]
        mTris_Size = reader.ReadInt32(); //mTris DCArray Size [4 bytes]
        mTris = new Tri[mTris_Size];

        for (int i = 0; i < mTris.Length; i++)
        {
            mTris[i] = new Tri(reader); //[148 BYTES]
        }

        //---------------------------mVerts--------------------------
        mVerts_Capacity = reader.ReadUInt32(); //mVerts DCArray Capacity [4 bytes]
        mVerts_Size = reader.ReadInt32(); //mVerts DCArray Size [4 bytes]
        mVerts = new Vert[mVerts_Size];

        for (int i = 0; i < mVerts.Length; i++)
        {
            mVerts[i] = new Vert(reader);
        }

        //---------------------------mNormals--------------------------
        mNormals_Capacity = reader.ReadUInt32(); //mNormals DCArray Capacity [4 bytes]
        mNormals_Size = reader.ReadInt32(); //mNormals DCArray Size [4 bytes]
        mNormals = new Vector3[mNormals_Size];

        for (int i = 0; i < mNormals.Length; i++)
        {
            mNormals[i] = new Vector3(reader);
        }

        //---------------------------mQuads--------------------------
        mQuads_Capacity = reader.ReadUInt32(); //mQuads DCArray Capacity [4 bytes]
        mQuads_Size = reader.ReadInt32(); //mQuads DCArray Size [4 bytes]
        mQuads = new Quad[mQuads_Size];

        for (int i = 0; i < mQuads.Length; i++)
        {
            mQuads[i] = new Quad(reader);
        }
    }

    /// <summary>
    /// Converts the data of this object into a byte array.
    /// </summary>
    /// <returns></returns>
    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
        ByteFunctions.WriteString(writer, mName); //mName [x bytes]

        //---------------------------mTris--------------------------
        writer.Write(mTris_Capacity); //mTris DCArray Capacity [4 bytes]
        writer.Write(mTris_Size); //mTris DCArray Size [4 bytes]

        for (int i = 0; i < mTris.Length; i++)
        {
            Tri mTri = mTris[i];

            writer.Write(mTri.mFootstepMaterial_BlockSize); //[4 BYTES] ALWAYS 8
            writer.Write((int)mTri.mFootstepMaterial); // [4 BYTES]
            mTri.mFlags.WriteBinaryData(writer); // [4 BYTES]
            writer.Write(mTri.mNormal); // [4 BYTES]
            writer.Write(mTri.mQuadBuddy); // [4 BYTES]
            writer.Write(mTri.mMaxRadius); // [4 BYTES]
            writer.Write(mTri.mVerts_BlockSize); //[4 BYTES] ALWAYS 16

            //SArray<int,3> [12 BYTES]
            writer.Write(mTri.mVerts[0]);
            writer.Write(mTri.mVerts[1]);
            writer.Write(mTri.mVerts[2]);

            writer.Write(mTri.mEdgeInfo_BlockSize); //[4 BYTES] ALWAYS 76

            //SArray<WalkBoxes::Edge,3> [72 BYTES]
            mTri.mEdgeInfo[0].WriteBinaryData(writer);
            mTri.mEdgeInfo[1].WriteBinaryData(writer);
            mTri.mEdgeInfo[2].WriteBinaryData(writer);

            writer.Write(mTri.mVertOffsets_BlockSize); //[4 BYTES] ALWAYS 16

            //SArray<int,3> [12 BYTES]
            writer.Write(mTri.mVertOffsets[0]);
            writer.Write(mTri.mVertOffsets[1]);
            writer.Write(mTri.mVertOffsets[2]);

            writer.Write(mTri.mVertScales_BlockSize); //[4 BYTES] ALWAYS 16

            //SArray<float,3> [12 BYTES]
            writer.Write(mTri.mVertScales[0]);
            writer.Write(mTri.mVertScales[1]);
            writer.Write(mTri.mVertScales[2]);
        }

        //---------------------------mVerts--------------------------
        writer.Write(mVerts_Capacity); //mVerts DCArray Capacity [4 bytes]
        writer.Write(mVerts_Size); //mVerts DCArray Size [4 bytes]

        for (int i = 0; i < mVerts.Length; i++)
        {
            mVerts[i].WriteBinaryData(writer);
        }

        //---------------------------mNormals--------------------------
        writer.Write(mNormals_Capacity); //mNormals DCArray Capacity [4 bytes]
        writer.Write(mNormals_Size); //mNormals DCArray Size [4 bytes]

        for (int i = 0; i < mNormals.Length; i++)
        {
            mNormals[i].WriteBinaryData(writer);
        }

        //---------------------------mQuads--------------------------
        writer.Write(mQuads_Capacity); //mQuads DCArray Capacity [4 bytes]
        writer.Write(mQuads_Size); //mQuads DCArray Size [4 bytes]

        for (int i = 0; i < mQuads.Length; i++)
        {
            mQuads[i].WriteBinaryData(writer);
        }
    }
    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 4; //mName Block Size [4 bytes] //mName block size (size + string len)
        totalByteSize += 4; //mName Length [4 bytes]
        totalByteSize += (uint)mName.Length; //mName [x bytes]

        //---------------------------mTris--------------------------
        totalByteSize += 4; //mTris DCArray Capacity [4 bytes]
        totalByteSize += 4; //mTris DCArray Size [4 bytes]

        for(int i = 0; i < mTris.Length; i++)
        {
            totalByteSize += mTris[i].GetByteSize();
        }

        //---------------------------mVerts--------------------------
        totalByteSize += 4; //mVerts DCArray Capacity [4 bytes]
        totalByteSize += 4; //mVerts DCArray Size [4 bytes]

        for (int i = 0; i < mVerts.Length; i++)
        {
            totalByteSize += mVerts[i].GetByteSize();
        }

        //---------------------------mNormals--------------------------
        totalByteSize += 4; //mNormals DCArray Capacity [4 bytes]
        totalByteSize += 4; //mNormals DCArray Size [4 bytes]

        for (int i = 0; i < mNormals.Length; i++)
        {
            totalByteSize += mNormals[i].GetByteSize();
        }

        //---------------------------mQuads--------------------------
        totalByteSize += 4; //mQuads DCArray Capacity [4 bytes]
        totalByteSize += 4; //mQuads DCArray Size [4 bytes]

        for (int i = 0; i < mQuads.Length; i++)
        {
            totalByteSize += mQuads[i].GetByteSize();
        }

        return totalByteSize;
    }
}
