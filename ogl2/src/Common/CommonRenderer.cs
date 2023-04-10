using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal class CommonRenderer
    {
        private GLControl _viewport;

        public delegate void ResizeDelegate(Size newSize);

        public event ResizeDelegate Resized;

        public void SetViewport(GLControl viewport)
        {
            _viewport = viewport;
            _viewport.MakeCurrent();
           
        }

        public float AspectRatio
        {
            get {
                return _viewport.AspectRatio;
            }
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
            Resized?.Invoke(_viewport.Size);
        }

        public Size GetSize()
        {
            return _viewport.Size;
        }

        public void SwapBuffers()
        {
            _viewport.SwapBuffers();
        }

        public void MakeCurrent()
        {
            _viewport.MakeCurrent();
        }
    }
}
