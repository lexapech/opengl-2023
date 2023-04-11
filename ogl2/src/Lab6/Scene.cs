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
        public enum ProjectionEnum
        {
            Perspective, Orthographic
        }

        public ProjectionEnum Projection;
        public List<SceneObject> Objects = new List<SceneObject>();
        public bool ShowAxis = true;
        public Vector3 CameraFocus;
        public float CameraDistance;
        public Vector2 CameraAngle;
        private int _lastId = 1;
        private int _selectedId = 0;
        public bool WireframeMode = false;
        public Vector3 LightPosition = new Vector3(100, 100, 100);

        public SceneObject SelectedObject{
            get
            {
                var match = Objects.Where(Object => Object.Id == _selectedId).ToList();
                if (match.Count() > 0)
                {
                    return match[0];
                }
                return null;
            }
        }


        public int SelectedId { get { return _selectedId; }}

        public Scene()
        {
            CameraDistance = 2;
            CameraFocus = Vector3.Zero;
            Projection = ProjectionEnum.Perspective;
        }

        public SceneObject AddObject(string name, MeshGenerator generator, Vector3 pos)
        {
            var obj = new SceneObject(name,generator, _lastId++);
            obj.Position = pos;
            Objects.Add(obj);
            return obj;
        }

        public void SelectObject(int id)
        {
            var match = Objects.Where(Object => Object.Id == id).ToList();
            if(match.Count() > 0) _selectedId = match[0].Id;
            else _selectedId = 0;
        }

        public void UpdateSelected(Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            var obj = SelectedObject;
            if(obj != null)
            {
                obj.Position = pos;
                obj.EulerAngles = rotation;
                obj.AbsScale = scale;
            }         
        }


        public SceneObject GetObject(string name)
        {
            return Objects.Find(x => x.Name == name);
        }

        public SceneObject AddObject(string name, MeshGenerator generator)
        {
            return AddObject(name, generator, Vector3.Zero);
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
