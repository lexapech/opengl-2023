using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ogl2
{
    public partial class Form1 : Form
    {
        private List<Vector2> _vertices = new List<Vector2>();

        private static Dictionary<string, PrimitiveType> _primitives = new Dictionary<string, PrimitiveType>
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
        private static Color[] _colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple };

        public Form1()
        {
            
            InitializeComponent();
            comboBox1.Items.AddRange(_primitives.Keys.ToArray());
            comboBox1.SelectedIndex = 0;
        }

        


        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            glControl1_Resize(sender, EventArgs.Empty);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void Render()
        {
            PrimitiveType primitiveType;
            if (comboBox1.SelectedItem == null) return;
            _primitives.TryGetValue((string)comboBox1.SelectedItem, out primitiveType);
            glControl1.MakeCurrent();
            GL.ClearColor(Color4.MidnightBlue);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(primitiveType);

            GL.Color4(Color4.Silver);
            for(int i=0;i<_vertices.Count;i++)
            {
                var color = _colors[i % _colors.Length];
                GL.Color4(color.R, color.G, color.B, 1.0f);
                GL.Vertex2(_vertices[i].X, _vertices[i].Y);
            }
           
            GL.End();

            glControl1.SwapBuffers();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();
            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);
            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            /* float aspect_ratio = Math.Max(glControl1.ClientSize.Width, 1) / (float)Math.Max(glControl1.ClientSize.Height, 1);
             Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
             GL.MatrixMode(MatrixMode.Projection);
             GL.LoadMatrix(ref perpective);*/
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            pos = new Vector2(pos.X / glControl1.ClientSize.Width, pos.Y / glControl1.ClientSize.Height);
            pos = pos * 2 - Vector2.One;
            _vertices.Add(pos);
            Render();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _vertices.Clear();
            Render();
        }

    }
}
