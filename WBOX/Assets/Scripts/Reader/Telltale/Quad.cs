using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct Quad
{
    public int[] mVerts; //SArray<int,4>

    public Quad(BinaryReader reader)
    {
        mVerts = new int[4];
        mVerts[0] = reader.ReadInt32(); // [4 BYTES]
        mVerts[0] = reader.ReadInt32(); // [4 BYTES]
        mVerts[0] = reader.ReadInt32(); // [4 BYTES]
        mVerts[0] = reader.ReadInt32(); // [4 BYTES]
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        for(int i = 0; i < mVerts.Length; i++)
        {
            writer.Write(mVerts[i]);
        }
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 4; //mVerts[1];
        totalByteSize += 4; //mVerts[2];
        totalByteSize += 4; //mVerts[3];
        totalByteSize += 4; //mVerts[4];

        return totalByteSize;
    }
}
