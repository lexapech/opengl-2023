using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab4
{
    internal class Lab4Controller : ILab
    {
        private readonly Spline _spline;

        private readonly Lab4Renderer _renderer;
        public Spline Spline { get { return _spline; } }

        public Action<bool> CursorChangeHandler;
        public Lab4Controller(CommonRenderer renderer)
        {
            _spline = new Spline();
            _renderer = new Lab4Renderer(renderer, this);
        }

        public void MouseDown(Vector2 pos, bool leftButton, bool rightButton, bool shift)
        {
            var pointIndex = _spline.NearControlPoint(Utility.ConvertMousePos(pos, _renderer.CommonRenderer.GetSize()));
            if (pointIndex != -1)
            {
                _spline.GrabControlPoint(pointIndex);
            }
        }

        public void MouseUp(Vector2 pos)
        {
            _spline.ReleaseControlPoint();
        }

        public void MouseMove(Vector2 pos, Vector2 previousMousePos, bool leftButton, bool rightButton, bool shift)
        {
            if (CursorChangeHandler != null)
            {
                if (_spline.NearControlPoint(Utility.ConvertMousePos(pos, _renderer.CommonRenderer.GetSize())) != -1)
                    CursorChangeHandler.Invoke(true);
                else
                    CursorChangeHandler.Invoke(false);
            }
            if (_spline.MoveGrabbedControlPoint(Utility.ConvertMousePos(pos, _renderer.CommonRenderer.GetSize())))
            {
                _spline.Generate();
                Paint();
            }
        }

        public void Zoom(int delta)
        {

        }

        public void Activate()
        {
            _spline.Generate();
            Paint();
        }

        public void Paint()
        {
            _renderer.CommonRenderer.Clear();
            _renderer.Render();
            _renderer.CommonRenderer.SwapBuffers();
        }

        public void SetSplineSteps(int steps)
        {
            _spline.Steps = steps;
            _spline.Generate();
            Paint();
        }

        public void SetSplineScale(float scale)
        {
            _spline.Scale = scale;
            _spline.Generate();
            Paint();
        }

        public void EnableBezierSpline(bool enabled)
        {
            _spline.RenderBezier = enabled;
            Paint();
        }

        public void EnableCardinalSpline(bool enabled)
        {
            _spline.RenderCardinal = enabled;
            Paint();
        }

    }
}
