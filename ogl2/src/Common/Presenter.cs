using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;
using ogl2.src.Lab3;
using ogl2.src.Lab4;
using ogl2.src.Lab5;
using ogl2.src.Lab6;

namespace ogl2
{ 
    internal class Presenter
    {
        public Lab12Controller Lab12;
        public Lab3Controller Lab3;
        public Lab4Controller Lab4;
        public Lab5Controller Lab5;
        public Lab6Controller Lab6;
        private ILab _currentLab;

        private readonly CommonRenderer _renderer;
        
        private Vector2 _previousMousePosition;

        public Presenter()
        {
            _renderer = new CommonRenderer();         
            Lab12 = new Lab12Controller(_renderer);
            Lab3 = new Lab3Controller(_renderer);
            Lab4 = new Lab4Controller(_renderer);
            Lab5 = new Lab5Controller(_renderer, Lab4.Spline);
            Lab6 = new Lab6Controller(_renderer);
            _currentLab = Lab12;

        }


        public void SetViewport(GLControl viewport)
        {
            Lab12.RendererState.Viewport = viewport;
            _renderer.SetViewport(viewport);            
            Lab12.RendererState.ScissorRegion = new Rectangle(0, 0, viewport.ClientSize.Width, viewport.ClientSize.Height);
        }

        public void Resize()
        {
            _renderer.Resize();
        }

        public void MouseDown(Vector2 point,bool leftButton, bool rightButton, bool shift)
        {
            _currentLab.MouseDown(point, leftButton, rightButton, shift);
            _previousMousePosition = point;
        }
        
        public void MouseMove(Vector2 point,bool leftButton, bool rightButton, bool shift)
        {
            _currentLab.MouseMove(point,_previousMousePosition,leftButton,rightButton,shift);
            if (rightButton)
                _previousMousePosition = point;
        }

        public void TabChanged(int index) 
        {
            switch (index)
            {
                case 0: _currentLab = Lab12; break;
                case 1: _currentLab = Lab12; break;
                case 2: _currentLab = Lab3; break;
                case 3: _currentLab = Lab4; break;
                case 4: _currentLab = Lab5; break;
                case 5: _currentLab = Lab6; break;
                default: break;
            }
            _currentLab.Activate();

            Lab4.CursorChangeHandler?.Invoke(false);
        }

        public void MouseUp(Vector2 point)
        {
            _currentLab.MouseUp(point);
        }
       
        public void Paint()
        {
            _currentLab.Paint();
        }
                    
        public void Zoom(int delta)
        {
            _currentLab.Zoom(delta);
        }
    }
}
