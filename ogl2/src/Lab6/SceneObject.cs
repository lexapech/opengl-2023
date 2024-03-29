﻿using ogl2.src.Lab6;
using ogl2;
using OpenTK;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ogl2
{
    internal class SceneObject
    {
        public string Name { get; private set; }
        public Mesh Mesh { get; private set; }
        public Material Material;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 AbsScale;
        public readonly int Id;
        private Vector3 _eulerAngles;
        private MeshGenerator _generator;
        public int Steps 
        { 
            get 
            {
                return _generator.GetSteps();
            } 
            set
            {
                var old = _generator.GetSteps();
                if (old != value)
                {
                    _generator.SetSteps(value);
                    Mesh = _generator.GetMesh();
                }
            }
        }

        public Vector3 EulerAngles {
            get
            {
                return _eulerAngles;
            }
            set
            {
                Rotation = Quaternion.FromEulerAngles(value);
                _eulerAngles = value;
            }
        }


        public SceneObject(string name,MeshGenerator generator, int id)
        {
            Name = name;
            Rotation = Quaternion.Identity;
            Position = new Vector3();
            AbsScale = Vector3.One;
            _generator = generator;    
            Mesh = generator.GetMesh();
            Id = id;
            Material = new Material
            {
                Specular = new Vector4(1, 1, 1, 1),
                Emission = new Vector4(0,0, 0, 1),
                Shininess = 100
            };
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
