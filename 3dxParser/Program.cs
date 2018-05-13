using System;
using System.Collections.Generic;
using System.IO;

namespace DXMeshParser
{  
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Models/");
            List<MeshClass> meshes = new List<MeshClass>();

            for (int i = 0; i != files.Length; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(files[i], FileMode.Open)))
                {
                    Console.WriteLine("{0}", files[i]);
                    if (reader.BaseStream.Length > 0)
                        meshes.Add(new MeshClass(reader));
                }
            }

            foreach(MeshClass mesh in meshes)
            {
                List<string> file = new List<string>();
                mesh.WriteToObj(ref file);
                File.WriteAllLines("Converted/" + mesh.MeshInfoChunk.MeshName + ".obj", file);
            }
        }
    }
}
