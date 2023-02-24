using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ogl2
{
    internal class Renderer
    {
        public enum Mode
        {
            Drawing, ScissorSelection
        }
    
        private static Color[] _colors = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };
        private static float[] _alphas = {1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f};
        private GLControl _viewport;
        public PrimitiveType SelectedPrimitive;
        private List<Vector2> _vertices = new List<Vector2>();
        public float PrimitiveSize { private get; set; }
        public bool CullingEnabled { private get; set; }
        public bool ScissorEnabled { private get; set; }
        public bool AlphaTestEnabled { private get; set; }
        public bool BlendingEnabled { private get; set; }
        public CullFaceMode CullFace { private get; set; }
        public PolygonMode PolygonModeFront { private get; set; }
        public PolygonMode PolygonModeBack { private get; set; }
        public AlphaFunction AlphaFunction { private get; set; }
        public float AlphaRef { private get; set; }
        public BlendingFactor BlendingFactorSrc { private get; set; }
        public BlendingFactor BlendingFactorDest { private get; set; }

        private Rectangle _scissorRegion;
        private Rectangle _scissorDragRect;
        public Mode MouseMode { private get; set; }
        private Vector2 _scissorStartPos;
        private Vector2 _scissorEndPos;
        private bool _scissorSelectionDrag;
        
        public Renderer()
        {
            SelectedPrimitive = PrimitiveType.Points;
            BlendingFactorSrc = BlendingFactor.Zero;
            BlendingFactorDest = BlendingFactor.Zero;
            PrimitiveSize = 1;
            CullingEnabled = false;
            ScissorEnabled = false;
            AlphaTestEnabled = false;
            BlendingEnabled = false;
            AlphaFunction = AlphaFunction.Always;
            AlphaRef = 0;
            MouseMode = Mode.Drawing;
            _scissorSelectionDrag = false;
            PolygonModeFront = PolygonMode.Fill;
            PolygonModeBack = PolygonMode.Fill;
            CullFace = CullFaceMode.Front;
            _scissorRegion = new Rectangle(0,0,0,0);
            _scissorDragRect = new Rectangle(0,0,0,0);
            _scissorStartPos = new Vector2(0,0);
            _scissorEndPos = new Vector2(0, 0);
        }

        public void Resize()
        {
            _viewport.MakeCurrent();
            GL.Viewport(0, 0, _viewport.ClientSize.Width, _viewport.ClientSize.Height);
            GL.LoadIdentity();
            GL.Ortho(0, _viewport.ClientSize.Width, 0, _viewport.ClientSize.Height,-1, 1);
        }

        public void MouseDown(Vector2 point)
        {
            if (MouseMode == Mode.Drawing)
                _vertices.Add(point);
            else if (MouseMode == Mode.ScissorSelection) {
                StartScissorSelection(point);
            }
            Render();
        }

        public void MouseMove(Vector2 point)
        {
            if(_scissorSelectionDrag && MouseMode == Mode.ScissorSelection)
            {
                _scissorEndPos = point;
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                Render(false);
                DrawSelection();
                SwapBuffers();
            }         
        }

        public void ClearScissorSelection()
        {
            _scissorRegion = new Rectangle(0, 0, _viewport.ClientSize.Width, _viewport.ClientSize.Height);
            Render();
        }

        public void MouseUp(Vector2 point)
        {
          
            if (_scissorSelectionDrag && MouseMode == Mode.ScissorSelection)
            {
                StopScissorSelection(point);
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                _scissorRegion = _scissorDragRect;
                Render();
                MouseMode = Mode.Drawing;
                _scissorSelectionDrag = false;
            }
        }

        private Rectangle PointsToRect(Vector2 point1,Vector2 point2)
        {
            var min = new Vector2(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            var max = new Vector2(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y));
            return Rectangle.FromLTRB((int)min.X, (int)min.Y, (int)max.X, (int)max.Y);
        }

        private void StartScissorSelection(Vector2 point)
        {
            _scissorSelectionDrag = true;
            _scissorStartPos = point;           
        }
        private void StopScissorSelection(Vector2 point)
        {
            _scissorSelectionDrag = false;
            _scissorEndPos = point;
        }

        public void ClearPoints()
        {
            _vertices.Clear();
            Render();
        }

        public void SetViewport(GLControl viewport)
        {
            if(_viewport == null)
            {
                _viewport = viewport;
                _scissorRegion = new Rectangle(0, 0, _viewport.ClientSize.Width, _viewport.ClientSize.Height);
            }
               
        }

        private void DrawSelection()
        {
            _viewport.MakeCurrent();
            GL.Disable(EnableCap.CullFace);
            GL.LineWidth(1f);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
            GL.Color4(1f, 1f, 1f, 1.0f);
            GL.Begin(PrimitiveType.Quads);             
            GL.Vertex2(_scissorDragRect.Left, _scissorDragRect.Top);
            GL.Vertex2(_scissorDragRect.Left, _scissorDragRect.Bottom);
            GL.Vertex2(_scissorDragRect.Right, _scissorDragRect.Bottom);
            GL.Vertex2(_scissorDragRect.Right, _scissorDragRect.Top);
            
            
            GL.End();
        }
        public void Clear()
        {
            _viewport.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
        public void SwapBuffers()
        {
            _viewport.SwapBuffers();
        }
        public void Render(bool swapBuffers = true)
        {
            Clear();
            _viewport.MakeCurrent();
            
            GL.LineWidth(PrimitiveSize);
            GL.PointSize(PrimitiveSize);
            GL.Scissor(_scissorRegion.Left, _scissorRegion.Top, _scissorRegion.Width, _scissorRegion.Height);
            GL.AlphaFunc(AlphaFunction, AlphaRef);
            GL.BlendFunc(BlendingFactorSrc, BlendingFactorDest);
            if(CullingEnabled) GL.Enable(EnableCap.CullFace);
            if(ScissorEnabled) GL.Enable(EnableCap.ScissorTest);
            if(AlphaTestEnabled) GL.Enable(EnableCap.AlphaTest);
            if(BlendingEnabled) GL.Enable(EnableCap.Blend);
            GL.CullFace(CullFace);
            GL.PolygonMode(MaterialFace.Front, PolygonModeFront);
            GL.PolygonMode(MaterialFace.Back, PolygonModeBack);
            GL.Begin(SelectedPrimitive);
            for (int i = 0; i < _vertices.Count; i++)
            {
                var color = _colors[i % _colors.Length];
                var alpha = _alphas[i % _alphas.Length];
                GL.Color4(color.R/255f, color.G / 255f, color.B / 255f, alpha);
                GL.Vertex2(_vertices[i].X, _vertices[i].Y);
            }
            GL.End();
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
            if (swapBuffers) SwapBuffers();
        }
    }
}
