using System.Collections.Generic;
using System.IO;

namespace DXMeshParser
{
    public class MeshClass
    {
        /* FIELDS */
        protected HeaderChunk headerChunk;
        protected MeshInfoChunk meshInfoChunk;
        protected TrianglesChunk trianglesChunk;
        protected VerticesChunk verticesChunk;
        protected NormalsChunk normalsChunk;
        protected UVsChunk uvsChunk;

        /* PROPERTIES */
        public HeaderChunk HeaderChunk { get { return headerChunk; } set { headerChunk = value; } }
        public MeshInfoChunk MeshInfoChunk { get { return meshInfoChunk; } set { meshInfoChunk = value; } }
        public TrianglesChunk TrianglesChunk { get { return trianglesChunk; } set { trianglesChunk = value; } }
        public VerticesChunk VerticesChunk { get { return verticesChunk; } set { verticesChunk = value; } }
        public NormalsChunk NormalsChunk { get { return normalsChunk; } set { normalsChunk = value; } }
        public UVsChunk UVsChunk { get { return uvsChunk; } set { uvsChunk = value; } }

        /* CONSTRUCTORS */
        public MeshClass(BinaryReader reader)
        {
            Read(reader);
        }

        /* FUNCTIONS */
        public void Read(BinaryReader reader)
        {
            reader.ReadBytes(16);

            headerChunk = new HeaderChunk();
            meshInfoChunk = new MeshInfoChunk(reader);

            if (meshInfoChunk.MeshName.Contains(".nif"))
                return;

            trianglesChunk = new TrianglesChunk(reader, meshInfoChunk.TriangleNum);
            verticesChunk = new VerticesChunk(reader, meshInfoChunk.VertNum);
            normalsChunk = new NormalsChunk(reader, meshInfoChunk.VertNum);
            uvsChunk = new UVsChunk(reader, meshInfoChunk.VertNum);
        }

        public void WriteToObj(ref List<string> file)
        {
            if (meshInfoChunk.MeshName.Contains(".nif"))
                return;

            verticesChunk.WriteToObj(ref file);
            file.Add("");
            uvsChunk.WriteToObj(ref file);
            file.Add("");
            normalsChunk.WriteToObj(ref file);
            file.Add("");
            file.Add(string.Format("g {0}", meshInfoChunk.MeshName));
            file.Add("");
            trianglesChunk.WriteToObj(ref file);
        }

        public override string ToString()
        {
            return string.Format("{0}", meshInfoChunk.MeshName);
        }

    }
}
