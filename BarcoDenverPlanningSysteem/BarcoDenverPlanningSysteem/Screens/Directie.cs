using BarcoDenverPlanningSysteem.Classes.Error;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcoDenverPlanningSysteem
{
    public partial class Directie : Form
    {
        private Point _mouseDownLocation;
        private readonly int buttonHeight = 29;

        private LogicalRepository repository;

        private ErrorHandler error = new ErrorHandler();

        public Directie(LogicalRepository logical)
        {
            InitializeComponent();

            repository = logical;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            _mouseDownLocation = new Point(0, 0);
        }

        private void Directie_Load(object sender, EventArgs e)
        {
            string[] templist = repository.GetAllWorkplaces();
            cbxAllWorkPlacesEditCode.Items.AddRange(templist);
            cbxAllWorkPlacesAddStaffMember.Items.AddRange(templist);
        }

        private void tbxNewCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnSaveCode_Click(object sender, EventArgs e)
        {
            if (cbxAllWorkPlacesEditCode.Text != "" && tbxNewCode.Text != "")
            {
                repository.EditCode(tbxNewCode.Text, cbxAllWorkPlacesEditCode.Text);
                tbxCurrentCode.Text = repository.GetCodeFromFunction(cbxAllWorkPlacesEditCode.SelectedItem.ToString());
                tbxNewCode.Text = "";
            }
            else
            {
                MessageBox.Show(error.NotEveryThingFilledInErrorMessage());
            }
        }

        private void cbxAllWorkPlaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxCurrentCode.Text = repository.GetCodeFromFunction(cbxAllWorkPlacesEditCode.SelectedItem.ToString());
        }

        private void pnInlogCodeAanpassen_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Panel pictureBox1 = (Panel)sender;
                pictureBox1.Left = e.X + pictureBox1.Left - _mouseDownLocation.X;
                pictureBox1.Top = e.Y + pictureBox1.Top - _mouseDownLocation.Y;
            }
        }

        private void pnInlogCodeAanpassen_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownLocation = e.Location;
        }

        private void Directie_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnAddStaffMember_Click(object sender, EventArgs e)
        {
            if (cbxAllWorkPlacesAddStaffMember.Text != "" || tbxCostPerHour.Text != "" || tbxStaffMemberName.Text != "")
            {
                MessageBox.Show("Werknemer is toegevoegd aan de database");
            }
            else
            {
                MessageBox.Show(error.NotEveryThingFilledInErrorMessage());
            }
        }

        private void tbxCostPerHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
