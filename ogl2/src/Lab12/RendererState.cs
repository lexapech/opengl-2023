using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ogl2
{
    internal class RendererState
    {

        public readonly Color[] Colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };
        public readonly float[] Alphas = { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f };

        public GLControl Viewport;
        public PrimitiveType SelectedPrimitive;
        public List<Vector2> Vertices = new List<Vector2>();
        public float PrimitiveSize;
        public bool CullingEnabled;
        public bool ScissorEnabled;
        public bool AlphaTestEnabled;
        public bool BlendingEnabled;
        public CullFaceMode CullFace;
        public PolygonMode PolygonModeFront;
        public PolygonMode PolygonModeBack;
        public AlphaFunction AlphaFunction;
        public float AlphaRef;
        public BlendingFactor BlendingFactorSrc;
        public BlendingFactor BlendingFactorDest;

        public Rectangle ScissorRegion;
       

        public RendererState()
        {
            SelectedPrimitive = PrimitiveType.Points;
            BlendingFactorSrc = BlendingFactor.Zero;
            BlendingFactorDest = BlendingFactor.Zero;
            PrimitiveSize = 1;
            CullingEnabled = false;
            ScissorEnabled = false;
            AlphaTestEnabled = false;
            BlendingEnabled = false;
            AlphaFunction = AlphaFunction.Always;
            AlphaRef = 0;
            
            PolygonModeFront = PolygonMode.Fill;
            PolygonModeBack = PolygonMode.Fill;
            CullFace = CullFaceMode.Front;
            ScissorRegion = new Rectangle(0, 0, 0, 0);
           
        }
    }
}
