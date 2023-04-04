using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace ogl2
{
    public class Mesh
    {
        public float[] Vertices;
        public int[] Indices;
        public PrimitiveType PrimitiveType;
    }
}
