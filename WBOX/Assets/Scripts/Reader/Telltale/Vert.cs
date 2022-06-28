using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct Vert
{
    public Flags mFlags;
    public Vector3 mPos;

    public Vert(BinaryReader reader)
    {
        mFlags = new Flags(reader);
        mPos = new Vector3(reader);
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        mFlags.WriteBinaryData(writer);
        mPos.WriteBinaryData(writer);
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += mFlags.GetByteSize(); //mFlags
        totalByteSize += mPos.GetByteSize(); //mPos

        return totalByteSize;
    }
}
