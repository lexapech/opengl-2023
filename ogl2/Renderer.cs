using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Windows.Forms;

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
        public static Dictionary<string, CullFaceMode> CullFaceModes = new Dictionary<string, CullFaceMode>
        {
            {"GL_FRONT",CullFaceMode.Front },
            {"GL_BACK",CullFaceMode.Back },
            {"GL_FRONT_AND_BACK",CullFaceMode.FrontAndBack }
        };
        public static Dictionary<string, PolygonMode> PolygonModes = new Dictionary<string, PolygonMode>
        {
            {"GL_FILL",PolygonMode.Fill },
            {"GL_POINT",PolygonMode.Point },
            {"GL_LINE",PolygonMode.Line }
        };
        private static Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };

        private GLControl _viewport;
        public PrimitiveType SelectedPrimitive;
        private List<Vector2> _vertices = new List<Vector2>();
        private float _primitiveSize;
        private bool _cull;
        private CullFaceMode _cullFace;
        private PolygonMode _polygonModeFront;
        private PolygonMode _polygonModeBack;
        public Renderer()
        {
            SelectedPrimitive = PrimitiveType.Points;
            _primitiveSize = 1;
            _cull = false;
            _polygonModeFront = PolygonMode.Fill;
            _polygonModeBack = PolygonMode.Fill;
            _cullFace = CullFaceMode.Front;
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
            SelectedPrimitive = primitiveType;
        }

        public void SetPrimitiveSize(float value)
        {
            _primitiveSize = value;
        }
    
        public void EnableCulling(bool enabled)
        {
            _cull = enabled;
        }

        public void SetCullingFace(CullFaceMode cullFaceMode)
        {
            _cullFace = cullFaceMode;
        }
        public void SetPolygonMode(PolygonMode front, PolygonMode back)
        {
            _polygonModeFront = front;
            _polygonModeBack = back;
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

            GL.LineWidth(_primitiveSize);
            GL.PointSize(_primitiveSize);
            if (_cull) GL.Enable(EnableCap.CullFace);
            GL.CullFace(_cullFace);
            GL.PolygonMode(MaterialFace.Front, _polygonModeFront);
            GL.PolygonMode(MaterialFace.Back, _polygonModeBack);
            GL.Begin(SelectedPrimitive);
            GL.Color4(Color4.Silver);
            for (int i = 0; i < _vertices.Count; i++)
            {
                var color = _colors[i % _colors.Length];
                GL.Color4(color.R/255f, color.G / 255f, color.B / 255f, 1.0f);
                GL.Vertex2(_vertices[i].X, _vertices[i].Y);
            }
            GL.End();

            GL.Disable(EnableCap.CullFace);
            _viewport.SwapBuffers();
        }
    }
}
