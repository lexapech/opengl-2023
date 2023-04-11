using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class StairsGenerator : MeshGenerator
    {
        private int _steps;
        public StairsGenerator(int steps)
        {
            _steps = steps;
        }
        public override void Generate()
        {
            for(int i = -1; i <= _steps; i++)
                GenerateStep(i);

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

        private void GenerateStep(int i)
        {
            var size = 1f / _steps;
            Vector3[] v = new Vector3[]
            {
                new Vector3(0, 0,0),
                new Vector3(0, 0, 1),
                new Vector3(1,  0, 0),
                new Vector3(1,  0, 1),
                new Vector3(0, 1,0),
                new Vector3(0, 1, 1),
                new Vector3(1,  1, 0),
                new Vector3(1,  1, 1),
                new Vector3(0, 2,0),
                new Vector3(0, 2, 1),
                new Vector3(1,  2, 0),
                new Vector3(1,  2, 1),
            };
            for (int j = 0; j < v.Length; j++) v[j] = v[j]*new Vector3(1,size,size) + new Vector3(0,size,size)*i - Vector3.One*0.5f;

            if (i == -1)
            {
                AddQuad(v, new int[] { 8, 9, 11, 10 }, _color);
                AddQuad(v, new int[] { 6, 10, 11, 7 }, _color);
                AddQuad(v, new int[] { 4, 8, 10, 6 }, _color);
                AddQuad(v, new int[] { 9, 8, 4, 5 }, _color);
                AddQuad(v, new int[] { 4, 6, 7, 5 }, _color);
            }
            else if (i == _steps)
            {
                AddQuad(v, new int[] { 0, 2, 3, 1 }, _color);
                AddQuad(v, new int[] { 2, 6, 7, 3 }, _color);
                AddQuad(v, new int[] { 5, 4, 0, 1 }, _color);
                AddQuad(v, new int[] { 7, 5, 1, 3 }, _color);
                AddQuad(v, new int[] { 4, 5, 7, 6 }, _color);
            }
            else
            {
                AddQuad(v, new int[] { 0, 2, 3, 1 }, _color);
                AddQuad(v, new int[] { 4, 8, 10, 6 }, _color);
                AddQuad(v, new int[] { 8, 9, 11, 10 }, _color);
                AddQuad(v, new int[] { 5, 1, 3, 7 }, _color);

                AddQuad(v, new int[] { 6, 10, 11, 7 }, _color);
                AddQuad(v, new int[] { 2, 6, 7, 3 }, _color);
                AddQuad(v, new int[] { 5, 4, 0, 1 }, _color);
                AddQuad(v, new int[] { 9, 8, 4, 5 }, _color);
            }
        }
    }
}
