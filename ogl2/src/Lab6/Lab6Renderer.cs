using ogl2.src.Lab5;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.MacOS;
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
        private int _fbo;
        private int _program = 0;
        private bool _shaderLoaded = false;
        private int[] _textures = new int[3];

        public Lab6Renderer(CommonRenderer commonRenderer, Lab6Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;
            CommonRenderer.Resized += Resize;
        }

        private void Resize(Size newSize)
        {
            InitTextures();
        }

        private void InitTextures()
        {
            var size = CommonRenderer.GetSize();
            GL.BindTexture(TextureTarget.Texture2D, _textures[0]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size.Width, size.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, _textures[1]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8i, size.Width, size.Height, 0, PixelFormat.RedInteger, PixelType.Int, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, _textures[2]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, size.Width, size.Height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public void Init()
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();
            _fbo = GL.GenFramebuffer();
            GL.GenTextures(_textures.Length, _textures);
            for(int i=0;i<_textures.Length;i++)
            {
                GL.BindTexture(TextureTarget.Texture2D, _textures[i]);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            InitTextures();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _textures[0], 0);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, _textures[1], 0);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, _textures[2], 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
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

        public int ReadId(Vector2 pos)
        {
            int[] pixels = new int[1];
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _fbo);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
            GL.ReadPixels((int)pos.X, (int)pos.Y, 1, 1, PixelFormat.RedInteger, PixelType.UnsignedByte,pixels);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            return pixels[0];
        }

        public void Render()
        {
            var scene = _controller.Scene;
            var size = CommonRenderer.GetSize();
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


            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
            DrawBuffersEnum[] drawBuffersEnum = new DrawBuffersEnum[]{
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1
            };
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
           
            CommonRenderer.Clear();
            
            GL.DrawBuffer(DrawBufferMode.ColorAttachment1);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.DrawBuffers(drawBuffersEnum.Length, drawBuffersEnum);

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
                GL.Uniform1(GL.GetUniformLocation(_program, "id"),sceneObject.Id);
                GL.Uniform3(GL.GetUniformLocation(_program, "lightSource"), new Vector3(2,2,2));

                GL.DrawElements(mesh.PrimitiveType, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.UseProgram(0);
                GL.BindVertexArray(0);
               
            }

            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
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



            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);  // if not already bound
            //GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            GL.BlitFramebuffer(0, 0, size.Width, size.Height, 0, 0, size.Width, size.Height,
                              ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit,BlitFramebufferFilter.Nearest);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

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
