using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab3
{
    internal class Lab3Renderer
    {
        public CommonRenderer CommonRenderer;
        private readonly Lab3Controller _controller;

        public Lab3Renderer(CommonRenderer commonRenderer, Lab3Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;
        }

        public void Render()
        {
            var fractal = _controller.Fractal;
            CommonRenderer.MakeCurrent();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(0, -1, 0);
            var size = CommonRenderer.GetSize();
            GL.Scale(1 / 200f / size.Width * size.Height, 1 / 200f, 1);
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < fractal.Branches.Count; i++)
            {
                GL.Color3(Color.Gray);
                var ortho = fractal.Branches[i].Direction.PerpendicularRight * fractal.Branches[i].Scale / 50f;
                GL.Vertex2(fractal.Branches[i].Start + ortho);
                GL.Vertex2(fractal.Branches[i].Start - ortho);
                GL.Vertex2(fractal.Branches[i].End + ortho);

                GL.Vertex2(fractal.Branches[i].Start - ortho);
                GL.Vertex2(fractal.Branches[i].End + ortho);
                GL.Vertex2(fractal.Branches[i].End - ortho);

                GL.Vertex2(fractal.Branches[i].End + ortho);
                GL.Vertex2(fractal.Branches[i].End - ortho);
                GL.Vertex2(fractal.Branches[i].End + fractal.Branches[i].Direction * ortho.Length);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
