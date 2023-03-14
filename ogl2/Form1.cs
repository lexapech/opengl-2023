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
        private readonly Presenter _presenter; 
        public Form1()
        {
            var renderer = new Renderer();
            _presenter = new Presenter(renderer);
            _presenter.CursorChangeHandler += SetCursor;
            
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
            _presenter.SetViewport(glControl1);
            InitComboBoxes();
            glControl1_Resize(sender, EventArgs.Empty);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            _presenter.Paint();
        }    

        private void glControl1_Resize(object sender, EventArgs e)
        {
            _presenter.SetViewport(glControl1);
            _presenter.Resize();             
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _presenter.MouseDown(pos);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var primitiveType = GetAttribute(comboBox1, Primitives);
            _presenter.SetPrimitiveType(primitiveType);

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _presenter.ClearPoints();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _presenter.SetPrimitiveSize((float)numericUpDown1.Value);
        }

        private void cullingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.EnableCulling(cullingCheckBox.Checked);
        }

        private void cullingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = GetAttribute(cullingComboBox, CullFaceModes);
            _presenter.SetCullFace(mode);
        }

        private void polygonMode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var polygonModeFront = GetAttribute(polygonMode1, PolygonModes);
            _presenter.SetPolygonModeFront(polygonModeFront);
        }

        private void polygonMode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var polygonModeBack = GetAttribute(polygonMode2, PolygonModes);
            _presenter.SetPolygonModeBack(polygonModeBack);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _presenter.MouseUp(pos);
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _presenter.MouseMove(pos);
        }

        private void scissorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.EnableScissor(scissorCheckBox.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _presenter.BeginScissorSelection();
        }

        private void scissorReset_Click(object sender, EventArgs e)
        {
            _presenter.ClearScissorSelection();
        }

        private void alphaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var alphaFunction = GetAttribute(alphaComboBox,AlphaModes);
            _presenter.SetAlphaFunction(alphaFunction);
        }

        private void alphaTrackBar_Scroll(object sender, EventArgs e)
        {
            var value = (float)alphaTrackBar.Value / alphaTrackBar.Maximum;
            _presenter.SetAlphaRef(value);
        }

        private void alphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.EnableAlphaTest(alphaCheckBox.Checked);
        }


        private void blendSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = GetAttribute(blendSource, BlendingFactorSrcs);
            _presenter.SetBlendingFactorSrc((BlendingFactor)blendingFactor); 
        }

        private void blendDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = GetAttribute(blendSource, BlendingFactorDests);
            _presenter.SetBlendingFactorDest((BlendingFactor)blendingFactor);
        }

        private void blendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.EnableBlending(blendCheckBox.Checked);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.TabChanged(tabControl1.SelectedIndex);
        }

        private void fractalSteps_ValueChanged(object sender, EventArgs e)
        {
            _presenter.SetFractalSteps((int)fractalSteps.Value);
        }

        private void ChangeSeedButton_Click(object sender, EventArgs e)
        {
            _presenter.ChangeFractalSeed();
        }

        private void curveScale_Scroll(object sender, EventArgs e)
        {
            var value = (float)curveScale.Value / curveScale.Maximum;
            _presenter.SetSplineScale(value);
        }

        private void curveSteps_ValueChanged(object sender, EventArgs e)
        {
            _presenter.SetSplineSteps((int)curveSteps.Value);
        }

        private void SetCursor(bool hand)
        {
            glControl1.Cursor = hand ? Cursors.Hand : Cursors.Default;
        }

        private void cardinalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.EnableCardinalSpline(cardinalCheckBox.Checked);
        }

        private void BezierCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.EnableBezierSpline(BezierCheckBox.Checked);
        }
    }
}
