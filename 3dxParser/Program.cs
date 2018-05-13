using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DXMeshParser
{
    struct Short3
    {
        public short s1;
        public short s2;
        public short s3;

        public Short3(short s1, short s2, short s3)
        {
            this.s1 = s1;
            this.s2 = s2;
            this.s3 = s3;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", s1, s2, s3);
        }
    }
    struct Float3
    {
        public float f1;
        public float f2;
        public float f3;

        public Float3(float f1, float f2, float f3)
        {
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", f1, f2, f3);
        }
    }
    struct Float2
    {
        public float f1;
        public float f2;

        public Float2(float f1, float f2)
        {
            this.f1 = f1;
            this.f2 = f2;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", f1, f2);
        }
    }

    class Mesh
    {
        public int vertNum;
        public int triangleNum;
        public int unk03;
        public int unk04;

        public string meshName;
        public string meshType;
        public string textureName;

        public float[] unkFloats;
        public Short3[] triangles;
        public Float3[] vertices;
        public Float3[] normals;
        public Float2[] uvcoords;

        public long section1End;
        public long section2End;
        public long section3End;

        public Mesh(BinaryReader reader)
        {
            reader.ReadBytes(16);

            //begin file.
            vertNum = reader.ReadInt32();
            triangleNum = reader.ReadInt32();
            unk03 = reader.ReadInt32();
            unk04 = reader.ReadInt32();

            //text part
            meshName = new string(reader.ReadChars(48)).Trim('\0');

            //different mesh format.
            if (meshName.Contains(".nif"))
                return;

            meshType = new string(reader.ReadChars(32)).Trim('\0');

            textureName = new string(reader.ReadChars(96)).Trim('\0');

            unkFloats = new float[6];
            triangles = new Short3[triangleNum];
            vertices = new Float3[vertNum];
            normals = new Float3[vertNum];
            uvcoords = new Float2[vertNum];

            //random float part
            for (int i = 0; i != 6; i++)
            {
                unkFloats[i] = reader.ReadSingle();
            }
            //Triangles
            for(int i = 0; i != triangles.Length; i++)
            {
                triangles[i] = new Short3(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
            }
            section1End = reader.BaseStream.Position;
            //Vertices
            for (int i = 0; i != vertices.Length; i++)
            {
                vertices[i] = new Float3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            section2End = reader.BaseStream.Position;

            //Normals
            for (int i = 0; i != normals.Length; i++)
            {
                normals[i] = new Float3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            section3End = reader.BaseStream.Position;

            //UV Coords
            for (int i = 0; i != uvcoords.Length; i++)
            {
                uvcoords[i] = new Float2(reader.ReadSingle(), reader.ReadSingle());
            }


            List<string> objFile = new List<string>();

            for (int i = 0; i != vertices.Length; i++)
            {
                objFile.Add(string.Format("v {0} {1} {2}", vertices[i].f1, vertices[i].f2, vertices[i].f3));
            }
            objFile.Add("");
            for (int i = 0; i != uvcoords.Length; i++)
            {
                objFile.Add(string.Format("vt {0} {1} {2}", uvcoords[i].f1, 1-uvcoords[i].f2, 0));
            }
            objFile.Add("");
            for (int i = 0; i != normals.Length; i++)
            {
                objFile.Add(string.Format("vn {0} {1} {2}", normals[i].f1, normals[i].f2, normals[i].f3, 0));
            }
            objFile.Add("");
            objFile.Add(string.Format("g {0}", meshName));
            for (int i = 0; i != triangles.Length; i++)
            {
                int num1 = triangles[i].s1 + 1;
                int num2 = triangles[i].s2 + 1;
                int num3 = triangles[i].s3 + 1;

                objFile.Add(string.Format("f {0}/{3}/{6} {1}/{4}/{7} {2}/{5}/{8}", num1, num2, num3, num1, num2, num3, num1, num2, num3));
            }

            File.WriteAllLines("Converted/" + meshName + ".obj", objFile);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", meshName, meshType);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Models/");
            List<Mesh> meshes = new List<Mesh>();

            for(int i = 0; i != files.Length; i++)
            {
                using(BinaryReader reader = new BinaryReader(File.Open(files[i], FileMode.Open)))
                {
                    if(reader.BaseStream.Length > 0)
                        meshes.Add(new Mesh(reader));
                }
            }
        }
    }
}
