using ogl2.src.Lab6;
using OpenTK;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal class SceneObject
    {
        public string Name { get; private set; }
        public Mesh Mesh { get; private set; }
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 AbsScale;

        public SceneObject(string name,MeshGenerator generator)
        {
            Name = name;
            Rotation = Quaternion.Identity;
            Position = new Vector3();
            AbsScale = Vector3.One;
            generator.Generate();
            Mesh = generator.GetMesh();
        }

        public SceneObject Translate(Vector3 delta)
        {
            Position += delta;
            return this;
        }
        public SceneObject Rotate(Vector3 axis,float angle)
        {
            Rotation = Quaternion.FromAxisAngle(axis, (float)(angle / 180f * Math.PI)) * Rotation;
            return this;
        }
        public SceneObject Scale(Vector3 scale)
        {
            AbsScale *= scale;
            return this;
        }


    }
}
