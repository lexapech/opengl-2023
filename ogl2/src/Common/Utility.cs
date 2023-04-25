using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal class Utility
    {
        public static Vector2 ConvertMousePos(Vector2 pos, Size viewportSize)
        {
            var normalized = new Vector2(pos.X / viewportSize.Width, pos.Y / viewportSize.Height);
            return normalized * 2 - Vector2.One;
        }

        public static Rectangle PointsToRect(Vector2 point1, Vector2 point2)
        {
            var min = new Vector2(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            var max = new Vector2(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y));
            return Rectangle.FromLTRB((int)min.X, (int)min.Y, (int)max.X, (int)max.Y);
        }

        public static Vector4 ConvertColor(Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        public static Color ConvertColor(Vector4 color)
        {
            return Color.FromArgb(((int)(color.X * 255f)) + ((int)(color.Y * 255f)<<8) + ((int)(color.Z * 255f)<<16) + (255<<24));
        }

        public static float Clamp(float value,float min,float max) 
        {
            return Math.Max(min, Math.Min(value, max));
        }

    }
}
