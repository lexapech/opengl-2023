using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using static ogl2.Surface;

namespace ogl2
{
    internal class Renderer : IRenderer
    {    
        private static readonly Color[] _colors = {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };
        private static readonly float[] _alphas = {1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f};
        private GLControl _viewport;
        private int _vao;
        private int _vbo;
        private int _ebo;
        private Mesh _mesh;
        private int _program = 0;
        public void DrawSelection(Rectangle rect)
        {
            _viewport.MakeCurrent();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(-1, -1, 0);
            GL.Scale(2.0 / _viewport.Width, 2.0 / _viewport.Height,1);         
            GL.Disable(EnableCap.CullFace);
            GL.LineWidth(1f);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
            GL.Color4(1f, 1f, 1f, 1.0f);
            GL.Begin(PrimitiveType.Quads);             
            GL.Vertex2(rect.Left, rect.Top);
            GL.Vertex2(rect.Left, rect.Bottom);
            GL.Vertex2(rect.Right, rect.Bottom);
            GL.Vertex2(rect.Right, rect.Top);
            
            
            GL.End();
            GL.PopMatrix();
        }

        public void SetViewport(GLControl viewport)
        {
            _viewport = viewport;
            _viewport.MakeCurrent();
             GL.EnableClientState(ArrayCap.VertexArray);
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();
        }

        public void Clear()
        {
            _viewport.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public void Resize()
        {
            _viewport.MakeCurrent();
            GL.Viewport(0, 0, _viewport.ClientSize.Width, _viewport.ClientSize.Height);
           
           /* GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _viewport.ClientSize.Width, 0, _viewport.ClientSize.Height, -1, 1);*/
        }

        public Size GetSize()
        {
            return _viewport.Size;
        }

        public void SwapBuffers()
        {
            _viewport.SwapBuffers();
        }

        public void Render(Fractal fractal)
        {
            _viewport.MakeCurrent();           
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(0, -1, 0);
            GL.Scale(1 / 200f / _viewport.Width*_viewport.Height, 1 / 200f,1);
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < fractal.Branches.Count; i++)
            {               
                GL.Color3(Color.Gray);
                var ortho = fractal.Branches[i].Direction.PerpendicularRight * fractal.Branches[i].Scale/50f;
                GL.Vertex2(fractal.Branches[i].Start + ortho);
                GL.Vertex2(fractal.Branches[i].Start - ortho);
                GL.Vertex2(fractal.Branches[i].End + ortho);
             
                GL.Vertex2(fractal.Branches[i].Start - ortho);
                GL.Vertex2(fractal.Branches[i].End + ortho);
                GL.Vertex2(fractal.Branches[i].End - ortho);

                GL.Vertex2(fractal.Branches[i].End + ortho);
                GL.Vertex2(fractal.Branches[i].End - ortho);
                GL.Vertex2(fractal.Branches[i].End + fractal.Branches[i].Direction * ortho.Length);
            }
            GL.End();
            GL.PopMatrix();
        }

        public void Render(RendererState model)
        {
            _viewport.MakeCurrent();
            
            GL.LineWidth(model.PrimitiveSize);
            GL.PointSize(model.PrimitiveSize);
            GL.Scissor(model.ScissorRegion.Left, model.ScissorRegion.Top,
                       model.ScissorRegion.Width, model.ScissorRegion.Height);
            GL.AlphaFunc(model.AlphaFunction, model.AlphaRef);
            GL.BlendFunc(model.BlendingFactorSrc, model.BlendingFactorDest);
            if(model.CullingEnabled) GL.Enable(EnableCap.CullFace);
            if(model.ScissorEnabled) GL.Enable(EnableCap.ScissorTest);
            if(model.AlphaTestEnabled) GL.Enable(EnableCap.AlphaTest);
            if(model.BlendingEnabled) GL.Enable(EnableCap.Blend);
            GL.CullFace(model.CullFace);
            GL.PolygonMode(MaterialFace.Front, model.PolygonModeFront);
            GL.PolygonMode(MaterialFace.Back, model.PolygonModeBack);
            GL.Begin(model.SelectedPrimitive);
            for (int i = 0; i < model.Vertices.Count; i++)
            {
                var color = _colors[i % _colors.Length];
                var alpha = _alphas[i % _alphas.Length];
                GL.Color4(color.R/255f, color.G / 255f, color.B / 255f, alpha);
                GL.Vertex2(model.Vertices[i].X, model.Vertices[i].Y);
            }
            GL.End();
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        private void RenderBezierCurve(Spline spline)
        {
            var points = spline.GetBezierPoints(); 
            
            for (int i=0; i< points.Length; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                for (int j = 0; j < points[i].Length/3; j++)
                {
                    GL.Color4(1f, 1f, 0f, 1f);
                    GL.Vertex2(points[i][3*j], points[i][3*j+1]);
                }
                GL.End();
                GL.Begin(PrimitiveType.Points);
                for (int j = 0; j < points[i].Length / 3; j++)
                {
                    GL.Color4(1f, 1f, 1f, 1f);
                    GL.Vertex2(points[i][3 * j], points[i][3 * j + 1]);
                }
                GL.End();

                GL.Map1(MapTarget.Map1Vertex3, 0.0f, 1.0f, 3, 4, points[i]);
                GL.Enable(EnableCap.Map1Vertex3);
                GL.Color3(0, 1.0, 0);
                GL.Begin(PrimitiveType.LineStrip);
                for (int j = 0; j <= 4 * spline.Steps; j++)
                    GL.EvalCoord1(((float)j) / (4 * spline.Steps));
                GL.End();
                GL.Disable(EnableCap.Map1Vertex3);
            }
                            
        }

        private void RenderSpline(Spline spline)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.LineWidth(2);
            for (int i = 0; i < spline.Vertices.Count; i++)
            {
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Vertex2(spline.Vertices[i].X, spline.Vertices[i].Y);
            }
            GL.End();

            GL.PointSize(5);

            GL.Begin(PrimitiveType.Points);
            for (int i = 1; i < spline.ControlPoints.Count - 1; i++)
            {
                GL.Color4(1f, 0f, 0f, 1f);
                GL.Vertex2(spline.ControlPoints[i].X, spline.ControlPoints[i].Y);
            }
            GL.End();
        }

