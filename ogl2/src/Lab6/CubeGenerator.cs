using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class CubeGenerator : MeshGenerator
    {
        public CubeGenerator() 
        {
        }
        public override void Generate()
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(-1,-1,-1),
                new Vector3(-1,-1,1),
                new Vector3(-1,1,-1),
                new Vector3(-1,1,1),
                new Vector3(1,-1,-1),
                new Vector3(1,-1,1),
                new Vector3(1,1,-1),
                new Vector3(1,1,1),
            };

            AddQuad(verts[0], verts[1], verts[3], verts[2], Utility.ConvertColor(Color.Red));
            AddQuad(verts[2], verts[6], verts[4], verts[0], Utility.ConvertColor(Color.Green));
            AddQuad(verts[1], verts[5], verts[7], verts[3], Utility.ConvertColor(Color.Blue));
            AddQuad(verts[6], verts[7], verts[5], verts[4], Utility.ConvertColor(Color.Orange));
            AddQuad(verts[7], verts[6], verts[2], verts[3], Utility.ConvertColor(Color.Yellow));
            AddQuad(verts[1], verts[0], verts[4], verts[5], Utility.ConvertColor(Color.White));

        }
    }
}
