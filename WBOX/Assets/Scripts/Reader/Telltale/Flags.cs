using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct Flags
{
    public uint mFlags;

    public Flags(BinaryReader reader)
    {
        mFlags = reader.ReadUInt32();
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(mFlags);
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 4; //mFlags;

        return totalByteSize;
    }
}
