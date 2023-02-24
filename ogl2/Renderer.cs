using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Text;
using System;
using System.Drawing.Drawing2D;

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
        public PrimitiveType _selectedPrimitive;
        private List<Vector2> _vertices = new List<Vector2>();
        private float _primitiveSize;
        private bool _cull;
        private bool _scissor;
        private bool _alpha;
        private bool _blend;
        private CullFaceMode _cullFace;
        private PolygonMode _polygonModeFront;
        private PolygonMode _polygonModeBack;
        private Rectangle _scissorRegion;
        private Rectangle _scissorDragRect;
        private Mode _mode;
        private Vector2 _scissorStartPos;
        private Vector2 _scissorEndPos;
        private bool _scissorSelectionDrag;
        private AlphaFunction _alphaFunction;
        private float _alphaRef;
        private BlendingFactor _blendingFactorSrc;
        private BlendingFactor _blendingFactorDest;
        public Renderer()
        {
            _selectedPrimitive = PrimitiveType.Points;
            _blendingFactorSrc = BlendingFactor.Zero;
            _blendingFactorDest = BlendingFactor.Zero;
            _primitiveSize = 1;
            _cull = false;
            _scissor = false;
            _alpha = false;
            _blend = false;
            _alphaFunction = AlphaFunction.Always;
            _alphaRef = 0;
            _mode = Mode.Drawing;
            _scissorSelectionDrag = false;
            _polygonModeFront = PolygonMode.Fill;
            _polygonModeBack = PolygonMode.Fill;
            _cullFace = CullFaceMode.Front;
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
            if (_mode == Mode.Drawing)
                _vertices.Add(point);
            else if (_mode == Mode.ScissorSelection) {
                StartScissorSelection(point);
            }
            Render();
        }

        public void SetMouseMode(Mode mode)
        {
            _mode = mode;
        }

        public void MouseMove(Vector2 point)
        {
            if(_scissorSelectionDrag && _mode == Mode.ScissorSelection)
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
          
            if (_scissorSelectionDrag && _mode == Mode.ScissorSelection)
            {
                StopScissorSelection(point);
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                _scissorRegion = _scissorDragRect;
                Render();
                SetMouseMode(Mode.Drawing);
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

        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            _selectedPrimitive = primitiveType;
        }

        public void SetPrimitiveSize(float value)
        {
            _primitiveSize = value;
        }
    
        public void EnableCulling(bool enabled)
        {
            _cull = enabled;
        }
        public void EnableScissor(bool enabled)
        {
            _scissor = enabled;
        }

        public void enableBlending(bool enabled)
        {
            _blend = enabled;
        }

        public void EnableAlpha(bool enabled)
        {
            _alpha = enabled;
        }

        public void SetCullingFace(CullFaceMode cullFaceMode)
        {
            _cullFace = cullFaceMode;
        }

        public void SetBlendingSource(BlendingFactor blendingFactor)
        {
            _blendingFactorSrc = blendingFactor;
        }

        public void SetBlendingDestination(BlendingFactor blendingFactor)
        {
            _blendingFactorDest = blendingFactor;
        }

        public void SetAlphaFunction(AlphaFunction alphaFunction)
        {
            _alphaFunction = alphaFunction;
        }

        public void SetAlphaRef(float alphaRef)
        {
            _alphaRef = alphaRef;
        }

        public void SetPolygonMode(PolygonMode front, PolygonMode back)
        {
            _polygonModeFront = front;
            _polygonModeBack = back;
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
            
            GL.LineWidth(_primitiveSize);
            GL.PointSize(_primitiveSize);
            GL.Scissor(_scissorRegion.Left, _scissorRegion.Top, _scissorRegion.Width, _scissorRegion.Height);
            GL.AlphaFunc(_alphaFunction, _alphaRef);
            GL.BlendFunc(_blendingFactorSrc, _blendingFactorDest);
            if(_cull) GL.Enable(EnableCap.CullFace);
            if(_scissor) GL.Enable(EnableCap.ScissorTest);
            if(_alpha) GL.Enable(EnableCap.AlphaTest);
            if(_blend) GL.Enable(EnableCap.Blend);
            GL.CullFace(_cullFace);
            GL.PolygonMode(MaterialFace.Front, _polygonModeFront);
            GL.PolygonMode(MaterialFace.Back, _polygonModeBack);
            GL.Begin(_selectedPrimitive);
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
