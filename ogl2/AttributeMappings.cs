using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using static ogl2.Renderer;

namespace ogl2
{
    internal class AttributeMappings
    {
        public static Dictionary<string, PrimitiveType> Primitives = new Dictionary<string, PrimitiveType>
        {
            {"GL_POINTS",PrimitiveType.Points },
            {"GL_LINES",PrimitiveType.Lines },
            {"GL_LINE_STRIP",PrimitiveType.LineStrip },
            {"GL_LINE_LOOP",PrimitiveType.LineLoop },
            {"GL_TRIANGLES",PrimitiveType.Triangles },
            {"GL_TRIANGLE_STRIP",PrimitiveType.TriangleStrip },
            {"GL_TRIANGLE_FAN",PrimitiveType.TriangleFan },
            {"GL_QUADS",PrimitiveType.Quads },
            {"GL_QUAD_STRIP",PrimitiveType.QuadStrip },
            {"GL_POLYGON",PrimitiveType.Polygon }
        };
        public static Dictionary<string, CullFaceMode> CullFaceModes = new Dictionary<string, CullFaceMode>
        {
            {"GL_FRONT",CullFaceMode.Front },
            {"GL_BACK",CullFaceMode.Back },
            {"GL_FRONT_AND_BACK",CullFaceMode.FrontAndBack }
        };
        public static Dictionary<string, PolygonMode> PolygonModes = new Dictionary<string, PolygonMode>
        {
            {"GL_FILL",PolygonMode.Fill },
            {"GL_POINT",PolygonMode.Point },
            {"GL_LINE",PolygonMode.Line }
        };

        public static Dictionary<string, AlphaFunction> AlphaModes = new Dictionary<string, AlphaFunction>
        {
            {"GL_ALWAYS",AlphaFunction.Always },
            {"GL_EQUAL",AlphaFunction.Equal},
            {"GL_GEQUAL",AlphaFunction.Gequal},
            {"GL_GREATER",AlphaFunction.Greater},
            {"GL_LEQUAL",AlphaFunction.Lequal},
            {"GL_LESS",AlphaFunction.Less},
            {"GL_NEVER",AlphaFunction.Never},
            {"GL_NOTEQUAL",AlphaFunction.Notequal}
        };

        public static Dictionary<string, BlendingFactorSrc> BlendingFactorSrcs = new Dictionary<string, BlendingFactorSrc>
        {
            {"GL_ZERO", BlendingFactorSrc.Zero},
            {"GL_ONE", BlendingFactorSrc.One},
            {"GL_DST_COLOR", BlendingFactorSrc.DstColor},
            {"GL_ONE_MINUS_DST_COLOR", BlendingFactorSrc.OneMinusDstColor},
            {"GL_SRC_ALPHA", BlendingFactorSrc.SrcAlpha},
            {"GL_ONE_MINUS_SRC_ALPHA", BlendingFactorSrc.OneMinusSrcAlpha},
            {"GL_DST_ALPHA", BlendingFactorSrc.DstAlpha},
            {"GL_ONE_MINUS_DST_ALPHA", BlendingFactorSrc.OneMinusDstAlpha},
            {"GL_SRC_ALPHA_SATURATE", BlendingFactorSrc.SrcAlphaSaturate}
        };
        public static Dictionary<string, BlendingFactorDest> BlendingFactorDests = new Dictionary<string, BlendingFactorDest>
        {
            {"GL_ZERO", BlendingFactorDest.Zero},
            {"GL_ONE", BlendingFactorDest.One},
            {"GL_SRC_COLOR",BlendingFactorDest.SrcColor },
            {"GL_ONE_MINUS_SRC_COLOR",BlendingFactorDest.OneMinusSrcColor },
            {"GL_SRC_ALPHA", BlendingFactorDest.SrcAlpha},
            {"GL_ONE_MINUS_SRC_ALPHA", BlendingFactorDest.OneMinusSrcAlpha},
            {"GL_DST_ALPHA", BlendingFactorDest.DstAlpha},
            {"GL_ONE_MINUS_DST_ALPHA", BlendingFactorDest.OneMinusDstAlpha}
        };

        public static T GetAttribute<T>(ComboBox combo,Dictionary<string, T> mapping)
        {           
            T attribute;
            if (combo.SelectedItem == null) return default;
            mapping.TryGetValue((string)combo.SelectedItem, out attribute);
            return attribute; 
        }
        public static string[] GetComboBoxStrings<T>(Dictionary<string, T> mapping)
        {
            return mapping.Keys.ToArray();
        }
    }
}
