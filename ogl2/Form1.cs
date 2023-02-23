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
            alphaComboBox.Items.AddRange(Renderer.AlphaModes.Keys.ToArray());
            blendSource.Items.AddRange(Renderer.BlendingFactorSrcs.Keys.ToArray());
            blendDestination.Items.AddRange(Renderer.BlendingFactorDests.Keys.ToArray());
            
        }
   
        private void glControl1_Load(object sender, EventArgs e)
        {
            _renderer.SetViewport(glControl1);           
            comboBox1.SelectedIndex = 0;
            cullingComboBox.SelectedIndex = 0;
            polygonMode1.SelectedIndex = 0;
            polygonMode2.SelectedIndex = 0;
            alphaComboBox.SelectedIndex = 0;
            blendSource.SelectedIndex = 0;
            blendDestination.SelectedIndex = 0;
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
            _renderer.MouseDown(pos);
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

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _renderer.MouseUp(pos);
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _renderer.MouseMove(pos);
        }

        private void scissorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.EnableScissor(scissorCheckBox.Checked);
            _renderer.Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _renderer.ClearScissorSelection();
            _renderer.SetMouseMode(Renderer.Mode.ScissorSelection);
        }

        private void scissorReset_Click(object sender, EventArgs e)
        {
            _renderer.ClearScissorSelection();
        }

        private void alphaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlphaFunction alphaFunction;
            if (alphaComboBox.SelectedItem == null) return;
            Renderer.AlphaModes.TryGetValue((string)alphaComboBox.SelectedItem, out alphaFunction);
            _renderer.SetAlphaFunction(alphaFunction);
            _renderer.Render();
        }

        private void alphaTrackBar_Scroll(object sender, EventArgs e)
        {
            var value = (float)alphaTrackBar.Value / alphaTrackBar.Maximum;
            _renderer.SetAlphaRef(value);
            _renderer.Render();
        }

        private void alphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.EnableAlpha(alphaCheckBox.Checked);
            _renderer.Render();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void blendSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlendingFactorSrc blendingFactor;
            if (blendSource.SelectedItem == null) return;
            Renderer.BlendingFactorSrcs.TryGetValue((string)blendSource.SelectedItem, out blendingFactor);
            _renderer.SetBlendingSource((BlendingFactor)blendingFactor);
            _renderer.Render();   
        }

        private void blendDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlendingFactorSrc blendingFactor;
            if (blendDestination.SelectedItem == null) return;
            Renderer.BlendingFactorSrcs.TryGetValue((string)blendDestination.SelectedItem, out blendingFactor);
            _renderer.SetBlendingDestination((BlendingFactor)blendingFactor);
            _renderer.Render();
        }

        private void blendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.enableBlending(blendCheckBox.Checked);
            _renderer.Render();
        }
    }
}
