using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This is a class name struct used in a meta header.
/// This contains a CRC64'd string of a class name used in the file.
/// The CRC64 string of a classname is usually all lowercase, and uses 
/// </summary>
[Serializable]
public struct ClassNames
{
    public Symbol mTypeNameCRC;
    public uint mVersionCRC;

    public override string ToString() => string.Format("[ClassNames] mTypeNameCRC: ({0}) mVersionCRC: {1}", mTypeNameCRC, mVersionCRC);

    public ClassNames(BinaryReader reader)
    {
        mTypeNameCRC = new Symbol(reader);
        mVersionCRC = reader.ReadUInt32();
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += mTypeNameCRC.GetByteSize(); //mTypeNameCRC
        totalByteSize += 4; //mVersionCRC

        return totalByteSize;
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        mTypeNameCRC.WriteBinaryData(writer);
        writer.Write(mVersionCRC);
    }
}
