﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal interface IRenderer
    {
        void SetViewport(GLControl viewport);
        void Render(RendererState model);
        void Render(Fractal fractal);
        void Render(Spline spline);
        void SwapBuffers();
        void DrawSelection(Rectangle rect);
        void Clear();

        Size GetSize();
        void Resize();
    }
}
