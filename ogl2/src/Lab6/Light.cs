using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogl2.src.Lab6
{
    internal struct Light
    {
        public bool Enabled;
        public Vector4 Position;
        public Vector4 Ambient;
        public Vector4 Diffuse;
        public Vector4 Specular;
        public Vector4 SpotDirection;
        public float SpotExponent;
        public float SpotCutoff;
        public float ConstantAttenuation;
        public float LinearAttenuation;
        public float QuadraticAttenuation;
    }
}
