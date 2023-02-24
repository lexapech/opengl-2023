using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ogl2
{
    internal class Renderer : IRenderer
    {
       
        private static Color[] _colors = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };
        private static float[] _alphas = {1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f};
        private GLControl _viewport;
          
        public void DrawSelection(Rectangle rect)
        {
            _viewport.MakeCurrent();
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
        }

        public void SetViewport(GLControl viewport)
        {
            _viewport = viewport;
        }

        public void Clear()
        {
            _viewport.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Resize()
        {
            _viewport.MakeCurrent();
            GL.Viewport(0, 0, _viewport.ClientSize.Width, _viewport.ClientSize.Height);
            GL.LoadIdentity();
            GL.Ortho(0, _viewport.ClientSize.Width, 0, _viewport.ClientSize.Height, -1, 1);
        }

        public void SwapBuffers()
        {
            _viewport.SwapBuffers();
        }
        public void Render(Model model)
        {
            _viewport.MakeCurrent();
            
            GL.LineWidth(model.PrimitiveSize);
            GL.PointSize(model.PrimitiveSize);
            GL.Scissor(model.ScissorRegion.Left, model.ScissorRegion.Top, model.ScissorRegion.Width, model.ScissorRegion.Height);
            GL.AlphaFunc(model.AlphaFunction, model.AlphaRef);
            GL.BlendFunc(model.BlendingFactorSrc, model.BlendingFactorDest);
            if(model.CullingEnabled) GL.Enable(EnableCap.CullFace);
            if(model.ScissorEnabled) GL.Enable(EnableCap.ScissorTest);
            if(model.AlphaTestEnabled) GL.Enable(EnableCap.AlphaTest);
            if(model.BlendingEnabled) GL.Enable(EnableCap.Blend);
            GL.CullFace(model.CullFace);
            GL.PolygonMode(MaterialFace.Front, model.PolygonModeFront);
            GL.PolygonMode(MaterialFace.Back, model.PolygonModeBack);
            GL.Begin(model.SelectedPrimitive);
            for (int i = 0; i < model.Vertices.Count; i++)
            {
                var color = _colors[i % _colors.Length];
                var alpha = _alphas[i % _alphas.Length];
                GL.Color4(color.R/255f, color.G / 255f, color.B / 255f, alpha);
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
