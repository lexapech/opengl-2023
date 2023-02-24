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
            cullingCheckBox.Checked = false;
            comboBox1.SelectedIndex = 0;
            cullingComboBox.SelectedIndex = 0;
            polygonMode1.SelectedIndex = 0;
            polygonMode2.SelectedIndex = 0;
            alphaComboBox.SelectedIndex = 0;
            blendSource.SelectedIndex = 0;
            blendDestination.SelectedIndex = 0;
        }
   
        private void glControl1_Load(object sender, EventArgs e)
        {
            _renderer.SetViewport(glControl1);
            InitComboBoxes();
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
            var primitiveType = GetAttribute(comboBox1, Primitives);         
            _renderer.SelectedPrimitive = primitiveType;
            _renderer.Render();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _renderer.ClearPoints();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _renderer.PrimitiveSize = (float)numericUpDown1.Value;
            _renderer.Render();
        }

        private void cullingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.CullingEnabled = cullingCheckBox.Checked;
            _renderer.Render();
        }

        private void cullingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = GetAttribute(cullingComboBox, CullFaceModes);
            _renderer.CullFace = mode;
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
            var polygonModeFront = GetAttribute(polygonMode1, PolygonModes);
            var polygonModeBack = GetAttribute(polygonMode2, PolygonModes);
            _renderer.PolygonModeFront = polygonModeFront;
            _renderer.PolygonModeFront = polygonModeBack;
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
            _renderer.ScissorEnabled = scissorCheckBox.Checked;
            _renderer.Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _renderer.ClearScissorSelection();
            _renderer.MouseMode = Renderer.Mode.ScissorSelection;
        }

        private void scissorReset_Click(object sender, EventArgs e)
        {
            _renderer.ClearScissorSelection();
        }

        private void alphaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var alphaFunction = GetAttribute(alphaComboBox,AlphaModes);          
            _renderer.AlphaFunction = alphaFunction;
            _renderer.Render();
        }

        private void alphaTrackBar_Scroll(object sender, EventArgs e)
        {
            var value = (float)alphaTrackBar.Value / alphaTrackBar.Maximum;
            _renderer.AlphaRef = value;
            _renderer.Render();
        }

        private void alphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.AlphaTestEnabled = alphaCheckBox.Checked;
            _renderer.Render();
        }


        private void blendSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = GetAttribute(blendSource, BlendingFactorSrcs);         
            _renderer.BlendingFactorSrc = (BlendingFactor)blendingFactor;
            _renderer.Render();   
        }

        private void blendDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = GetAttribute(blendSource, BlendingFactorDests);
            _renderer.BlendingFactorDest = (BlendingFactor)blendingFactor;
            _renderer.Render();
        }

        private void blendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.BlendingEnabled = blendCheckBox.Checked;
            _renderer.Render();
        }
    }
}
