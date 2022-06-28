using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct Symbol
{
    public ulong mCrc64;

    public override string ToString() => string.Format("[Symbol] mCrc64: {0}", mCrc64);

    public Symbol(BinaryReader reader)
    {
        mCrc64 = reader.ReadUInt64();
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 8; //mCrc64;

        return totalByteSize;
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(mCrc64);
    }
}
