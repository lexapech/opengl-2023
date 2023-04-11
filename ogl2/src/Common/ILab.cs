using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal interface ILab
    {
        void MouseDown(Vector2 pos, bool leftButton, bool rightButton, bool shift);
        void MouseUp(Vector2 pos);
        void MouseMove(Vector2 pos, Vector2 previousMousePos, bool leftButton, bool rightButton, bool shift);
        void Zoom(int delta);
        void Activate();
        void Paint();
    }
}
