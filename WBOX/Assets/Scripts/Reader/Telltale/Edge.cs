using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct Edge
{
    public int mV1;
    public int mV2;
    public int mEdgeDest;
    public int mEdgeDestEdge;
    public int mEdgeDir;
    public float mMaxRadius;

    public Edge(BinaryReader reader)
    {
        mV1 = reader.ReadInt32();
        mV2 = reader.ReadInt32();
        mEdgeDest = reader.ReadInt32();
        mEdgeDestEdge = reader.ReadInt32();
        mEdgeDir = reader.ReadInt32();
        mMaxRadius = reader.ReadSingle();
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(mV1);
        writer.Write(mV2);
        writer.Write(mEdgeDest);
        writer.Write(mEdgeDestEdge);
        writer.Write(mEdgeDir);
        writer.Write(mMaxRadius);
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 4; //mV1;
        totalByteSize += 4; //mV2;
        totalByteSize += 4; //mEdgeDest;
        totalByteSize += 4; //mEdgeDestEdge;
        totalByteSize += 4; //mEdgeDir;
        totalByteSize += 4; //mMaxRadius;

        return totalByteSize;
    }
}
