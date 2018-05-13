namespace DXMeshParser
{
    public struct Short3
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
    public struct Float3
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
    public struct Float2
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
}
