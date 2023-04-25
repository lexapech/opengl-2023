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
        private bool _selectedEvent = true;
        public Form1()
        {
            _presenter = new Presenter();         
            _presenter.Lab4.CursorChangeHandler += SetCursor;
            _presenter.Lab6.SelectionChanged += UpdateSelectedInfo;


            InitializeComponent();         
        }

        private void UpdateSelectedInfo(SceneObject sceneObject)
        {
            
            panel13.Visible = sceneObject != null;
            if (sceneObject == null) return;
            _selectedEvent = false;
            Lab6_SelectedName.Text = "Выбранный объект: "+ sceneObject.Name;
            Lab6_Selected_X.Value = (decimal)sceneObject.Position.X;
            Lab6_Selected_Y.Value = (decimal)sceneObject.Position.Y;
            Lab6_Selected_Z.Value = (decimal)sceneObject.Position.Z;

            Lab6_Selected_Pitch.Value = (decimal)Utility.Clamp((float)(sceneObject.EulerAngles.X * 180f / Math.PI),-180,180);
            Lab6_Selected_Yaw.Value = (decimal)Utility.Clamp((float)(sceneObject.EulerAngles.Y * 180f / Math.PI), -180, 180);
            Lab6_Selected_Roll.Value = (decimal)Utility.Clamp((float)(sceneObject.EulerAngles.Z * 180f / Math.PI), -180, 180);

            Lab6_Selected_SX.Value = (decimal)sceneObject.AbsScale.X;
            Lab6_Selected_SY.Value = (decimal)sceneObject.AbsScale.Y;
            Lab6_Selected_SZ.Value = (decimal)sceneObject.AbsScale.Z;

            Lab6_Shininess.Value = (decimal)sceneObject.Material.Shininess;

            panel19.BackColor = Utility.ConvertColor(sceneObject.Material.Specular);
            panel20.BackColor = Utility.ConvertColor(sceneObject.Material.Emission);
            if (sceneObject.Steps < 3)
            {
                label20.Visible = false;
                Lab6_Division.Visible = false;
            }
            else
            {
                label20.Visible = true;
                Lab6_Division.Visible = true;
                Lab6_Division.Value = sceneObject.Steps;
            }              
            _selectedEvent = true;
        }

        private void ChangeSelectedProps()
        {
            if (!_selectedEvent) return;
            Vector3 pos;
            Vector3 angles;
            Vector3 scale;
            pos.X = (float)Lab6_Selected_X.Value;
            pos.Y = (float)Lab6_Selected_Y.Value;
            pos.Z = (float)Lab6_Selected_Z.Value;

            angles.X = (float)((float)Lab6_Selected_Pitch.Value  / 180f * Math.PI);
            angles.Y = (float)((float)Lab6_Selected_Yaw.Value /  180f * Math.PI);
            angles.Z = (float)((float)Lab6_Selected_Roll.Value / 180f * Math.PI);

            scale.X = (float)Lab6_Selected_SX.Value;
            scale.Y = (float)Lab6_Selected_SY.Value;
            scale.Z = (float)Lab6_Selected_SZ.Value;

            Color specular = panel19.BackColor;
            Color emission = panel20.BackColor;

            float shininess = (float)Lab6_Shininess.Value;

            _presenter.Lab6.UpdateSelected(pos,angles, scale,(int)Lab6_Division.Value, specular,emission,shininess);
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
            _presenter.MouseDown(pos, (e.Button & MouseButtons.Left) != 0, (e.Button & MouseButtons.Right) != 0, (ModifierKeys & Keys.Shift) != 0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var primitiveType = GetAttribute(comboBox1, Primitives);
            _presenter.Lab12.SetPrimitiveType(primitiveType);

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _presenter.Lab12.ClearPoints();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _presenter.Lab12.SetPrimitiveSize((float)numericUpDown1.Value);
        }

        private void cullingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab12.EnableCulling(cullingCheckBox.Checked);
        }

        private void cullingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = GetAttribute(cullingComboBox, CullFaceModes);
            _presenter.Lab12.SetCullFace(mode);
        }

        private void polygonMode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var polygonModeFront = GetAttribute(polygonMode1, PolygonModes);
            _presenter.Lab12.SetPolygonModeFront(polygonModeFront);
        }

        private void polygonMode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var polygonModeBack = GetAttribute(polygonMode2, PolygonModes);
            _presenter.Lab12.SetPolygonModeBack(polygonModeBack);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _presenter.MouseUp(pos);
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = new Vector2(e.Location.X, glControl1.ClientSize.Height - e.Location.Y);
            _presenter.MouseMove(pos,(e.Button & MouseButtons.Left) != 0, (e.Button & MouseButtons.Right) != 0, (ModifierKeys & Keys.Shift) != 0);
        }

        private void scissorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab12.EnableScissor(scissorCheckBox.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _presenter.Lab12.BeginScissorSelection();
        }

        private void scissorReset_Click(object sender, EventArgs e)
        {
            _presenter.Lab12.ClearScissorSelection();
        }

        private void alphaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var alphaFunction = GetAttribute(alphaComboBox,AlphaModes);
            _presenter.Lab12.SetAlphaFunction(alphaFunction);
        }

        private void alphaTrackBar_Scroll(object sender, EventArgs e)
        {
            var value = (float)alphaTrackBar.Value / alphaTrackBar.Maximum;
            _presenter.Lab12.SetAlphaRef(value);
        }

        private void alphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab12.EnableAlphaTest(alphaCheckBox.Checked);
        }


        private void blendSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = GetAttribute(blendSource, BlendingFactorSrcs);
            _presenter.Lab12.SetBlendingFactorSrc((BlendingFactor)blendingFactor); 
        }

        private void blendDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            var blendingFactor = GetAttribute(blendSource, BlendingFactorDests);
            _presenter.Lab12.SetBlendingFactorDest((BlendingFactor)blendingFactor);
        }

        private void blendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab12.EnableBlending(blendCheckBox.Checked);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.TabChanged(tabControl1.SelectedIndex);
        }

        private void fractalSteps_ValueChanged(object sender, EventArgs e)
        {
            _presenter.Lab3.SetFractalSteps((int)fractalSteps.Value);
        }

        private void ChangeSeedButton_Click(object sender, EventArgs e)
        {
            _presenter.Lab3.ChangeFractalSeed();
        }

        private void curveScale_Scroll(object sender, EventArgs e)
        {
            var value = (float)curveScale.Value / curveScale.Maximum;
            _presenter.Lab4.SetSplineScale(value);
        }

        private void curveSteps_ValueChanged(object sender, EventArgs e)
        {
            _presenter.Lab4.SetSplineSteps((int)curveSteps.Value);
        }

        private void SetCursor(bool hand)
        {
            glControl1.Cursor = hand ? Cursors.Hand : Cursors.Default;
        }

        private void cardinalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab4.EnableCardinalSpline(cardinalCheckBox.Checked);
        }

        private void BezierCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab4.EnableBezierSpline(BezierCheckBox.Checked);
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var pos = new Vector3(trackBar1.Value / (float)trackBar1.Maximum,
                trackBar2.Value / (float)trackBar2.Maximum,
                trackBar3.Value / (float)trackBar3.Maximum);
            _presenter.Lab5.SetSurfaceLightPosition(pos);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            var pos = new Vector3(trackBar1.Value / (float)trackBar1.Maximum,
                trackBar2.Value / (float)trackBar2.Maximum,
                trackBar3.Value / (float)trackBar3.Maximum);
            _presenter.Lab5.SetSurfaceLightPosition(pos);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            var pos = new Vector3(trackBar1.Value / (float)trackBar1.Maximum,
                trackBar2.Value / (float)trackBar2.Maximum,
                trackBar3.Value / (float)trackBar3.Maximum);
            _presenter.Lab5.SetSurfaceLightPosition(pos);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var pos = new Vector3(trackBar1.Value / (float)trackBar1.Maximum,
                trackBar2.Value / (float)trackBar2.Maximum,
                trackBar3.Value / (float)trackBar3.Maximum);
            _presenter.Lab5.SetSurfaceLightPosition(pos);

            glControl1.MouseWheel += new MouseEventHandler(glControl1_MouseWheel);
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            var delta = e.Delta;
            _presenter.Zoom(delta);
        }

        private void glControl1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void Lab6_wireframe_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab6.SetWireframe(Lab6_wireframe.Checked);
        }

        private void Lab6_Ortho_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab6.SetOrthographic(Lab6_Ortho.Checked);
        }

        private void Lab6_Selected_X_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_Y_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_Z_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_Pitch_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_Yaw_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_Roll_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_SX_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_SY_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Selected_SZ_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_Transparent_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab6.SetTransparent(Lab6_Transparent.Checked);
        }

        private void Lab6_Division_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void Lab6_CoordAxis_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab6.SetCoordAxis(Lab6_CoordAxis.Checked);
        }

        private void Lab6_Shader_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab6.EnableShader(Lab6_Shader.Checked);
        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel14_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel14.BackColor = colorDialog1.Color;
            _presenter.Lab6.SetLightColors(panel14.BackColor, panel17.BackColor, panel18.BackColor);
        }

        private void panel17_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel17.BackColor = colorDialog1.Color;
            _presenter.Lab6.SetLightColors(panel14.BackColor, panel17.BackColor, panel18.BackColor);
        }

        private void panel18_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel18.BackColor = colorDialog1.Color;
            _presenter.Lab6.SetLightColors(panel14.BackColor, panel17.BackColor, panel18.BackColor);
        }

        private void SetLightPos()
        {
            Vector4 pos;
            pos.X = (float)numericUpDown11.Value;
            pos.Y = (float)numericUpDown10.Value;
            pos.Z = (float)numericUpDown9.Value;
            pos.W = (float)numericUpDown2.Value;
            _presenter.Lab6.SetLightPosition(pos);
        }
        private void SetLightDir()
        {
            Vector3 pos;
            pos.X = (float)numericUpDown8.Value;
            pos.Y = (float)numericUpDown7.Value;
            pos.Z = (float)numericUpDown6.Value;
            _presenter.Lab6.SetLightDirection(pos);
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            SetLightPos();
            SetLightDir();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            SetLightPos();
            SetLightDir();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            SetLightPos();
            SetLightDir();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            SetLightDir();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            SetLightDir();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            SetLightDir();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            //W
            SetLightPos();
        }

        private void panel19_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel19.BackColor = colorDialog1.Color;
            ChangeSelectedProps();
        }

        private void panel20_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel20.BackColor = colorDialog1.Color;
            ChangeSelectedProps();
        }

        private void Lab6_Shininess_ValueChanged(object sender, EventArgs e)
        {
            ChangeSelectedProps();
        }

        private void SetLightProps()
        {
            float exp = (float)numericUpDown3.Value;
            float cutoff = (float)numericUpDown4.Value;
            float conAtt = (float)numericUpDown5.Value;
            float linAtt = (float)numericUpDown12.Value;
            float quadAtt = (float)numericUpDown13.Value;
            _presenter.Lab6.SetLightProps(exp, cutoff,conAtt,linAtt,quadAtt);
        }

        private void Lab7_LightOn_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.Lab6.EnableLight(Lab7_LightOn.Checked);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            SetLightProps();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            SetLightProps();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            SetLightProps();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            SetLightProps();
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            SetLightProps();
        }
    }
}
