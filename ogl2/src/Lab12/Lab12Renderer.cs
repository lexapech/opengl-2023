using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal class Lab12Renderer
    {
        public CommonRenderer CommonRenderer;
        private readonly Lab12Controller _controller;

        public Lab12Renderer(CommonRenderer commonRenderer, Lab12Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;
        }

        public void DrawSelection(Rectangle rect)
        {
            CommonRenderer.MakeCurrent();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(-1, -1, 0);
            var size = CommonRenderer.GetSize();
            GL.Scale(2.0 / size.Width, 2.0 / size.Height, 1);
            GL.Disable(EnableCap.CullFace);
            GL.LineWidth(1f);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
            GL.Color4(1f, 1f, 1f, 1.0f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(rect.Left, rect.Top);
            GL.Vertex2(rect.Left, rect.Bottom);
            GL.Vertex2(rect.Right, rect.Bottom);
            GL.Vertex2(rect.Right, rect.Top);


            GL.End();
            GL.PopMatrix();
        }

        public void Render()
        {
            CommonRenderer.MakeCurrent();
            var model = _controller.RendererState;

            GL.LineWidth(model.PrimitiveSize);
            GL.PointSize(model.PrimitiveSize);
            GL.Scissor(model.ScissorRegion.Left, model.ScissorRegion.Top,
                       model.ScissorRegion.Width, model.ScissorRegion.Height);
            GL.AlphaFunc(model.AlphaFunction, model.AlphaRef);
            GL.BlendFunc(model.BlendingFactorSrc, model.BlendingFactorDest);
            if (model.CullingEnabled) GL.Enable(EnableCap.CullFace);
            if (model.ScissorEnabled) GL.Enable(EnableCap.ScissorTest);
            if (model.AlphaTestEnabled) GL.Enable(EnableCap.AlphaTest);
            if (model.BlendingEnabled) GL.Enable(EnableCap.Blend);
            GL.CullFace(model.CullFace);
            GL.PolygonMode(MaterialFace.Front, model.PolygonModeFront);
            GL.PolygonMode(MaterialFace.Back, model.PolygonModeBack);
            GL.Begin(model.SelectedPrimitive);
            for (int i = 0; i < model.Vertices.Count; i++)
            {
                var color = model.Colors[i % model.Colors.Length];
                var alpha = model.Alphas[i % model.Alphas.Length];
                GL.Color4(color.R / 255f, color.G / 255f, color.B / 255f, alpha);
                GL.Vertex2(model.Vertices[i].X, model.Vertices[i].Y);
            }
            GL.End();
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

    }
}
