using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            headerChunk = new HeaderChunk();
            meshInfoChunk = new MeshInfoChunk(reader);
        }

    }
}
