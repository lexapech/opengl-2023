using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class SphereGenerator : MeshGenerator
    {
        private int _steps;
        public SphereGenerator(int steps)
        {
            _steps = steps;
        }

        public override MeshGenerator SetSteps(int steps)
        {
            _steps = steps;
            return this;
        }

        public override int GetSteps()
        {
            return _steps;
        }

        public override void Generate()
        {
            for(int u = 0; u < _steps; u++)
            {
                for (int v = 0; v < _steps; v++)
                {
                    var size = 2f / _steps;
                    var p1 = size * u - 1f;
                    var p2 = size * v - 1f;
                    AddQuad(new Vector3(p1, p2, 1), new Vector3(p1 + size, p2, 1), new Vector3(p1 + size, p2 + size, 1), new Vector3(p1, p2 + size, 1), _color);
                    AddQuad(new Vector3(p1, p2 + size, -1), new Vector3(p1 + size, p2 + size, -1), new Vector3(p1 + size, p2, -1),new Vector3(p1, p2, -1), _color);

                    AddQuad(new Vector3(p1,-1, p2), new Vector3(p1 + size, -1, p2), new Vector3(p1 + size, -1, p2 + size), new Vector3(p1, -1, p2 + size), _color);
                    AddQuad(new Vector3(p1, 1, p2 + size), new Vector3(p1 + size, 1, p2 + size), new Vector3(p1 + size, 1, p2), new Vector3(p1, 1, p2), _color);

                    AddQuad(new Vector3(1,p1, p2), new Vector3(1,p1 + size, p2), new Vector3(1,p1 + size, p2 + size), new Vector3(1,p1, p2 + size), _color);
                    AddQuad(new Vector3(-1,p1, p2 + size), new Vector3(-1,p1 + size, p2 + size), new Vector3(-1,p1 + size, p2), new Vector3(-1, p1, p2), _color);
                }
            }

            for(int i = 0; i < _triangles.Count; i++)
            {
                var triangle = _triangles[i];
                triangle.v1.Position = triangle.v1.Normal = new Vector3(triangle.v1.Position.X, triangle.v1.Position.Y, triangle.v1.Position.Z).Normalized();
                triangle.v2.Position = triangle.v2.Normal = new Vector3(triangle.v2.Position.X, triangle.v2.Position.Y, triangle.v2.Position.Z).Normalized();
                triangle.v3.Position = triangle.v3.Normal = new Vector3(triangle.v3.Position.X, triangle.v3.Position.Y, triangle.v3.Position.Z).Normalized();
            }
            
        }
    }
}
