using System.Collections.Generic;
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

        public override string ToString()
        {
            return string.Format("{0}, {1}", header, version);
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

            unkFloats = new float[6];

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

        public override string ToString()
        {
            return string.Format("{0}, {1}", meshName, meshType);
        }
    }

    public class TrianglesChunk
    {
        /* FIELDS */
        protected Short3[] triangles;

        /* PROPERTIES */
        public Short3[] Triangles { get { return triangles; } set { triangles = value; } }

        /* CONSTRUCTORS */
        public TrianglesChunk(BinaryReader reader, int triangleSize)
        {
            triangles = new Short3[triangleSize];
            Read(reader);
        }

        /* FUNCTIONS */
        public void Read(BinaryReader reader)
        {
            for (int i = 0; i != triangles.Length; i++)
            {
                triangles[i] = new Short3(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
            }
        }

        public void WriteToObj(ref List<string> file)
        {
            for (int i = 0; i != triangles.Length; i++)
            {
                int num1 = triangles[i].s1 + 1;
                int num2 = triangles[i].s2 + 1;
                int num3 = triangles[i].s3 + 1;

                file.Add(string.Format("f {0}/{3}/{6} {1}/{4}/{7} {2}/{5}/{8}", num1, num2, num3, num1, num2, num3, num1, num2, num3));
            }
        }

        public override string ToString()
        {
            return string.Format("Size: {0}", triangles.Length);
        }
    }

    public class VerticesChunk
    {
        /* FIELDS */
        protected Float3[] vertices;

        /* PROPERTIES */
        public Float3[] Vertices { get { return vertices; } set { vertices = value; } }

        /* CONSTRUCTORS */
        public VerticesChunk(BinaryReader reader, int verticesSize)
        {
            vertices = new Float3[verticesSize];
            Read(reader);
        }

        /* FUNCTIONS */
        public void Read(BinaryReader reader)
        {
            for (int i = 0; i != vertices.Length; i++)
            {
                vertices[i] = new Float3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
        }

        public void WriteToObj(ref List<string> file)
        {
            for (int i = 0; i != vertices.Length; i++)
            {
                file.Add(string.Format("v {0} {1} {2}", vertices[i].f1, vertices[i].f2, vertices[i].f3));
            }
        }

        public override string ToString()
        {
            return string.Format("Size: {0}", vertices.Length);
        }
    }

    public class NormalsChunk
    {
        /* FIELDS */
        protected Float3[] normals;

        /* PROPERTIES */
        public Float3[] Normals { get { return normals; } set { normals = value; } }

        /* CONSTRUCTORS */
        public NormalsChunk(BinaryReader reader, int normalSize)
        {
            normals = new Float3[normalSize];
            Read(reader);
        }

        /* FUNCTIONS */
        public void Read(BinaryReader reader)
        {
            for (int i = 0; i != normals.Length; i++)
            {
                normals[i] = new Float3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
        }

        public void WriteToObj(ref List<string> file)
        {
            for (int i = 0; i != normals.Length; i++)
            {
                file.Add(string.Format("vn {0} {1} {2}", normals[i].f1, normals[i].f2, normals[i].f3));
            }
        }

        public override string ToString()
        {
            return string.Format("Size: {0}", normals.Length);
        }
    }

    public class UVsChunk
    {
        /* FIELDS */
        protected Float2[] uvsCoords;

        /* PROPERTIES */
        public Float2[] UVsCoords { get { return uvsCoords; } set { uvsCoords = value; } }

        /* CONSTRUCTORS */
        public UVsChunk(BinaryReader reader, int uvsSize)
        {
            uvsCoords = new Float2[uvsSize];
            Read(reader);
        }

        /* FUNCTIONS */
        public void Read(BinaryReader reader)
        {
            for (int i = 0; i != uvsCoords.Length; i++)
            {
                uvsCoords[i] = new Float2(reader.ReadSingle(), reader.ReadSingle());
            }
        }

        public void WriteToObj(ref List<string> file)
        {
            for (int i = 0; i != uvsCoords.Length; i++)
            {
                file.Add(string.Format("vt {0} {1}", uvsCoords[i].f1, uvsCoords[i].f2));
            }
        }

        public override string ToString()
        {
            return string.Format("Size: {0}", uvsCoords.Length);
        }
    }
}
