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
       
        private static readonly Color[] _colors = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };
        private static readonly float[] _alphas = {1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f};
        private GLControl _viewport;
          
        public void DrawSelection(Rectangle rect)
        {
            _viewport.MakeCurrent();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(-1, -1, 0);
            GL.Scale(2.0 / _viewport.Width, 2.0 / _viewport.Height,1);         
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
           /* GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _viewport.ClientSize.Width, 0, _viewport.ClientSize.Height, -1, 1);*/
        }

        public Size GetSize()
        {
            return _viewport.Size;
        }

        public void SwapBuffers()
        {
            _viewport.SwapBuffers();
        }

        public void Render(Fractal fractal)
        {
            _viewport.MakeCurrent();           
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(0, -1, 0);
            GL.Scale(1 / 200f / _viewport.Width*_viewport.Height, 1 / 200f,1);
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < fractal.Branches.Count; i++)
            {               
                GL.Color3(Color.Gray);
                var ortho = fractal.Branches[i].Direction.PerpendicularRight * fractal.Branches[i].Scale/50f;
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

        public void Render(RendererState model)
        {
            _viewport.MakeCurrent();
            
            GL.LineWidth(model.PrimitiveSize);
            GL.PointSize(model.PrimitiveSize);
            GL.Scissor(model.ScissorRegion.Left, model.ScissorRegion.Top,
                       model.ScissorRegion.Width, model.ScissorRegion.Height);
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
