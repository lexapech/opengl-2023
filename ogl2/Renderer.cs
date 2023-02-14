using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ogl2
{
    internal class Renderer
    {
        public static Dictionary<string, PrimitiveType> Primitives = new Dictionary<string, PrimitiveType>
        {
            {"GL_POINTS",PrimitiveType.Points },
            {"GL_LINES",PrimitiveType.Lines },
            {"GL_LINE_STRIP",PrimitiveType.LineStrip },
            {"GL_LINE_LOOP",PrimitiveType.LineLoop },
            {"GL_TRIANGLES",PrimitiveType.Triangles },
            {"GL_TRIANGLE_STRIP",PrimitiveType.TriangleStrip },
            {"GL_TRIANGLE_FAN",PrimitiveType.TriangleFan },
            {"GL_QUADS",PrimitiveType.Quads },
            {"GL_QUAD_STRIP",PrimitiveType.QuadStrip },
            {"GL_POLYGON",PrimitiveType.Polygon }
        };
        private static Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };

        private GLControl _viewport;
        private PrimitiveType selectedPrimitive;
        private List<Vector2> _vertices = new List<Vector2>();
        public Renderer()
        {
            selectedPrimitive = PrimitiveType.Points;
        }

        public void Resize()
        {
            _viewport.MakeCurrent();
            GL.Viewport(0, 0, _viewport.ClientSize.Width, _viewport.ClientSize.Height);
        }

        public void AddPoint(Vector2 point)
        {
            _vertices.Add(point);
            Render();
        }
        public void ClearPoints()
        {
            _vertices.Clear();
            Render();
        }

        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            selectedPrimitive = primitiveType;
        }

        public void SetViewport(GLControl viewport)
        {
            if(_viewport == null)
                _viewport = viewport;
        }
        public void Render()
        {            
            _viewport.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(selectedPrimitive);

            GL.Color4(Color4.Silver);
            for (int i = 0; i < _vertices.Count; i++)
            {
                var color = _colors[i % _colors.Length];
                GL.Color4(color.R, color.G, color.B, 1.0f);
                GL.Vertex2(_vertices[i].X, _vertices[i].Y);
            }
            GL.End();

            _viewport.SwapBuffers();
        }
    }
}
