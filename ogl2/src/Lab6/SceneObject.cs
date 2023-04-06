using OpenTK;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal class SceneObject
    {
        public string Name { get; private set; }
        public Mesh Mesh { get; private set; }
        public Vector3 Position;
        public Quaternion Rotation;
        public SceneObject(string name)
        {
            Name = name;
            Rotation = new Quaternion();
            Position = new Vector3();

            Mesh = new Mesh()
            {
                Vertices = new float[] {0,0,0,1,0,0, 1,0,0 ,1,0,0, 0,1,0,1,0,0 },
                Indices = new int[] {0,1,2 },
                PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles
            };
        }

    }
}
