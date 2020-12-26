using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsMSN2020
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            HideSubMenu();
        }

        private void HideSubMenu()
        {
            panelSubMenu1.Visible = false;
            panelSubMenu2.Visible = false;
            panelSubMenu3.Visible = false;
            panelSubMenu4.Visible = false;
        }

        private void HidingSubMenu()
        {
            if (panelSubMenu1.Visible == true)
                panelSubMenu1.Visible = false;
            if (panelSubMenu2.Visible == true)
                panelSubMenu2.Visible = false;
            if (panelSubMenu3.Visible == true)
                panelSubMenu3.Visible = false;
            if (panelSubMenu4.Visible == true)
                panelSubMenu4.Visible = false;
        }

        private Form activeForm = null;
        private void OpenChildForm(Form ChildForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(ChildForm);
            panelChildForm.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }

        private void ShowSubMenu(Panel SubMenu)
        {
            if (SubMenu.Visible == false)
            {
                HidingSubMenu();
                SubMenu.Visible = true;
            }
            else
                SubMenu.Visible = false;

        }


        private void FormMain_Load(object sender, EventArgs e)
        {

        }
        #region Ввод Данных
        private void btnVvod_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubMenu1);
            //code
        }
       
        private void btnAZR_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Form1());
            //code

        }

        private void btnDrugoe_Click(object sender, EventArgs e)
        {
            //code

        }

        #endregion
        #region Результаты
        private void btnResults_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubMenu2);
        }

        private void btnRaschet_Click(object sender, EventArgs e)
        {



        }

        private void btnAZ_Click(object sender, EventArgs e)
        {

        }

        private void btnMacroAZ_Click(object sender, EventArgs e)
        {

        }

        private void btnMatrixAZ_Click(object sender, EventArgs e)
        {

        }

        private void btnOdnogroupAZ_Click(object sender, EventArgs e)
        {

        }

        private void PlotnostAZ_Click(object sender, EventArgs e)
        {

        }

        private void btnOtrazhatel_Click(object sender, EventArgs e)
        {

        }

        private void btnMacroOtr_Click(object sender, EventArgs e)
        {

        }

        private void btnMatrixOtr_Click(object sender, EventArgs e)
        {

        }

        private void btnOdnogroupOtr_Click(object sender, EventArgs e)
        {

        }

        private void btnPlotnostOtr_Click(object sender, EventArgs e)
        {

        }

        private void btnOstalnoe_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Экспорт

        private void btnExport_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubMenu3);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }

        private void btnDrugoiFormat_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Справка
        private void btnSpravka_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubMenu4);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }

        private void btnOprogramme_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
// design by Cateyrin v0.1