        public void Render(Spline spline)
        {
            _viewport.MakeCurrent();
            if (spline.RenderCardinal)
                RenderSpline(spline);
            if (spline.RenderBezier)         
               RenderBezierCurve(spline);
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
            //GL.EnableClientState(ArrayCap.VertexArray);
            _mesh = mesh;
            //_vao = GL.GenVertexArray();
            //_vbo = GL.GenBuffer();
            //_ebo = GL.GenBuffer();
        }

        public void LoadShaders(string vertex,string fragment)
        {
            int shader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(shader, vertex);
            GL.CompileShader(shader);
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

        public void Render(Surface surface)
        {
            if(_mesh == null)
            {
                InitMesh(surface.Mesh);
            }
            UpdateMesh(surface.Mesh);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            var perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(30.0f),
                _viewport.AspectRatio,
                0.1f,
                100.0f);
            GL.LoadMatrix(ref perspectiveMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            float x = (float)Math.Cos(surface.CameraAngle.X) * (float)Math.Cos(surface.CameraAngle.Y);
            float z = (float)Math.Sin(surface.CameraAngle.X) * (float)Math.Cos(surface.CameraAngle.Y);
            float y = (float)Math.Sin(surface.CameraAngle.Y);

            var modelView = Matrix4.LookAt(new Vector3(x,y,z) * surface.CameraDistance, Vector3.Zero, Vector3.UnitY);
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
          
            DrawLight(surface.Point,modelView);
            GL.PopMatrix();

            GL.MatrixMode(MatrixMode.Projection);

            GL.PopMatrix();
           
            GL.Disable(EnableCap.Blend);
        }

        private void DrawLight(Vector3 pos,Matrix4 mat)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(pos);
            var mat1 =Matrix4.Transpose( mat.ClearTranslation());
            GL.MultMatrix(ref mat1);
           
            GL.Scale(0.05f, 0.05f,0.05f);
               
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(1f, 1f, 0);
            GL.Vertex3(0,0,0);
            for(int i = 0; i <= 16; i++)
            {
                GL.Vertex3(Math.Cos(i/16f*2*Math.PI), Math.Sin(i / 16f * 2 * Math.PI), 0);
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

        public void Render(Scene scene)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.Enable(EnableCap.DepthTest);
            var perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(30.0f),
                _viewport.AspectRatio,
                0.1f,
                100.0f);
            GL.LoadMatrix(ref perspectiveMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            
            var modelView = Matrix4.LookAt(scene.CameraDirection * scene.CameraDistance + scene.CameraFocus, scene.CameraFocus, Vector3.UnitY);
            //modelView = Matrix4.CreateTranslation(0, 0, -surface.Size/2)*modelView;
            GL.LoadMatrix(ref modelView);
            
            if (scene.ShowAxis)
            {
                GL.PushMatrix();
                DrawArrow(Color.Blue,2);
                GL.Rotate(90, Vector3.UnitY);
                DrawArrow(Color.Red,2);
                GL.Rotate(-90, Vector3.UnitX);
                DrawArrow(Color.Green, 2);
                GL.PopMatrix();
            }

            foreach(SceneObject sceneObject in scene.Objects)
            {
                var mesh = sceneObject.Mesh;
                GL.BindVertexArray(_vao);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * mesh.Vertices.Length, mesh.Vertices, BufferUsageHint.DynamicDraw);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * mesh.Indices.Length, mesh.Indices, BufferUsageHint.DynamicDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                
                GL.DrawElements(mesh.PrimitiveType, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);
            }

            GL.PopMatrix();

            GL.MatrixMode(MatrixMode.Projection);

            GL.PopMatrix();


            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        private void DrawArrow(Color color,float length)
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
                GL.Vertex3(Math.Cos(i / 16f * 2 * Math.PI)* size, Math.Sin(i / 16f * 2 * Math.PI)* size, length*0.9);
            }

            GL.End();
        }
    }
}
