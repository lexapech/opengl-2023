using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab3
{
    internal class Lab3Controller : ILab
    {
        private readonly Fractal _fractal;
        private readonly Lab3Renderer _renderer;
        public Fractal Fractal { get { return _fractal;}}

        public Lab3Controller(CommonRenderer commonRenderer)
        {
            _fractal = new Fractal();
            _renderer = new Lab3Renderer(commonRenderer, this);
        }

        public void SetFractalSteps(int steps)
        {
            _fractal.Steps = steps;
            _fractal.Generate();
            Paint();
        }

        public void ChangeFractalSeed()
        {
            _fractal.ChangeSeed();
            _fractal.Generate();
            Paint();
        }

        public void MouseDown(Vector2 pos, bool leftButton, bool rightButton, bool shift)
        {
            
        }

        public void MouseUp(Vector2 pos)
        {
         
        }

        public void MouseMove(Vector2 pos, Vector2 previousMousePos, bool leftButton, bool rightButton, bool shift)
        {
           
        }

        public void Zoom(int delta)
        {

        }

        public void Paint()
        {
            _renderer.CommonRenderer.Clear();
            _renderer.Render();
            _renderer.CommonRenderer.SwapBuffers();
        }

        public void Activate()
        {
            _fractal.Generate();
            Paint();
        }
    }
}
