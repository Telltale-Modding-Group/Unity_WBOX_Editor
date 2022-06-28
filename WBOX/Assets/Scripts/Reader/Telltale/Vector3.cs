using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct Vector3
{
    public float x;
    public float y;
    public float z;

    public override string ToString() => string.Format("[Vector3] x: {0} y: {1} z: {2}", x, y, z);

    public Vector3(BinaryReader reader)
    {
        x = reader.ReadSingle();
        y = reader.ReadSingle();
        z = reader.ReadSingle();
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(x);
        writer.Write(y);
        writer.Write(z);
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 4; //x;
        totalByteSize += 4; //y;
        totalByteSize += 4; //z;

        return totalByteSize;
    }
}
