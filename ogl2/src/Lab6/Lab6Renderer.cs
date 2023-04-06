using ogl2.src.Lab5;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal class Lab6Renderer
    {
        public CommonRenderer CommonRenderer;
        private Lab6Controller _controller;

        private int _vao;
        private int _vbo;
        private int _ebo;
        private int _program = 0;
        private bool _shaderLoaded = false;

        public Lab6Renderer(CommonRenderer commonRenderer, Lab6Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;
        }

        public void Init()
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();
        }

        public void LoadShaders(string vertex, string fragment)
        {
            int shader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(shader, vertex);
            GL.CompileShader(shader);
            Console.WriteLine("Shader log:");
            Console.WriteLine(GL.GetShaderInfoLog(shader));
            int shader2 = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(shader2, fragment);
            GL.CompileShader(shader2);
            Console.WriteLine(GL.GetShaderInfoLog(shader2));
            int program = GL.CreateProgram();
            GL.AttachShader(program, shader);
            GL.AttachShader(program, shader2);
            GL.LinkProgram(program);
            Console.WriteLine("Program");
            Console.WriteLine(GL.GetProgramInfoLog(program));
            _program = program;
            _shaderLoaded = true;

        }

        public void Render()
        {
            var scene = _controller.Scene;
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.Enable(EnableCap.DepthTest);
            var perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(30.0f),
                CommonRenderer.AspectRatio,
                0.1f,
                100.0f);
            GL.LoadMatrix(ref perspectiveMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            var view = Matrix4.LookAt(scene.CameraDirection * scene.CameraDistance + scene.CameraFocus, scene.CameraFocus, Vector3.UnitY);
            //modelView = Matrix4.CreateTranslation(0, 0, -surface.Size/2)*modelView;
            GL.LoadMatrix(ref view);

            if (scene.ShowAxis)
            {
                GL.PushMatrix();
                DrawArrow(Color.Blue, 2);
                GL.Rotate(90, Vector3.UnitY);
                DrawArrow(Color.Red, 2);
                GL.Rotate(-90, Vector3.UnitX);
                DrawArrow(Color.Green, 2);
                GL.PopMatrix();
            }

            foreach (SceneObject sceneObject in scene.Objects)
            {
                var mesh = sceneObject.Mesh;

                //modelView1 *= Matrix4.CreateFromQuaternion(sceneObject.Rotation);
                var model = Matrix4.CreateScale(sceneObject.AbsScale) * Matrix4.CreateFromQuaternion(sceneObject.Rotation) *

                    Matrix4.CreateTranslation(sceneObject.Position);

                GL.BindVertexArray(_vao);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * mesh.Vertices.Length, mesh.Vertices, BufferUsageHint.DynamicDraw);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * mesh.Indices.Length, mesh.Indices, BufferUsageHint.DynamicDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 3 * sizeof(float));

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 10 * sizeof(float), 6 * sizeof(float));
                GL.UseProgram(_program);
                
                GL.BindAttribLocation(_program, 0, "vertexPosition");
                GL.BindAttribLocation(_program, 1, "normal");
                GL.BindAttribLocation(_program, 2, "color");
                var vp = view * perspectiveMatrix;
                GL.UniformMatrix4(GL.GetUniformLocation(_program, "mMatrix"), false, ref model);
                GL.UniformMatrix4(GL.GetUniformLocation(_program, "vpMatrix"), false, ref vp);

                GL.DrawElements(mesh.PrimitiveType, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.UseProgram(0);
                GL.BindVertexArray(0);
               
            }

            GL.PopMatrix();

            GL.MatrixMode(MatrixMode.Projection);

            GL.PopMatrix();


            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        private void DrawArrow(Color color, float length)
        {
            float size = 0.1f;
            GL.Color3(color);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, length);
            GL.End();
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(0, 0, length);
            for (int i = 0; i <= 16; i++)
            {
                GL.Vertex3(Math.Cos(i / 16f * 2 * Math.PI) * size, Math.Sin(i / 16f * 2 * Math.PI) * size, length * 0.9);
            }

            GL.End();
        }


    }
}
