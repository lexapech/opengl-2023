using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ogl2
{ 
    internal class Presenter
    {
        private IRenderer _renderer;
        private RendererState _state;
        private Fractal _fractal;

        private Rectangle _scissorDragRect;
        public Mode CurrentMode;
        private Vector2 _scissorStartPos;
        private Vector2 _scissorEndPos;
        private bool _scissorSelectionDrag;

        public Presenter(IRenderer renderer)
        {
            _renderer = renderer;
            _state = new RendererState();
            _fractal = new Fractal();
            CurrentMode = Mode.Drawing;
            _scissorSelectionDrag = false;
            _scissorDragRect = new Rectangle(0, 0, 0, 0);
            _scissorStartPos = new Vector2(0, 0);
            _scissorEndPos = new Vector2(0, 0);
        }

        public enum Mode
        {
            Drawing, ScissorSelection, Fractal
        }

        public void SetViewport(GLControl viewport)
        {
            if (_state.Viewport == null)
            {
                _state.Viewport = viewport;
                _renderer.SetViewport(viewport);
                _state.ScissorRegion = new Rectangle(0, 0, viewport.ClientSize.Width, viewport.ClientSize.Height);
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
            _state.Vertices.Clear();
            Paint();
        }

        public void Resize()
        {
            _renderer.Resize();
        }

        public void MouseDown(Vector2 point)
        {
            if (CurrentMode == Mode.Drawing)
                _state.Vertices.Add(point);
            else if (CurrentMode == Mode.ScissorSelection)
            {
                StartScissorSelection(point);
            }
            Paint();
        }

        public void MouseMove(Vector2 point)
        {
            if (_scissorSelectionDrag && CurrentMode == Mode.ScissorSelection)
            {
                _scissorEndPos = point;
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                _renderer.Clear();
                _renderer.Render(_state);
                _renderer.DrawSelection(_scissorDragRect);
                _renderer.SwapBuffers();
            }
        }

        public void TabChanged(int index) 
        {
            if (index == 2)
            {
                CurrentMode = Mode.Fractal;
                _fractal.Generate();
                Paint();
            }
            else 
            { 
                CurrentMode = Mode.Drawing;
                Paint();
            }
        }

        public void SetFractalSteps(int steps)
        {
            _fractal.Steps = steps;
            _fractal.Generate();
            Paint();
        }

        public void ChangeFractalSeed()
        {
            _fractal.ChangeSeed();
            _fractal.Generate();
            Paint();
        }

        public void ClearScissorSelection()
        {
            _state.ScissorRegion = new Rectangle(0, 0, _state.Viewport.ClientSize.Width, _state.Viewport.ClientSize.Height);
            Paint();
        }

        public void BeginScissorSelection()
        {
            ClearScissorSelection();
            CurrentMode = Mode.ScissorSelection;
        }

        public void MouseUp(Vector2 point)
        {
            if (_scissorSelectionDrag && CurrentMode == Mode.ScissorSelection)
            {
                StopScissorSelection(point);
                _scissorDragRect = PointsToRect(_scissorStartPos, _scissorEndPos);
                _state.ScissorRegion = _scissorDragRect;
                Paint();
                CurrentMode = Mode.Drawing;
                _scissorSelectionDrag = false;
            }
        }

        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            _state.SelectedPrimitive = primitiveType;
            _renderer.Clear();
            _renderer.Render(_state);
            _renderer.SwapBuffers();
        }
        public void PaintFractal()
        {
            _renderer.Clear();
            _renderer.Render(_fractal);
            _renderer.SwapBuffers();
        }
        public void Paint()
        {
            _renderer.Clear();
            if(CurrentMode == Mode.Fractal)
            {
                _renderer.Render(_fractal);
            }
            else
            {
                _renderer.Render(_state);
            }
            _renderer.SwapBuffers();
        }
        public void SetPrimitiveSize(float primitiveSize)
        {
            _state.PrimitiveSize = primitiveSize;
            Paint();
        }

        public void SetCullFace(CullFaceMode cullFace)
        {
            _state.CullFace = cullFace;
            Paint();
        }

        public void SetPolygonModeFront(PolygonMode polygonMode)
        {
            _state.PolygonModeFront = polygonMode;
            Paint();
        }
        public void SetPolygonModeBack(PolygonMode polygonMode)
        {
            _state.PolygonModeBack = polygonMode;
            Paint();
        }

        public void SetAlphaFunction(AlphaFunction alphaFunction)
        {
            _state.AlphaFunction = alphaFunction;
            Paint();
        }

        public void SetAlphaRef(float alphaRef)
        {
            _state.AlphaRef = alphaRef;
            Paint();
        }

        
        public void SetBlendingFactorSrc(BlendingFactor blendingFactor)
        {
            _state.BlendingFactorSrc = blendingFactor;
            Paint();
        }
        public void SetBlendingFactorDest(BlendingFactor blendingFactor)
        {
            _state.BlendingFactorDest = blendingFactor;
            Paint();
        }
        public void EnableCulling(bool enabled)
        {
            _state.CullingEnabled = enabled;
            Paint();
        }
        public void EnableScissor(bool enabled)
        {
            _state.ScissorEnabled = enabled;
            Paint();
        }

        public void EnableAlphaTest(bool enabled)
        {
            _state.AlphaTestEnabled = enabled;
            Paint();
        }

        public void EnableBlending(bool enabled)
        {
            _state.BlendingEnabled = enabled;
            Paint();
        }
        
    }
}
