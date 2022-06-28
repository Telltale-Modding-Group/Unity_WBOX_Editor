using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
NOTE:
Any variables with a star symbol *
Are not actually in the original data struct, but are serialized in the file.
However, they are just as important as the data itself.
*/

[Serializable]
public struct Tri
{
    public uint mFootstepMaterial_BlockSize; //* (always 8)
    public EnumMaterial mFootstepMaterial;

    public Flags mFlags;
    public int mNormal;
    public int mQuadBuddy; //references a quad by index
    public float mMaxRadius;

    public uint mVerts_BlockSize; //* (always 16)
    public int[] mVerts; //SArray<int,3>

    public uint mEdgeInfo_BlockSize; //* (always 76)
    public Edge[] mEdgeInfo; //SArray<WalkBoxes::Edge,3>

    public uint mVertOffsets_BlockSize; //* (always 16)
    public int[] mVertOffsets; //SArray<int,3> 

    public uint mVertScales_BlockSize; //* (always 16)
    public float[] mVertScales; //SArray<float,3>

    public Tri(BinaryReader reader)
    {
        mFootstepMaterial_BlockSize = reader.ReadUInt32(); //[4 BYTES] ALWAYS 8
        mFootstepMaterial = (EnumMaterial)reader.ReadInt32(); // [4 BYTES]
        mFlags = new Flags(reader); // [4 BYTES]
        mNormal = reader.ReadInt32(); // [4 BYTES]
        mQuadBuddy = reader.ReadInt32(); // [4 BYTES]
        mMaxRadius = reader.ReadSingle(); // [4 BYTES]

        mVerts_BlockSize = reader.ReadUInt32(); //[4 BYTES] ALWAYS 16
        mVerts = new int[3];
        mVerts[0] = reader.ReadInt32();
        mVerts[1] = reader.ReadInt32();
        mVerts[2] = reader.ReadInt32();

        mEdgeInfo_BlockSize = reader.ReadUInt32(); //[4 BYTES] ALWAYS 76
        mEdgeInfo = new Edge[3]; //SArray<WalkBoxes::Edge,3> [72 BYTES]
        mEdgeInfo[0] = new Edge(reader); //[24 BYTES]
        mEdgeInfo[1] = new Edge(reader); //[24 BYTES]
        mEdgeInfo[2] = new Edge(reader);  //[24 BYTES]

        mVertOffsets_BlockSize = reader.ReadUInt32(); //[4 BYTES] ALWAYS 16
        mVertOffsets = new int[3];
        mVertOffsets[0] = reader.ReadInt32();
        mVertOffsets[1] = reader.ReadInt32();
        mVertOffsets[2] = reader.ReadInt32();

        mVertScales_BlockSize = reader.ReadUInt32(); //[4 BYTES] ALWAYS 16
        mVertScales = new float[3];
        mVertScales[0] = reader.ReadSingle();
        mVertScales[1] = reader.ReadSingle();
        mVertScales[2] = reader.ReadSingle();
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 4; //mFootstepMaterial_BlockSize [4 BYTES]
        totalByteSize += 4; //mFootstepMaterial [4 BYTES]
        totalByteSize += mFlags.GetByteSize(); //mFlags [4 BYTES]
        totalByteSize += 4; //mNormal [4 BYTES]
        totalByteSize += 4; //mQuadBuddy [4 BYTES]
        totalByteSize += 4; //mMaxRadius [4 BYTES]

        //mVerts SArray<int,3> [12 BYTES]
        totalByteSize += 4; //mVerts_BlockSize [4 BYTES] 
        totalByteSize += 4;
        totalByteSize += 4;
        totalByteSize += 4;

        totalByteSize += 4; //mEdgeInfo_BlockSize [4 BYTES]
        totalByteSize += mEdgeInfo[0].GetByteSize();
        totalByteSize += mEdgeInfo[0].GetByteSize();
        totalByteSize += mEdgeInfo[0].GetByteSize();

        //mVertOffsets SArray<int,3> [12 BYTES]
        totalByteSize += 4; //mVertOffsets_BlockSize [4 BYTES]
        totalByteSize += 4;
        totalByteSize += 4;
        totalByteSize += 4;

        //mVertScales SArray<float,3> [12 BYTES]
        totalByteSize += 4; //mVertScales_BlockSize [4 BYTES]
        totalByteSize += 4;
        totalByteSize += 4;
        totalByteSize += 4;

        return totalByteSize;
    }
}
