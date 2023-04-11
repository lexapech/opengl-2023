using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ogl2.src.Lab6.Lab6Controller;

namespace ogl2.src.Lab6
{
    internal class Lab6Controller : ILab
    {
        public delegate void SelectionChange(SceneObject sceneObject);
        public event SelectionChange SelectionChanged;


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
                CameraDistance = 8
            };

            _scene.AddObject("cube", new CubeGenerator().SetColor(Color.LightBlue));
            _scene.AddObject("cube2", new CubeGenerator().SetColor(Color.LightBlue)).Translate(new Vector3(3,0,0));
            var cube = _scene.GetObject("cube");
            //cube.Translate(new Vector3(1, 0, 0)).Rotate(Vector3.UnitY,45).Scale(new Vector3(1,1,0.5f));
        }

        private void LoadShaders(string vertex, string fragment,string outline)
        {
            var shader1 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, vertex));
            var shader2 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fragment));
            var shader3 = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, outline));
            _renderer.LoadShaders(shader1, shader2, shader3);
        }


        public void Activate()
        {
            if (!_loaded)
            {
                _renderer.Init();
                LoadShaders("vertex6.glsl", "fragment6.glsl","outline6.glsl");
                _loaded = true;
            }
            Paint();
            Paint();
        }

        public void MouseDown(Vector2 pos, bool leftButton, bool rightButton, bool shift)
        {
            if(leftButton)
            {
                _scene.SelectObject(_renderer.ReadId(pos));
                SelectionChanged?.Invoke(_scene.SelectedObject);
                Paint();
            }       
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
            if (_scene.Projection == Scene.ProjectionEnum.Orthographic)
                _renderer.SetProjection(_scene.Projection);
            /*var cube = _scene.GetObject("cube");
            cube.Rotate(_scene.CameraDirection, delta*0.05f);*/
            Paint();
        }

        public void SetOrthographic(bool enabled)
        {
            _scene.Projection = enabled?Scene.ProjectionEnum.Orthographic:Scene.ProjectionEnum.Perspective;
            _renderer.SetProjection(_scene.Projection);
            Paint();
        }

        public void SetWireframe(bool enabled)
        {
            _scene.WireframeMode = enabled;
            Paint();
        }

        public void UpdateSelected(Vector3 pos,Vector3 rotation,Vector3 scale)
        {
            _scene.UpdateSelected(pos, rotation, scale);
            Paint();
        }
    }
}
