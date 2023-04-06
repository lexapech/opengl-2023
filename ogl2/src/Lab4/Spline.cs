using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ogl2
{
    internal class Spline
    {
        public List<Vector2> ControlPoints = new List<Vector2>();
        public List<Vector2> Vertices = new List<Vector2>();
        public int Steps = 10;
        public float Scale = 0f;
        public bool RenderBezier = false;
        public bool RenderCardinal = true;
        private readonly float _controlPointRadius = 0.03f;
        private int _grabbedControlPointIndex = -1;
        public Spline()
        {
            AddControlPoint(new Vector2(-0.5f, -0.5f));
            AddControlPoint(new Vector2(-0.5f, -0.5f));
            AddControlPoint(new Vector2(0, 0));
            AddControlPoint(new Vector2(0.5f, -0.5f));
            AddControlPoint(new Vector2(0.5f, -0.5f));
        }

        public void Generate()
        {
            Vertices.Clear();
            SetEndPoints();
            Vertices.Add(ControlPoints[1]);
            for (int i = 0; i + 4 <= ControlPoints.Count; i++)
            {
                Vertices.AddRange(Interpolate(ControlPoints.GetRange(i, 4).ToArray(), Steps));
            }        
        }



        public void GrabControlPoint(int index)
        {
            _grabbedControlPointIndex = index;
        }

        public void ReleaseControlPoint()
        {
            _grabbedControlPointIndex = -1;
        }

        public bool MoveGrabbedControlPoint(Vector2 pos)
        {
            if(_grabbedControlPointIndex != -1)
            {
                ControlPoints[_grabbedControlPointIndex] = pos;
                return true;
            }
            return false; 
        }

        public int NearControlPoint(Vector2 pos)
        {
            for(int i = 1; i < ControlPoints.Count-1; i++)
            {
                if ((ControlPoints[i] - pos).Length < _controlPointRadius)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Clear()
        {
            ControlPoints.Clear();
            Vertices.Clear();
        }
        public void AddControlPoint(Vector2 pos)
        {
            ControlPoints.Add(pos);
        }
        public void SetEndPoints()
        {    
            ControlPoints[0] = 2 * ControlPoints[1] - ControlPoints[2];
            ControlPoints[ControlPoints.Count - 1] = 2 * ControlPoints[ControlPoints.Count - 2] - ControlPoints[ControlPoints.Count - 3];          
        }

        public List<Vector2> Interpolate(Vector2[] points,int steps)
        {
            float s = Scale;
            List<Vector2> points2 = new List<Vector2>();
            var curveMatrix = new Matrix4(
                0, 1, 0, 0,
                -s, 0, s, 0,
                2 * s, s - 3, 3 - 2 * s, -s,
                -s, 2 - s, s - 2, s);       
            var pointMatrix = new Matrix4();
            pointMatrix.Row0 = new Vector4(points[0]);
            pointMatrix.Row1 = new Vector4(points[1]);
            pointMatrix.Row2 = new Vector4(points[2]);
            pointMatrix.Row3 = new Vector4(points[3]);
            steps++;
            for (int i = 1; i < steps; i++)
            {
                float t = ((float)i) / steps;
                Vector4 tVector = new Vector4(1, t, t * t, t * t * t);
                var res = tVector * curveMatrix * pointMatrix;
                points2.Add(res.Xy);
            }
            points2.Add(points[2]);
            return points2;
        }

        public float[][] GetBezierPoints()
        {
            List<Vector2> newPoints = new List<Vector2>();
            var segments = ControlPoints.Count - 3;
            float s = Scale / 3;
            for(int i = 0; i < segments; i++)
            {
                newPoints.Add(ControlPoints[i + 1]);
                newPoints.Add(ControlPoints[i + 1] + s * (ControlPoints[i + 2] - ControlPoints[i + 0]));
                newPoints.Add(ControlPoints[i + 2] - s * (ControlPoints[i + 3] - ControlPoints[i + 1]));
                newPoints.Add(ControlPoints[i + 2]);
            }            
            float[][] points = new float[segments][];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new float[newPoints.Count / segments * 3];
            }
            for (int i = 0; i < newPoints.Count; i++)
            {
                points[i / 4][3 * (i % 4)] = newPoints[i].X;
                points[i / 4][3 * (i % 4) + 1] = newPoints[i].Y;
                points[i / 4][3 * (i % 4) + 2] = 0;
            }
            return points;
        }
    }
}
