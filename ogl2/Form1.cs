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
    using static AttributeMappings;
    public partial class Form1 : Form
    {
        private Renderer _renderer; 
        public Form1()
        {
            _renderer = new Renderer();
            InitializeComponent();
            InitComboBoxes();


        }

        private void InitComboBoxes()
        {
            comboBox1.Items.AddRange(GetComboBoxStrings(Primitives));
            cullingComboBox.Items.AddRange(GetComboBoxStrings(CullFaceModes));
            polygonMode1.Items.AddRange(GetComboBoxStrings(PolygonModes));
            polygonMode2.Items.AddRange(GetComboBoxStrings(PolygonModes));
            alphaComboBox.Items.AddRange(GetComboBoxStrings(AlphaModes));
            blendSource.Items.AddRange(GetComboBoxStrings(BlendingFactorSrcs));
            blendDestination.Items.AddRange(GetComboBoxStrings(BlendingFactorDests));
        }
   
        private void glControl1_Load(object sender, EventArgs e)
        {
            _renderer.SetViewport(glControl1);
            cullingCheckBox.Checked = false;
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
            var primitiveType = AttributeMappings.GetAttribute(comboBox1, AttributeMappings.Primitives);         
            _renderer.SetPrimitiveType(primitiveType);
            _renderer.Render();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _renderer.ClearPoints();
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
            var mode = AttributeMappings.GetAttribute(cullingComboBox, AttributeMappings.CullFaceModes);
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
            var polygonModeFront = AttributeMappings.GetAttribute(polygonMode1, AttributeMappings.PolygonModes);
            var polygonModeBack = AttributeMappings.GetAttribute(polygonMode2, AttributeMappings.PolygonModes); ;
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
            var alphaFunction = AttributeMappings.GetAttribute(alphaComboBox,AttributeMappings.AlphaModes);          
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


        private void blendSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = AttributeMappings.GetAttribute(blendSource, AttributeMappings.BlendingFactorSrcs);         
            _renderer.SetBlendingSource((BlendingFactor)blendingFactor);
            _renderer.Render();   
        }

        private void blendDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = AttributeMappings.GetAttribute(blendSource, AttributeMappings.BlendingFactorDests);
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
