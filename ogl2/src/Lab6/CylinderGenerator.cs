using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class CylinderGenerator : MeshGenerator
    {
        protected int _vertices;
        protected bool _softNormals;
        public CylinderGenerator(int vertices)
        {
            _vertices = vertices;
            _softNormals = true;
        }

        public override MeshGenerator SetNormalsSmoothing(bool enabled) 
        {
            _softNormals = enabled;
            return this;
        }

        public override MeshGenerator SetSteps(int steps)
        {
            _vertices = steps;
            return this;
        }

        public override int GetSteps()
        {
            return _vertices;
        }

        public override void Generate()
        {
            var verts = GenerateSides(1);
            GenerateCaps(verts);
        }

        protected Vector3[] GenerateSides(float radius, bool normalToCenter = false)
        {
            int triangles = _triangles.Count();
            Vector3[] verts = new Vector3[_vertices * 2 + 2];
            for (int i = 0; i < _vertices; i++)
            {
                var angle = 2 * Math.PI * i / _vertices;
                verts[i] = new Vector3((float)Math.Cos(angle) * radius, 1, (float)Math.Sin(angle)* radius);
                verts[i + _vertices] = new Vector3((float)Math.Cos(angle) * radius, -1, (float)Math.Sin(angle)* radius);
            }
            for (int i = 0; i < _vertices; i++)
            {
                var iNext = (i + 1) % _vertices;
                AddQuad(verts[i], verts[iNext], verts[_vertices + iNext], verts[_vertices + i], _color);
            }
            var normalDir = normalToCenter ? -1 : 1;
            if (_softNormals)
            {
                for (int i = triangles; i < _triangles.Count; i++)
                {
                    var triangle = _triangles[i];
                    triangle.v1.Normal = normalDir * new Vector3(triangle.v1.Position.X, 0, triangle.v1.Position.Z).Normalized();
                    triangle.v2.Normal = normalDir * new Vector3(triangle.v2.Position.X, 0, triangle.v2.Position.Z).Normalized();
                    triangle.v3.Normal = normalDir * new Vector3(triangle.v3.Position.X, 0, triangle.v3.Position.Z).Normalized();
                }
            }

            return verts;
        }

        protected void GenerateCaps(Vector3[] verts)
        {
            Vector3[] centers = new Vector3[]{new Vector3(0, 1, 0), new Vector3(0, -1, 0)};

            for (int i = 0; i < _vertices; i++)
            {
                var iNext = (i + 1) % _vertices;
                AddTriangle(centers[0], verts[iNext], verts[i], _color);
                AddTriangle(verts[_vertices + i], verts[_vertices + iNext], centers[1], _color);
            }
        }

    }
}
