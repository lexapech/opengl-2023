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
using OpenTK.Platform.Windows;

namespace ogl2
{
    public partial class Form1 : Form
    {
        private Renderer _renderer; 
        public Form1()
        {
            _renderer = new Renderer();
            InitializeComponent();
            comboBox1.Items.AddRange(Renderer.Primitives.Keys.ToArray());
            comboBox1.SelectedIndex = 0;
        }
   
        private void glControl1_Load(object sender, EventArgs e)
        {
            _renderer.SetViewport(glControl1);
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            glControl1_Resize(sender, EventArgs.Empty);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            _renderer.Render();
        }    
        private void glControl1_Resize(object sender, EventArgs e)
        {
            _renderer.SetViewport(glControl1);
            _renderer.Resize();             
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            pos = new Vector2(pos.X / glControl1.ClientSize.Width, pos.Y / glControl1.ClientSize.Height);
            pos = pos * 2 - Vector2.One;
            _renderer.AddPoint(pos);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrimitiveType primitiveType;
            if (comboBox1.SelectedItem == null) return;
            Renderer.Primitives.TryGetValue((string)comboBox1.SelectedItem, out primitiveType);
            _renderer.SetPrimitiveType(primitiveType);
            _renderer.Render();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _renderer.ClearPoints();
        }

    }
}
