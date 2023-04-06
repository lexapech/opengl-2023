using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    abstract class MeshGenerator
    {
        protected class Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector4 Color;
        }
        protected class Triangle
        {
            public Vertex v1;
            public Vertex v2;
            public Vertex v3;
        }

        protected List<Triangle> _triangles;
        protected Vector4 _color;

        abstract public void Generate();

        public MeshGenerator SetColor(Color color)
        {
            _triangles = new List<Triangle>();
            _color = Utility.ConvertColor(color);
            return this;
        }
      
        public Mesh GetMesh()
        {
            Generate();
            var mesh = new Mesh();
            List<float> data = new List<float>();
            foreach (Triangle t in _triangles)
            {
                data.Add(t.v1.Position.X);
                data.Add(t.v1.Position.Y);
                data.Add(t.v1.Position.Z);
                data.Add(t.v1.Normal.X);
                data.Add(t.v1.Normal.Y);
                data.Add(t.v1.Normal.Z);
                data.Add(t.v1.Color.X);
                data.Add(t.v1.Color.Y);
                data.Add(t.v1.Color.Z);
                data.Add(t.v1.Color.W);

                data.Add(t.v2.Position.X);
                data.Add(t.v2.Position.Y);
                data.Add(t.v2.Position.Z);
                data.Add(t.v2.Normal.X);
                data.Add(t.v2.Normal.Y);
                data.Add(t.v2.Normal.Z);
                data.Add(t.v2.Color.X);
                data.Add(t.v2.Color.Y);
                data.Add(t.v2.Color.Z);
                data.Add(t.v2.Color.W);

                data.Add(t.v3.Position.X);
                data.Add(t.v3.Position.Y);
                data.Add(t.v3.Position.Z);
                data.Add(t.v3.Normal.X);
                data.Add(t.v3.Normal.Y);
                data.Add(t.v3.Normal.Z);
                data.Add(t.v3.Color.X);
                data.Add(t.v3.Color.Y);
                data.Add(t.v3.Color.Z);
                data.Add(t.v3.Color.W);
            }
            mesh.Vertices = data.ToArray();
            mesh.PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
            mesh.Indices = Enumerable.Range(0, _triangles.Count * 3).ToArray();
            return mesh;
        }

        protected void AddTriangle(Vertex v1,Vertex v2,Vertex v3)
        {
            _triangles.Add(new Triangle()
            {
                v1 = v1,
                v2 = v2,
                v3 = v3
            });
        }

        protected void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector4 color)
        {
            AddTriangle(v1, v2, v3, color);
            AddTriangle(v1, v3, v4, color);
        }

        protected void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector4 color)
        {
            var normal = Vector3.Cross(v2 - v1, v3 - v1).Normalized();
            var vert1 = new Vertex
            {
                Position = v1,
                Normal = normal,
                Color = color
            };
            var vert2 = new Vertex
            {
                Position = v2,
                Normal = normal,
                Color = color
            };
            var vert3 = new Vertex
            {
                Position = v3,
                Normal = normal,
                Color = color
            };
            AddTriangle(vert1, vert2, vert3);         
        }

    }
}
