using System.IO;

namespace DXMeshParser
{
    public class HeaderChunk
    {
        /* FIELDS */
        protected string header;
        protected string version;
        protected long padding;

        /* CONSTRUCTORS */
        public HeaderChunk()
        {
            Create();
        }

        /* FUNCTIONS */
        public void Create()
        {
            header = ".3DX";
            version = "V003";
            padding = 0;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(header.ToCharArray());
            writer.Write(version.ToCharArray());
            writer.Write(padding);
        }
    }
    public class MeshInfoChunk
    {
        /* FIELDS */
        protected int vertNum;
        protected int triangleNum;
        protected int unkNum; //Potentially number of textures, or meshes.
        protected int unk32; //unknown int32.
        protected string meshName;
        protected string meshType;
        protected string textureName;
        protected float[] unkFloats;

        /* PROPERTIES */
        public int VertNum { get { return vertNum; } set { vertNum = value; } }
        public int TriangleNum { get { return triangleNum; } set { triangleNum = value; } }
        public string MeshName { get { return meshName; } set { meshName = value; } }
        public string MeshType { get { return meshType; } set { meshType = value; } }
        public string TextureName { get { return textureName; } set { textureName = value; } }

        /* CONSTRUCTORS */
        public MeshInfoChunk(BinaryReader reader)
        {
            Read(reader);
        }

        /* FUNCTIONS */
        public void Read(BinaryReader reader)
        {
            vertNum = reader.ReadInt32();
            triangleNum = reader.ReadInt32();
            unkNum = reader.ReadInt32();
            unk32 = reader.ReadInt32();

            meshName = new string(reader.ReadChars(48)).Trim('\0');
            meshType = new string(reader.ReadChars(32)).Trim('\0');
            textureName = new string(reader.ReadChars(96)).Trim('\0');

            for (int i = 0; i != 6; i++)
                unkFloats[i] = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(vertNum);
            writer.Write(triangleNum);
            writer.Write(unkNum);
            writer.Write(unk32);

            writer.Write(new byte[176]); //TODO:: WRITE STRINGS WITH APPROPRIATE PADDING.

            foreach (float single in unkFloats)
                writer.Write(single);
        }
    }

    public class TrianglesChunk
    {
        /* FUNCTIONS */
        public TrianglesChunk(BinaryReader reader)
        {

        }
    }

    public class VerticesChunk
    {
        /* FUNCTIONS */
        public VerticesChunk(BinaryReader reader)
        {

        }
    }

    public class NormalsChunk
    {
        /* FUNCTIONS */
        public NormalsChunk(BinaryReader reader)
        {

        }
    }

    public class UVsChunk
    {
        /* FUNCTIONS */
        public UVsChunk(BinaryReader reader)
        {

        }
    }
}
