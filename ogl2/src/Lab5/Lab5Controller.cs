using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ogl2.Presenter;

namespace ogl2.src.Lab5
{
    internal class Lab5Controller : ILab
    {
        private readonly Surface _surface;
        public Surface Surface { get { return _surface; } }
        private readonly Lab5Renderer _renderer;
        private bool _loaded = false;
        public Lab5Controller(CommonRenderer commonRenderer, Spline spline) 
        {
            _renderer = new Lab5Renderer(commonRenderer, this);
            _surface = new Surface(spline)
            {
                CameraAngle = new Vector2((float)(-45 / 180f * Math.PI), (float)(45 / 180f * Math.PI)),
                CameraDistance = 2
            };

            _surface.Generate();
        }

        private void LoadShaders(string vertex, string fragment)
        {
            if(_loaded) return;
            var shader1 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, vertex));
            var shader2 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fragment));
            _renderer.LoadShaders(shader1,shader2);
            _loaded = true;
        }

        public void MouseDown(Vector2 pos)
        {

        }

        public void MouseUp(Vector2 pos)
        {
            
        }

        public void MouseMove(Vector2 pos, Vector2 previousMousePos, bool leftButton, bool rightButton, bool shift)
        {
            if (rightButton)
            {
                var delta = pos - previousMousePos;
                _surface.RotateCamera(delta);
                Paint();
            }
        }

        public void Activate()
        {
            LoadShaders("vertex.glsl", "fragment.glsl");
            _surface.Generate();
            Paint();
        }

        public void Paint()
        {
            _renderer.CommonRenderer.Clear();
            _renderer.Render();
            _renderer.CommonRenderer.SwapBuffers();
        }

        public void Zoom(int delta)
        {
            _surface.Zoom(delta);
            Paint();          
        }
        public void SetSurfaceLightPosition(Vector3 pos)
        {
            _surface.Point = pos;
            Paint();
        }

    }
}
