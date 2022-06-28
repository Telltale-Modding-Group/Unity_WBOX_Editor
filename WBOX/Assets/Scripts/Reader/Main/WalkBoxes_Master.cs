using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class WalkBoxes_Master
{
    //meta header object
    public MSV6 msv6;
    public MSV5 msv5;
    public MTRE mtre;

    //main walkbox object
    public Walkboxes walkboxes;

    public WalkBoxes_Master(string filePath)
    {
        string fileMetaVersion = Read_MetaStreamKeyword(filePath);

        using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
        {
            switch (fileMetaVersion)
            {
                case "6VSM":
                    msv6 = new MSV6(reader);
                    break;
                case "5VSM":
                    msv5 = new MSV5(reader);
                    break;
                case "ERTM":
                    mtre = new MTRE(reader);
                    break;
            }

            walkboxes = new Walkboxes(reader);
        }
    }

    public static string Read_MetaStreamKeyword(string sourceFile)
    {
        string metaStreamVersion = "";

        using (BinaryReader reader = new BinaryReader(File.OpenRead(sourceFile)))
        {
            metaStreamVersion += reader.ReadChar();
            metaStreamVersion += reader.ReadChar();
            metaStreamVersion += reader.ReadChar();
            metaStreamVersion += reader.ReadChar();
        }

        return metaStreamVersion;
    }

    public object Get_Meta_Object()
    {
        if (msv6 != null)
            return msv6;
        else if (msv5 != null)
            return msv5;
        else if (mtre != null)
            return mtre;
        else
            return null;
    }
}