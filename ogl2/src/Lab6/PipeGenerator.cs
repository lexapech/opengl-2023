using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class PipeGenerator : CylinderGenerator
    {
        protected float _innerRadius;
        public PipeGenerator(int vertices,float innerRadius) : base(vertices)
        {
            _innerRadius = innerRadius;
        }
    
        public override void Generate()
        {
            var verts = GenerateSides(1);
            var innerverts = GenerateSides(Math.Min(_innerRadius,1),true);
            GenerateCaps(verts, innerverts);
        }

        private void GenerateCaps(Vector3[] outer,Vector3[] inner)
        {
            for (int i = 0; i < _vertices; i++)
            {
                var iNext = (i + 1) % _vertices;
                AddQuad(inner[i], inner[iNext], outer[iNext], outer[i],_color);
                AddQuad(outer[i+ _vertices], outer[iNext+ _vertices], inner[iNext + _vertices], inner[i+ _vertices], _color);
            }
        }


    }
}
