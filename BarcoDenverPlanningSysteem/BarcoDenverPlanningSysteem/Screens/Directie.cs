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

        //happens when the form is loaded
        private void Directie_Load(object sender, EventArgs e)
        {
            //we fill this with everything because we can edit the code of administrators
            cbxAllWorkPlacesEditCode.Items.AddRange(repository.GetAllWorkplaces());
            //we remove administrator from this list because administrator has acces to everything
            cbxWorkplaceSelectionWorkplaceEditor.Items.AddRange(repository.GetAllWorkplaces(new int[1] { 0 }));

            cbxAddStaffmemberWorkplace.Items.AddRange(repository.GetPlannableFunctionsAvailableToUser());
            RefreshScreen();
        }

        private void RefreshScreen()
        {
            cbxChooseStaffMemberEditStaffMember.Items.Clear();

            cbxChooseStaffMemberEditStaffMember.Items.AddRange(repository.GetListOfStaffMembers());

            repository.FillViewWithAllStaffMembers(dgvAllStaffMembers);
        }

        #region TabPage Editdatabase
        //keycheck for textboxes that only allow numbers
        private void tbxNewCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        //when the button for saving staffmembers is pushed we need to change the old code to the new code and update the screen
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

        //when the index of the cbxAllWorkPlaces is changed we update the current code textbox
        private void cbxAllWorkPlaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxCurrentCode.Text = repository.GetCodeFromFunction(cbxAllWorkPlacesEditCode.SelectedItem.ToString());
        }

        //when dragging the form we update the screen so the panel follows the mouse
        private void pnInlogCodeAanpassen_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Panel pictureBox1 = (Panel)sender;
                pictureBox1.Left = e.X + pictureBox1.Left - _mouseDownLocation.X;
                pictureBox1.Top = e.Y + pictureBox1.Top - _mouseDownLocation.Y;
            }
        }

        //we get the position of the mouse when we start dragging the panel
        private void pnInlogCodeAanpassen_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownLocation = e.Location;
        }

        //close application
        private void Directie_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //when the button for adding staffmembers is pushed we fire events for adding him to the database after that we update the screen
        private void BtnAddStaffMember_Click(object sender, EventArgs e)
        {
            if (tbxCostPerHour.Text != "" || tbxStaffMemberName.Text != "" || cbxAddStaffmemberWorkplace.Text == "")
            {
                repository.AddStaffMember(tbxStaffMemberName.Text, double.Parse(tbxCostPerHour.Text), cbxAddStaffmemberWorkplace.Text);
                RefreshScreen();

                //Clear everything that was filled in.
                cbxAddStaffmemberWorkplace.Text = "";
                tbxStaffMemberName.Text = "";
                tbxCostPerHour.Text = "";
            }
            else
            {
                MessageBox.Show(error.NotEveryThingFilledInErrorMessage());
            }
        }

        //check that we only fill in numbers here
        private void TbxCostPerHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        //delete the selected staffmembers from the database and then update the screen
        private void BtnDeleteStaffMember_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Weet je zeker dat je de geselecteerde rij(en) wilt verwijderen", "Verwijder personeelslid", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (dgvAllStaffMembers.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvAllStaffMembers.SelectedRows)
                    {
                        repository.DeleteStaffMemberByid(int.Parse(row.Cells["id"].Value.ToString()));
                    }

                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show(error.NoRowsSelectedMessage());
                }
            }
        }

        private void btnSaveWorkplaceEdit_Click(object sender, EventArgs e)
        {
            //TODO: save Edits from workplaces

        }

        private void btnSaveStaffMemberEdit_Click(object sender, EventArgs e)
        {
            //TODO: save edits from staffmembers
        }

        private void cbxWorkplaceSelectionWorkplaceEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: fill the right comboboxes with information from database

        }

        private void cbxChooseStaffMemberEditStaffMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            int staffmemberid = 0;

            foreach (DataGridViewRow row in dgvAllStaffMembers.Rows)
            {
                if (row.Cells["name"].Value.ToString() == cbxChooseStaffMemberEditStaffMember.Text)
                {
                    staffmemberid = int.Parse(row.Cells["id"].Value.ToString());
                }
            }

            repository.GetCheckedItemsFromStaffmemberName(staffmemberid, clbEditStaffmemberCheckListBox);
        }

#endregion

        #region TabPage Overview
        private void CalendarForOverView_DateChanged(object sender, DateRangeEventArgs e)
        {
            //TODO: fill the tables with the needed information LOW
        }

        private void cbxWorkplaceSelectionForOverview_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Make table information change whenever you change the workplace you want to see a overview of LOW
        }

        private void btnSelectRange_Click(object sender, EventArgs e)
        {
            //TODO: Add functionalitie for selecting a range of dates to view the overview with LOW
        }

        #endregion

        #region PlanningTab


        private void cbxWorkplaceSelectionForPlanning_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Fill the planning tables with the corosponding data
        }

        private void CalendarForPlanning_DateChanged(object sender, DateRangeEventArgs e)
        {
            //TODO: fill the planing tables with the corresponding data
        }

#endregion

        #region kolibri tab

        private void btnCreateKolibriFile_Click(object sender, EventArgs e)
        {
            //TODO: create a kolibri file
        }

        private void CalendarKoliBriFile_DateChanged(object sender, DateRangeEventArgs e)
        {
            //TODO: change the date for wich a colibri file has to be made
        }

#endregion
    }
}
