using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

public static class ConvertStructs
{
    /*
    public static int[] Get_IntArray(int length, BinaryReader reader)
    {
        int[] intArray = new int[length];

        for (int i = 0; i < length; i++)
        {
            intArray[i] = reader.ReadInt32(); // [4 BYTES]
        }

        return intArray;
    }

    public static float[] Get_FloatArray(int length, BinaryReader reader)
    {
        float[] floatArray = new float[length];

        for (int i = 0; i < length; i++)
        {
            floatArray[i] = reader.ReadSingle(); // [4 BYTES]
        }

        return floatArray;
    }

    public static Tri GetNewTri()
    {
        return new Tri()
        {
            mEdgeInfo_BlockSize = 76,
            mEdgeInfo = new Edge[3]
            {
                new Edge(),
                new Edge(),
                new Edge()
            },
            mMaxRadius = 100000.0f,
            mQuadBuddy = -1,
            mFlags = new Flags(),
            mFootstepMaterial_BlockSize = 8,
            mFootstepMaterial = EnumMaterial.Default,
            mNormal = 0,
            mVertOffsets_BlockSize = 16,
            mVertScales_BlockSize = 16,
            mVerts_BlockSize = 16,
            mVertOffsets = new int[3],
            mVerts = new int[3],
            mVertScales = new float[3]
            {
                1.0f,
                1.0f,
                1.0f
            }
        };
    }
    */
}