using ogl2.src.Lab6;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ogl2.Presenter;

namespace ogl2
{
    internal class Scene
    {
        public List<SceneObject> Objects = new List<SceneObject>();
        public bool ShowAxis = true;
        public Vector3 CameraFocus;
        public float CameraDistance;
        public Vector2 CameraAngle;

        public Scene()
        {
            CameraDistance = 2;
            CameraFocus = Vector3.Zero;
        }

        public void AddObject(string name, MeshGenerator generator, Vector3 pos)
        {
            var obj = new SceneObject(name,generator);
            obj.Position = pos;
            Objects.Add(obj);
        }

        public SceneObject GetObject(string name)
        {
            return Objects.Find(x => x.Name == name);
        }

        public void AddObject(string name, MeshGenerator generator)
        {
            AddObject(name, generator, Vector3.Zero);
        }
        public void RotateCamera(Vector2 delta)
        {
            CameraAngle += delta * new Vector2( 0.01f, -0.01f);
            if (CameraAngle.X > 2 * Math.PI) CameraAngle.X -= (float)(2 * Math.PI);
            if (CameraAngle.X < 0) CameraAngle.X += (float)(2 * Math.PI);
            if (CameraAngle.Y > Math.PI / 2 * 0.9f) CameraAngle.Y = (float)(Math.PI / 2 * 0.9f);
            if (CameraAngle.Y < -Math.PI / 2 * 0.9f) CameraAngle.Y = (float)(-Math.PI / 2 * 0.9f);
        }

        public void Zoom(int delta)
        {
            CameraDistance -= delta * 0.005f;
            if (CameraDistance > 30) CameraDistance = 30;
            if (CameraDistance < 0.2f) CameraDistance = 0.2f;

        }


        public Vector3 CameraDirection 
        {
            get
            {
                float x = (float)Math.Cos(CameraAngle.X) * (float)Math.Cos(CameraAngle.Y);
                float z = (float)Math.Sin(CameraAngle.X) * (float)Math.Cos(CameraAngle.Y);
                float y = (float)Math.Sin(CameraAngle.Y);
                var cameraDir = new Vector3(x, y, z);
                return cameraDir;
            }
        }

        public void MoveCamera(Vector2 delta)
        {
            var cameraDir = CameraDirection;
            Vector3 x = Vector3.Cross(cameraDir, Vector3.UnitY).Normalized();
            Vector3 y = Vector3.Cross(cameraDir, x).Normalized();

            CameraFocus += (x * delta.X + y * delta.Y)*0.01f;
        }
    }
}
