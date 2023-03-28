using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace ogl2
{
    internal class Surface
    {
        public class SurfaceMesh
        {
            public float[] Vertices;
            public int[] Indices;
        }

        public SurfaceMesh Mesh;
        private float _size = 1;
        private int _resolution;
        private Spline _spline;
        public Vector3 Point;
        public Vector2 CameraAngle;
        public float CameraDistance;
       
        

        public int Resolution {get {return _resolution;}}
        public float Size {get {return _size;}}
        public Surface(Spline spline)
        {
            _spline = spline;          
        }

        public void Generate()
        {
            _spline.Generate();
            _resolution = _spline.Vertices.Count;
            Mesh = new SurfaceMesh
            {
                Vertices = new float[_resolution * _resolution * 6],
                Indices = new int[(_resolution - 1) * (_resolution - 1) * 4]
            };
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    var vertexPos = new Vector3(_spline.Vertices[x].X, _spline.Vertices[x].Y, (float)y/_resolution) * _size;
                    var index = x + y * _resolution;
                    Mesh.Vertices[index * 6] = vertexPos.X;
                    Mesh.Vertices[index * 6 + 1] = vertexPos.Y;
                    Mesh.Vertices[index * 6 + 2] = vertexPos.Z - _size/2;
                    var prev = x > 0 ? _spline.Vertices[x - 1] : _spline.Vertices[x];
                    var next = (x < _resolution - 1) ? _spline.Vertices[x + 1] : _spline.Vertices[x];
                    var normal =Vector3.Cross(Vector3.UnitZ, new Vector3(next - prev).Normalized());
                    Mesh.Vertices[index * 6 + 3] = normal.X;
                    Mesh.Vertices[index * 6 + 4] = normal.Y;
                    Mesh.Vertices[index * 6 + 5] = 0;

                    var quadIndex = x + y * (_resolution - 1);

                    if (x < _resolution - 1 && y < _resolution - 1)
                    {
                        Mesh.Indices[quadIndex * 4] = index;
                        Mesh.Indices[quadIndex * 4 + 1] = index + 1;
                        Mesh.Indices[quadIndex * 4 + 2] = index + _resolution + 1;
                        Mesh.Indices[quadIndex * 4 + 3] = index + _resolution;
                    }
                }
            }
        }

    }
}
