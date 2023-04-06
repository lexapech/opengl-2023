using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class Lab6Controller : ILab
    {
        private readonly Scene _scene;
        public Scene Scene { get { return _scene; } }
        private readonly Lab6Renderer _renderer;
        private bool _loaded = false;
        public Lab6Controller(CommonRenderer commonRenderer)
        {
            _renderer = new Lab6Renderer(commonRenderer, this);
            _scene = new Scene
            {
                CameraAngle = new Vector2((float)(-45 / 180f * Math.PI), (float)(45 / 180f * Math.PI)),
                CameraDistance = 2
            };
        }

        private void LoadShaders(string vertex, string fragment)
        {
            var shader1 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, vertex));
            var shader2 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fragment));
            _renderer.LoadShaders(shader1, shader2);
        }


        public void Activate()
        {
            if (!_loaded)
            {
                _renderer.Init();
                LoadShaders("vertex.glsl", "fragment.glsl");
                _loaded = true;
            }
            Paint();
        }

        public void MouseDown(Vector2 pos)
        {
        }

        public void MouseMove(Vector2 pos, Vector2 previousMousePos, bool leftButton, bool rightButton, bool shift)
        {
            if (shift && rightButton)
            {
                var delta = pos - previousMousePos;
                _scene.MoveCamera(delta);
                Paint();
            }
            else if (rightButton)
            {
                var delta = pos - previousMousePos;
                _scene.RotateCamera(delta);
                Paint();
            }
        }

        public void MouseUp(Vector2 pos)
        {

        }

        public void Paint()
        {
            _renderer.CommonRenderer.Clear();
            _renderer.Render();
            _renderer.CommonRenderer.SwapBuffers();
        }

        public void Zoom(int delta)
        {
            _scene.Zoom(delta);
            Paint();
        }
    }
}
