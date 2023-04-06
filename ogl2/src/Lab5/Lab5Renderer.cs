using ogl2.src.Lab4;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;



namespace ogl2.src.Lab5
{
    internal class Lab5Renderer
    {
        public CommonRenderer CommonRenderer;
        private readonly Lab5Controller _controller;

        private int _vao;
        private int _vbo;
        private int _ebo;
        private Mesh _mesh;
        private int _program = 0;

        public Lab5Renderer(CommonRenderer commonRenderer, Lab5Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;      
        }

        private void UpdateMesh(Mesh mesh)
        {
            _mesh = mesh;
            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * mesh.Vertices.Length, mesh.Vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * mesh.Indices.Length, mesh.Indices, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        private void InitMesh(Mesh mesh)
        {
            _mesh = mesh;
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
            _program = program;

        }

        public void Render()
        {
            var surface = _controller.Surface;
            CommonRenderer.MakeCurrent();
            if (_mesh == null)
            {
                InitMesh(surface.Mesh);
            }
            UpdateMesh(surface.Mesh);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            var perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(30.0f),
                CommonRenderer.AspectRatio,
                0.1f,
                100.0f);
            GL.LoadMatrix(ref perspectiveMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            float x = (float)Math.Cos(surface.CameraAngle.X) * (float)Math.Cos(surface.CameraAngle.Y);
            float z = (float)Math.Sin(surface.CameraAngle.X) * (float)Math.Cos(surface.CameraAngle.Y);
            float y = (float)Math.Sin(surface.CameraAngle.Y);

            var modelView = Matrix4.LookAt(new Vector3(x, y, z) * surface.CameraDistance, Vector3.Zero, Vector3.UnitY);
            //modelView = Matrix4.CreateTranslation(0, 0, -surface.Size/2)*modelView;
            GL.LoadMatrix(ref modelView);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            //GL.Color4(1f, 1f, 0f, 1f);
            GL.BindVertexArray(_vao);
            GL.UseProgram(_program);
            GL.BindAttribLocation(_program, 0, "vertexPosition");
            GL.BindAttribLocation(_program, 1, "normal");
            GL.UniformMatrix4(GL.GetUniformLocation(_program, "projectionMatrix"), false, ref perspectiveMatrix);
            GL.UniformMatrix4(GL.GetUniformLocation(_program, "modelViewMatrix"), false, ref modelView);
            GL.Uniform3(GL.GetUniformLocation(_program, "color"), new Vector3(0.8f, 0.8f, 0.8f));
            GL.Uniform3(GL.GetUniformLocation(_program, "point"), surface.Point);
            GL.DrawElements(PrimitiveType.Quads, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.UseProgram(0);
            GL.BindVertexArray(0);

            DrawLight(surface.Point, modelView);
            GL.PopMatrix();

            GL.MatrixMode(MatrixMode.Projection);

            GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
        }

        private void DrawLight(Vector3 pos, Matrix4 mat)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(pos);
            var mat1 = Matrix4.Transpose(mat.ClearTranslation());
            GL.MultMatrix(ref mat1);

            GL.Scale(0.05f, 0.05f, 0.05f);

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(1f, 1f, 0);
            GL.Vertex3(0, 0, 0);
            for (int i = 0; i <= 16; i++)
            {
                GL.Vertex3(Math.Cos(i / 16f * 2 * Math.PI), Math.Sin(i / 16f * 2 * Math.PI), 0);
            }

            GL.End();
            GL.Begin(PrimitiveType.Lines);
            float r1 = 1.5f;
            float r2 = 2f;
            for (int i = 0; i <= 16; i++)
            {
                GL.Vertex3(r1 * Math.Cos(i / 16f * 2 * Math.PI), r1 * Math.Sin(i / 16f * 2 * Math.PI), 0);
                GL.Vertex3(r2 * Math.Cos(i / 16f * 2 * Math.PI), r2 * Math.Sin(i / 16f * 2 * Math.PI), 0);
            }
            GL.End();
            GL.PopMatrix();
        }

    }
}
