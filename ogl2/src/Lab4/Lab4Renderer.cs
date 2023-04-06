using ogl2.src.Lab3;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab4
{
    internal class Lab4Renderer
    {
        public CommonRenderer CommonRenderer;
        private readonly Lab4Controller _controller;

        public Lab4Renderer(CommonRenderer commonRenderer, Lab4Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;
        }

        private void RenderBezierCurve(Spline spline)
        {
            var points = spline.GetBezierPoints();

            for (int i = 0; i < points.Length; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                for (int j = 0; j < points[i].Length / 3; j++)
                {
                    GL.Color4(1f, 1f, 0f, 1f);
                    GL.Vertex2(points[i][3 * j], points[i][3 * j + 1]);
                }
                GL.End();
                GL.Begin(PrimitiveType.Points);
                for (int j = 0; j < points[i].Length / 3; j++)
                {
                    GL.Color4(1f, 1f, 1f, 1f);
                    GL.Vertex2(points[i][3 * j], points[i][3 * j + 1]);
                }
                GL.End();

                GL.Map1(MapTarget.Map1Vertex3, 0.0f, 1.0f, 3, 4, points[i]);
                GL.Enable(EnableCap.Map1Vertex3);
                GL.Color3(0, 1.0, 0);
                GL.Begin(PrimitiveType.LineStrip);
                for (int j = 0; j <= 4 * spline.Steps; j++)
                    GL.EvalCoord1(((float)j) / (4 * spline.Steps));
                GL.End();
                GL.Disable(EnableCap.Map1Vertex3);
            }

        }

        private void RenderSpline(Spline spline)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.LineWidth(2);
            for (int i = 0; i < spline.Vertices.Count; i++)
            {
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Vertex2(spline.Vertices[i].X, spline.Vertices[i].Y);
            }
            GL.End();

            GL.PointSize(5);

            GL.Begin(PrimitiveType.Points);
            for (int i = 1; i < spline.ControlPoints.Count - 1; i++)
            {
                GL.Color4(1f, 0f, 0f, 1f);
                GL.Vertex2(spline.ControlPoints[i].X, spline.ControlPoints[i].Y);
            }
            GL.End();
        }

        public void Render()
        {
            var spline = _controller.Spline;
            CommonRenderer.MakeCurrent();
            if (spline.RenderCardinal)
                RenderSpline(spline);
            if (spline.RenderBezier)
                RenderBezierCurve(spline);
        }

    }
}
