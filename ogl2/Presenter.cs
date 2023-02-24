using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using static ogl2.Renderer;

namespace ogl2
{ 
    internal class Presenter
    {
        private IRenderer _renderer;
        private Model _model;

        private Rectangle _scissorDragRect;
        public Mode MouseMode;
        private Vector2 _scissorStartPos;
        private Vector2 _scissorEndPos;
        private bool _scissorSelectionDrag;

        public Presenter(IRenderer renderer)
        {
            _renderer = renderer;
            _model = new Model();
            MouseMode = Mode.Drawing;
            _scissorSelectionDrag = false;
            _scissorDragRect = new Rectangle(0, 0, 0, 0);
            _scissorStartPos = new Vector2(0, 0);
            _scissorEndPos = new Vector2(0, 0);
        }

        public enum Mode
        {
            Drawing, ScissorSelection
        }

        public void SetViewport(GLControl viewport)
        {
            if (_model.Viewport == null)
            {
                _model.Viewport = viewport;
                _renderer.SetViewport(viewport);
                _model.ScissorRegion = new Rectangle(0, 0, viewport.ClientSize.Width, viewport.ClientSize.Height);
            }                   
        }

        private Rectangle PointsToRect(Vector2 point1, Vector2 point2)
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
            _model.Vertices.Clear();
            Paint();
        }

        public void Resize()
        {
            _renderer.Resize();
        }

        public void MouseDown(Vector2 point)
        {
            if (MouseMode == Mode.Drawing)
                _model.Vertices.Add(point);
            else if (MouseMode == Mode.ScissorSelection)
            {
                StartScissorSelection(point);
            }
            Paint();
        }

        public void MouseMove(Vector2 point)
        {
            if (_scissorSelectionDrag && MouseMode == Mode.ScissorSelection)
            {
                _scissorEndPos = point;
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                _renderer.Clear();
                _renderer.Render(_model);
                _renderer.DrawSelection(_scissorDragRect);
                _renderer.SwapBuffers();
            }
        }

        public void ClearScissorSelection()
        {
            _model.ScissorRegion = new Rectangle(0, 0, _model.Viewport.ClientSize.Width, _model.Viewport.ClientSize.Height);
            Paint();
        }

        public void BeginScissorSelection()
        {
            ClearScissorSelection();
            MouseMode = Mode.ScissorSelection;
        }

        public void MouseUp(Vector2 point)
        {
            if (_scissorSelectionDrag && MouseMode == Mode.ScissorSelection)
            {
                StopScissorSelection(point);
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                _model.ScissorRegion = _scissorDragRect;
                Paint();
                MouseMode = Mode.Drawing;
                _scissorSelectionDrag = false;
            }
        }

        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            _model.SelectedPrimitive = primitiveType;
            _renderer.Clear();
            _renderer.Render(_model);
            _renderer.SwapBuffers();
        }
        public void Paint()
        {
            _renderer.Clear();
            _renderer.Render(_model);
            _renderer.SwapBuffers();
        }
        public void SetPrimitiveSize(float primitiveSize)
        {
            _model.PrimitiveSize = primitiveSize;
            Paint();
        }

        public void SetCullFace(CullFaceMode cullFace)
        {
            _model.CullFace = cullFace;
            Paint();
        }

        public void SetPolygonModeFront(PolygonMode polygonMode)
        {
            _model.PolygonModeFront = polygonMode;
            Paint();
        }
        public void SetPolygonModeBack(PolygonMode polygonMode)
        {
            _model.PolygonModeBack = polygonMode;
            Paint();
        }

        public void SetAlphaFunction(AlphaFunction alphaFunction)
        {
            _model.AlphaFunction = alphaFunction;
            Paint();
        }

        public void SetAlphaRef(float alphaRef)
        {
            _model.AlphaRef = alphaRef;
            Paint();
        }

        
        public void SetBlendingFactorSrc(BlendingFactor blendingFactor)
        {
            _model.BlendingFactorSrc = blendingFactor;
            Paint();
        }
        public void SetBlendingFactorDest(BlendingFactor blendingFactor)
        {
            _model.BlendingFactorDest = blendingFactor;
            Paint();
        }
        public void EnableCulling(bool enabled)
        {
            _model.CullingEnabled = enabled;
            Paint();
        }
        public void EnableScissor(bool enabled)
        {
            _model.ScissorEnabled = enabled;
            Paint();
        }

        public void EnableAlphaTest(bool enabled)
        {
            _model.AlphaTestEnabled = enabled;
            Paint();
        }

        public void EnableBlending(bool enabled)
        {
            _model.BlendingEnabled = enabled;
            Paint();
        }
        
    }
}
