using BarcoDenverPlanningSysteem.Classes.Error;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcoDenverPlanningSysteem
{
    public partial class Keuken : Form
    {
        LogicalRepository repository;
        private ErrorHandler error;

        private DateTime lastDate;

        private bool busy;

        public Keuken(LogicalRepository logical)
        {
            InitializeComponent();
            repository = logical;
            Setup();
        }

        private void Setup()
        {
            //setting variables
            lastDate = dtpDateTimePicker.Value;
            busy = false;
            error = new ErrorHandler();

            //hiding correct buttons
            HideStaffCost(true);
        }

        private void HideStaffCost(bool hide)
        {
            if (hide)
            {
                txtStaffCostMonday.Hide();
                txtStaffCostTuesday.Hide();
                txtStaffCostWednesday.Hide();
                txtStaffCostThursday.Hide();
                txtStaffCostFriday.Hide();
                txtStaffCostSaturday.Hide();
                txtStaffCostSunday.Hide();

                lblStaffCostMonday.Hide();
                lblStaffCostTuesday.Hide();
                lblStaffCostWednesday.Hide();
                lblStaffCostThursday.Hide();
                lblStaffCostFriday.Hide();
                lblStaffCostSaturday.Hide();
                lblStaffCostSunday.Hide();

                lblBreakTime.Hide();
                dtpBreakTime.Hide();

                pnlMonday.Show();
                pnlTuesday.Show();
                pnlWednesday.Show();
                pnlThursday.Show();
                pnlFriday.Show();
                pnlSaturday.Show();
                pnlSunday.Show();
            }
            else
            {
                txtStaffCostMonday.Show();
                txtStaffCostTuesday.Show();
                txtStaffCostWednesday.Show();
                txtStaffCostThursday.Show();
                txtStaffCostFriday.Show();
                txtStaffCostSaturday.Show();
                txtStaffCostSunday.Show();

                lblStaffCostMonday.Show();
                lblStaffCostTuesday.Show();
                lblStaffCostWednesday.Show();
                lblStaffCostThursday.Show();
                lblStaffCostFriday.Show();
                lblStaffCostSaturday.Show();
                lblStaffCostSunday.Show();

                lblBreakTime.Show();
                dtpBreakTime.Show();

                pnlMonday.Hide();
                pnlTuesday.Hide();
                pnlWednesday.Hide();
                pnlThursday.Hide();
                pnlFriday.Hide();
                pnlSaturday.Hide();
                pnlSunday.Hide();
            }
        }

        private void btnRealHours_Click(object sender, EventArgs e)
        {
            btnPlanning.Enabled = true;
            btnRealHours.Enabled = false;

            HideStaffCost(false);
            //TODO: show the real planning from data base
        }

        private void Keuken_Load(object sender, EventArgs e)
        {
            //sets the text of the form
            this.Text = repository.GetCurrentUser().ToFriendlyString();

            //sets the dates to the current date to start with
            //TODO: check for the bug that occured here
            dtpDateTimePicker.Value = DateTime.Now;
            dtpDateTimePicker.Value = SetMonday(false);
            dtpDateTimePicker.Value = dtpDateTimePicker.Value.AddHours(10);

            //fills the comboboxes
            cbxStaffMemberName.Items.AddRange(repository.GetListOfStaffMembers());
            cbxWorkplace.Items.AddRange(repository.GetPlannableFunctionsAvailableToUser());

            //set 
            //TODO: fill the planning with the planning from the database
        }

        private void txtExpectedRevenueMonday_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void dtpDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!busy)
            {
                dtpDateTimePicker.Value = SetMonday(false);

                busy = false;
            }
        }

        //set all date labels to correct date
        private void SetDateLabels(DateTime newdate)
        {
            DateTime temp = newdate;
            string format = "dddd dd MMMM yyyy";

            lblMonday.Text = temp.ToString(format); temp = temp.AddDays(1);
            lblTeusday.Text = temp.ToString(format); temp = temp.AddDays(1);
            lblWednesday.Text = temp.ToString(format); temp = temp.AddDays(1);
            lblThursday.Text = temp.ToString(format); temp = temp.AddDays(1);
            lblFriday.Text = temp.ToString(format); temp = temp.AddDays(1);
            lblSaturday.Text = temp.ToString(format); temp = temp.AddDays(1);
            lblSunday.Text = temp.ToString(format);
        }

        //sets the selected date to the monday of the selected week
        private DateTime SetMonday(bool increment)
        {
            DateTime tempDateTime = dtpDateTimePicker.Value;
            
            while (tempDateTime.DayOfWeek != DayOfWeek.Monday)
            {
                if (increment)
                {
                    tempDateTime = tempDateTime.AddDays(1);
                }
                else
                {
                    tempDateTime = tempDateTime.AddDays(-1);
                }
            }


            return tempDateTime;
        }

        private void btnMonthOverview_Click(object sender, EventArgs e)
        {
            //TODO: show a month overview for the logged in user
        }

        private void btnPlanning_Click(object sender, EventArgs e)
        {
            btnPlanning.Enabled = false;
            btnRealHours.Enabled = true;

            HideStaffCost(true);
            //TODO:Show The normal planning of the logged in user
        }

        private void btnAddToPlanning_Click(object sender, EventArgs e)
        {
            //check tijd
            if (dtpStartTime.Value.Hour < dtpEndTime.Value.Hour)
            {
                //check lege velden
                if (cbxStaffMemberName.Text != "" || cbxWorkplace.Text != "")
                {
                    //TODO: add a staffmember to the planning of the logged in person
                }
                else
                {
                    MessageBox.Show(error.NotEveryThingFilledInErrorMessage());
                }
            }
            else
            {
                MessageBox.Show(error.WrongTimeMessage());
            }
        }

        private void Keuken_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            //TODO: edit the selected record in the planning of the logged in user
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            //TODO: delete the selected record from the planning of the user
        }

        private void btnStaffMemberOverview_Click(object sender, EventArgs e)
        {
            //TODO: show an overview of all the staffmembers a person can plan
        }
    }
}
