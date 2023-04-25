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
using System.Windows.Forms;
using static ogl2.Scene;

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
        private int _lightShader = 0;
        private int _outlineShader = 0;
        private bool _shaderLoaded = false;
        private int[] _textures = new int[3];
        private Matrix4 _projection;

        public Lab6Renderer(CommonRenderer commonRenderer, Lab6Controller controller)
        {
            CommonRenderer = commonRenderer;
            _controller = controller;
            CommonRenderer.Resized += Resize;
        }

        private void Resize(Size newSize)
        {
            InitTextures();
            SetProjection(_controller.Scene.Projection);
        }

        private void InitTextures()
        {
            var size = CommonRenderer.GetSize();
            GL.BindTexture(TextureTarget.Texture2D, _textures[0]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size.Width, size.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, _textures[1]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8i, size.Width, size.Height, 0, PixelFormat.RedInteger, PixelType.Int, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, _textures[2]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, size.Width, size.Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public void Init()
        {
            GL.Enable(EnableCap.StencilTest);
            float[] ambientColor = new float[4] {1, 0, 0, 0};
            //GL.LightModel(LightModelParameter.LightModelAmbient,ambientColor);
            GL.Enable(EnableCap.Normalize);
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
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, _textures[2], 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            SetProjection(_controller.Scene.Projection);

        }

        public void LoadShaders(string vertex, string fragment,string outline)
        {
            int shader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(shader, vertex);
            GL.CompileShader(shader);
            Console.WriteLine("Shader log:");
            Console.WriteLine(GL.GetShaderInfoLog(shader));
            int shader2 = GL.CreateShader(ShaderType.FragmentShader);
            int shader3 = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(shader2, fragment);
            GL.ShaderSource(shader3, outline);
            GL.CompileShader(shader2);
            GL.CompileShader(shader3);
            Console.WriteLine(GL.GetShaderInfoLog(shader2));
            Console.WriteLine(GL.GetShaderInfoLog(shader3));
            int program = GL.CreateProgram();
            int program2 = GL.CreateProgram();
            GL.AttachShader(program, shader);
            GL.AttachShader(program2, shader);
            GL.AttachShader(program2, shader3);
            GL.AttachShader(program, shader2);
            GL.LinkProgram(program);
            GL.LinkProgram(program2);
            Console.WriteLine("Program");
            Console.WriteLine(GL.GetProgramInfoLog(program));
            Console.WriteLine(GL.GetProgramInfoLog(program2));
            _lightShader = program;
            _outlineShader = program2;
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

        public void SetProjection(ProjectionEnum projection)
        {
            switch (projection)
            {
                case ProjectionEnum.Perspective:
                    _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(30.0f),
                CommonRenderer.AspectRatio,
                0.1f,
                100.0f);
                    break;
                case ProjectionEnum.Orthographic:
                    _projection = Matrix4.CreateOrthographic(_controller.Scene.CameraDistance, _controller.Scene.CameraDistance/CommonRenderer.AspectRatio, 0.1f, 100f);
                    break;
            }            
        }

        private void SetupLight(Light light)
        {
            GL.Light(LightName.Light0, LightParameter.Position, light.Position);
            GL.Light(LightName.Light0, LightParameter.Ambient, light.Ambient);
            GL.Light(LightName.Light0, LightParameter.Specular, light.Specular);
            GL.Light(LightName.Light0, LightParameter.SpotExponent, light.SpotExponent);
            GL.Light(LightName.Light0, LightParameter.SpotCutoff, light.SpotCutoff);
            GL.Light(LightName.Light0, LightParameter.LinearAttenuation, light.LinearAttenuation);
            GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, light.ConstantAttenuation);
            GL.Light(LightName.Light0, LightParameter.QuadraticAttenuation, light.QuadraticAttenuation);
            GL.Light(LightName.Light0, LightParameter.Diffuse, light.Diffuse);
            GL.Light(LightName.Light0, LightParameter.SpotDirection, light.SpotDirection);

            if (light.Enabled) 
                GL.Enable(EnableCap.Light0);
            else 
                GL.Disable(EnableCap.Light0);

        }

        public void Render()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            var scene = _controller.Scene;
            GL.Enable(EnableCap.Lighting);

          
            var view = LoadMatricesAndGetView(scene);
           
            ClearFramebuffer();

            SetupLight(scene.Light);

            foreach (SceneObject sceneObject in scene.Objects.OrderBy(x=>x.Id == scene.SelectedId?0:1))
            {
                DrawObject(sceneObject, scene, view);
            }
            GL.Disable(EnableCap.Lighting);
            if (scene.ShowAxis) DrawAxis();

            CopyFramebuffer();
            RestoreMatrices();

            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Light0);
        }

        private void ClearFramebuffer()
        {
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
        }

        private Matrix4 LoadMatricesAndGetView(Scene scene)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadMatrix(ref _projection);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            var view = Matrix4.LookAt(scene.CameraPosition, scene.CameraFocus, Vector3.UnitY);
            GL.LoadMatrix(ref view);
            return view;
        }

        private void BindVAO(Mesh mesh,bool shader)
        {                             
            GL.BindVertexArray(_vao);
            
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * mesh.Vertices.Length, mesh.Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * mesh.Indices.Length, mesh.Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 0);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 10 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 3 * sizeof(float));
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.NormalPointer(NormalPointerType.Float, 10 * sizeof(float), 3 * sizeof(float));

            GL.EnableClientState(ArrayCap.ColorArray);
            if(shader)
            {
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 10 * sizeof(float), 6 * sizeof(float));
            }
            else
            {
                GL.DisableVertexAttribArray(2);
            }
            GL.ColorPointer(4, ColorPointerType.Float, 10 * sizeof(float), 6 * sizeof(float));

            

        }

        private void DrawObject(SceneObject sceneObject,Scene scene,Matrix4 view)
        {
            var mesh = sceneObject.Mesh;
            
            var model = Matrix4.CreateScale(sceneObject.AbsScale) * Matrix4.CreateFromQuaternion(sceneObject.Rotation) *
                Matrix4.CreateTranslation(sceneObject.Position);

            GL.MatrixMode(MatrixMode.Modelview);           

            GL.PushMatrix();

            var modelView = model * view;
            GL.LoadMatrix(ref modelView);

            

            bool outlined = sceneObject.Id == scene.SelectedId;                
            var vp = view * _projection;
            int mainShader = scene.WireframeMode ? _outlineShader : _lightShader;

           // GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, sceneObject.Material.Ambient);
            GL.Enable(EnableCap.ColorMaterial);
            // GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, sceneObject.Material.Diffuse);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, sceneObject.Material.Specular);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, sceneObject.Material.Shininess);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, sceneObject.Material.Emission);

            BindVAO(mesh,scene.UseShader);
            if (scene.UseShader) GL.UseProgram(mainShader);
            GL.BindAttribLocation(mainShader, 0, "vertexPosition");
            GL.BindAttribLocation(mainShader, 1, "normal");
            GL.BindAttribLocation(mainShader, 2, "color");

            GL.UniformMatrix4(GL.GetUniformLocation(mainShader, "mMatrix"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(mainShader, "vpMatrix"), false, ref vp);
            if (scene.WireframeMode)
            {
                GL.Uniform4(GL.GetUniformLocation(mainShader, "color"), Utility.ConvertColor(Color.White));
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.Uniform3(GL.GetUniformLocation(mainShader, "lightSource"), scene.LightPosition);
                GL.Uniform3(GL.GetUniformLocation(mainShader, "cameraPos"), scene.CameraPosition);
                GL.Uniform1(GL.GetUniformLocation(mainShader, "id"), sceneObject.Id);
            }

            if (outlined)
            {
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
                GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            }
            if (scene.Transparent)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            }

            GL.DrawElements(mesh.PrimitiveType, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.DisableClientState( ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            //DrawCube();

            GL.Disable(EnableCap.Blend);

            if (outlined && !scene.WireframeMode)
            {
                GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
                GL.StencilMask(0x00);

                model = Matrix4.CreateScale(1 + 0.02f / sceneObject.AbsScale.X, 1 + 0.02f / sceneObject.AbsScale.Y, 1 + 0.02f / sceneObject.AbsScale.Z) * model;
                GL.UseProgram(_outlineShader);
                GL.BindAttribLocation(_outlineShader, 0, "vertexPosition");
                GL.BindAttribLocation(_outlineShader, 1, "normal");
                GL.BindAttribLocation(_outlineShader, 2, "color");
                GL.UniformMatrix4(GL.GetUniformLocation(_outlineShader, "mMatrix"), false, ref model);
                GL.UniformMatrix4(GL.GetUniformLocation(_outlineShader, "vpMatrix"), false, ref vp);
                GL.Uniform4(GL.GetUniformLocation(_outlineShader, "color"), Utility.ConvertColor(Color.Orange));

                GL.DrawElements(mesh.PrimitiveType, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);

                GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
                GL.StencilMask(0xFF);

            }
            GL.PopMatrix();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.UseProgram(0);
            GL.BindVertexArray(0);
        }


        private void DrawAxis()
        {
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            GL.PushMatrix();
            DrawArrow(Color.Blue, 2);
            GL.Rotate(90, Vector3.UnitY);
            DrawArrow(Color.Red, 2);
            GL.Rotate(-90, Vector3.UnitX);
            DrawArrow(Color.Green, 2);
            GL.PopMatrix();
        }

        private void CopyFramebuffer()
        {
            var size = CommonRenderer.GetSize();
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _fbo);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.BlitFramebuffer(0, 0, size.Width, size.Height,
                               0, 0, size.Width, size.Height,
                               ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit,
                               BlitFramebufferFilter.Nearest);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        private void RestoreMatrices()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
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
