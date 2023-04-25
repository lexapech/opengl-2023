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
                CameraAngle = new Vector2((float)(90 / 180f * Math.PI), (float)(0 / 180f * Math.PI)),
                CameraDistance = 15,
                Light = new Light 
                {
                    Enabled = true,
                    Position = new Vector4(10, 10, 10, 0),
                    Ambient = new Vector4(0, 0, 0, 1),
                    Diffuse = new Vector4(1, 1, 1, 1f),
                    Specular = new Vector4(1, 1, 1, 1),
                    SpotDirection = new Vector4(-1, -1, -1, 1).Normalized(),
                    SpotCutoff = 180,
                    SpotExponent = 0,
                    ConstantAttenuation = 1,
                    LinearAttenuation = 0,
                    QuadraticAttenuation = 0
                }
            };
            InitScene();
        }

        private void InitScene()
        {
            _scene.AddObject("cube", new CubeGenerator()
                .SetColor(Color.LightBlue))
                .Translate(new Vector3(-2.5f, 1.5f, 0))
                .EulerAngles = new Vector3 (40, -25 , 30) / 180f * (float)Math.PI;
            _scene.AddObject("cube2", new CubeGenerator()
                .SetColor(Color.LightBlue))
                .Translate(new Vector3(3.6f, -0.3f, 0))
                .Scale(new Vector3(0.7f,0.1f,0.25f))
                .EulerAngles = new Vector3(55, -30, 30) / 180f * (float)Math.PI;
            _scene.AddObject("sphere1", new SphereGenerator(16)
                .SetColor(Color.LightGray))
                .Translate(new Vector3(-0.5f, 1.9f, 0.3f));
            _scene.AddObject("sphere2", new SphereGenerator(16)
                .SetColor(Color.LightGoldenrodYellow))
                .Translate(new Vector3(-1.1f, 0, 0.3f))
                .Scale(Vector3.One* 0.3f);
            _scene.AddObject("cyl1", new CylinderGenerator(16)
                .SetColor(Color.Purple))
                .Translate(new Vector3(-1.8f, -2.1f, 0))
                .Scale(new Vector3(0.5f, 1.9f, 0.5f))
                .EulerAngles = new Vector3(160, -30, 50) / 180f * (float)Math.PI;
            _scene.AddObject("cyl2", new CylinderGenerator(3)
                .SetNormalsSmoothing(false)
                .SetColor(Color.Aquamarine))
                .Translate(new Vector3(0.9f, -0f, 0))
                .Scale(new Vector3(0.5f, 0.5f, 0.5f))
                .EulerAngles = new Vector3(100, -160, 0) / 180f * (float)Math.PI;
            _scene.AddObject("cyl3", new CylinderGenerator(6)
                .SetNormalsSmoothing(false)
                .SetColor(Color.PaleVioletRed))
                .Translate(new Vector3(1.9f, -1.4f, 0))
                .Scale(new Vector3(0.5f, 0.3f, 0.5f))
                .EulerAngles = new Vector3(85, -170, -35) / 180f * (float)Math.PI;
            _scene.AddObject("stair1", new StairsGenerator(5)
                .SetColor(Color.Beige))
                .Translate(new Vector3(-3.5f, -1.4f, 0))
                .Scale(new Vector3(1f, 1f, 1f))
                .EulerAngles = new Vector3(-100, -110, -120) / 180f * (float)Math.PI;
            _scene.AddObject("stair2", new StairsGenerator(5)
                .SetColor(Color.DarkViolet))
                .Translate(new Vector3(1.4f, -2.9f, 0.6f))
                .Scale(new Vector3(1.5f, 1.9f, 1.2f))
                .EulerAngles = new Vector3(-90, -10, -90) / 180f * (float)Math.PI;
            _scene.AddObject("pipe1", new PipeGenerator(16,0.5f)
                .SetColor(Color.Cyan))
                .Translate(new Vector3(3.2f, 1.9f, 0.3f))
                .Scale(new Vector3(0.4f, 1.7f, 0.4f))
                .EulerAngles = new Vector3(110, 0, 30) / 180f * (float)Math.PI;


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
        public void SetTransparent(bool enabled)
        {
            _scene.Transparent = enabled;
            Paint();
        }

        public void SetCoordAxis(bool enabled)
        {
            _scene.ShowAxis = enabled;
            Paint();
        }

        public void EnableShader(bool enabled)
        {
            _scene.UseShader = enabled;
            Paint();
        }

        public void SetLightColors(Color ambient,Color diffuse, Color specular)
        {
            _scene.Light.Ambient = Utility.ConvertColor(ambient);
            _scene.Light.Diffuse = Utility.ConvertColor(diffuse);
            _scene.Light.Specular = Utility.ConvertColor(specular);
            Paint();
        }

        public void SetLightPosition(Vector4 vector)
        {
            _scene.Light.Position = vector;
            Paint();
        }

        public void SetLightDirection(Vector3 pos)
        {
            _scene.Light.SpotDirection =new Vector4( (pos- _scene.Light.Position.Xyz).Normalized());
            Paint();
        }

        public void EnableLight(bool enabled)
        {
            _scene.Light.Enabled = enabled;
            Paint();
        }

        public void SetLightProps(float exp,float cutoff,float conAtt,float linAtt,float quadAtt)
        {
            if (cutoff > 90) cutoff = 180;
            _scene.Light.SpotExponent = exp;
            _scene.Light.SpotCutoff = cutoff;
            _scene.Light.ConstantAttenuation = conAtt;
            _scene.Light.LinearAttenuation = linAtt;
            _scene.Light.QuadraticAttenuation = quadAtt;
            Paint();
        }

        public void UpdateSelected(Vector3 pos,Vector3 rotation,Vector3 scale,int steps,Color specular, Color emission, float shinihess)
        {
            if(steps<3) steps = 3;
            _scene.UpdateSelected(pos, rotation, scale, steps, specular, emission, shinihess);
            Paint();
        }
    }
}
