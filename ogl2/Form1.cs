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
            cullingCheckBox.Checked = false;
            cullingComboBox.Items.AddRange(Renderer.CullFaceModes.Keys.ToArray());          
            polygonMode1.Items.AddRange(Renderer.PolygonModes.Keys.ToArray());       
            polygonMode2.Items.AddRange(Renderer.PolygonModes.Keys.ToArray());
            
        }
   
        private void glControl1_Load(object sender, EventArgs e)
        {
            _renderer.SetViewport(glControl1);           
            comboBox1.SelectedIndex = 0;
            cullingComboBox.SelectedIndex = 0;
            polygonMode1.SelectedIndex = 0;
            polygonMode2.SelectedIndex = 0;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _renderer.SetPrimitiveSize((float)numericUpDown1.Value);
            _renderer.Render();
        }

        private void cullingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.EnableCulling(cullingCheckBox.Checked);
            _renderer.Render();
        }

        private void cullingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CullFaceMode mode;
            if (cullingComboBox.SelectedItem == null) return;
            Renderer.CullFaceModes.TryGetValue((string)cullingComboBox.SelectedItem, out mode);
            _renderer.SetCullingFace(mode);
            _renderer.Render();
        }

        private void polygonMode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePolygonMode();
        }

        private void polygonMode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePolygonMode();
        }
        private void UpdatePolygonMode()
        {
            PolygonMode polygonModeFront;
            PolygonMode polygonModeBack;
            if (polygonMode2.SelectedItem == null) return;
            Renderer.PolygonModes.TryGetValue((string)polygonMode2.SelectedItem, out polygonModeBack);
            if (polygonMode1.SelectedItem == null) return;
            Renderer.PolygonModes.TryGetValue((string)polygonMode1.SelectedItem, out polygonModeFront);
            _renderer.SetPolygonMode(polygonModeFront, polygonModeBack);
            _renderer.Render();
        }
    }
}
