using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;


namespace ogl2
{
    internal class Lab12Controller : ILab
    {
        private readonly RendererState _state;

        private readonly Lab12Renderer _renderer;

        private MouseMode _mode;

        private Rectangle _scissorDragRect;
        private Vector2 _scissorStartPos;
        private Vector2 _scissorEndPos;
        private bool _scissorSelectionDrag;


        public RendererState RendererState
        {
            get
            {
                return _state;
            }
        }

        enum MouseMode
        {
            Drawing, Selection
        }


        public Lab12Controller(CommonRenderer commonRenderer) 
        {
            _state = new RendererState();
            _mode = MouseMode.Drawing;
            _renderer = new Lab12Renderer(commonRenderer, this);
        }

        void ILab.MouseDown(Vector2 pos)
        {          
            if (_mode == MouseMode.Drawing)
                _state.Vertices.Add(Utility.ConvertMousePos(pos, _renderer.CommonRenderer.GetSize()));          
            else if (_mode == MouseMode.Selection)
            {
                StartScissorSelection(pos);
            }
            Paint();
        }

        void ILab.MouseMove(Vector2 pos, Vector2 previousMousePos, bool leftButton, bool rightButton, bool shift)
        {
            if (_scissorSelectionDrag && _mode == MouseMode.Selection)
            {
                _scissorEndPos = pos;
                _scissorDragRect = Utility.PointsToRect(_scissorStartPos, _scissorEndPos);
                _renderer.CommonRenderer.Clear();
                _renderer.Render();
                _renderer.DrawSelection(_scissorDragRect);
                _renderer.CommonRenderer.SwapBuffers();
            }
        }

        void ILab.MouseUp(Vector2 pos)
        {
            if (_scissorSelectionDrag && _mode == MouseMode.Selection)
            {
                StopScissorSelection(pos);
                _scissorDragRect = Utility.PointsToRect(_scissorStartPos, _scissorEndPos);
                _state.ScissorRegion = _scissorDragRect;
                Paint();
                _mode = MouseMode.Drawing;
                _scissorSelectionDrag = false;
            }
        }

        public void Zoom(int delta)
        {

        }
        private void Paint()
        {
            _renderer.CommonRenderer.Clear();
            _renderer.Render();
            _renderer.CommonRenderer.SwapBuffers();
        }

        void ILab.Paint()
        {
            Paint();
        }

        public void Activate()
        {
            Paint();
        }


        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            _state.SelectedPrimitive = primitiveType;
            Paint();
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

        public void ClearPoints()
        {
            _state.Vertices.Clear();
            Paint();
        }

        public void ClearScissorSelection()
        {
            _state.ScissorRegion = new Rectangle(0, 0, _state.Viewport.ClientSize.Width, _state.Viewport.ClientSize.Height);
            Paint();
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


        public void BeginScissorSelection()
        {
            ClearScissorSelection();
            _mode = MouseMode.Selection;
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
