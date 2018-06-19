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
            //set screen to dubble buffor for smoother rendering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //setting variables
            lastDate = dtpDateTimePicker.Value;
            busy = false;
            error = new ErrorHandler();

            //hiding correct buttons
            HideStaffCost(true);
        }
        
        private void btnRealHours_Click(object sender, EventArgs e)
        {
            btnPlanning.Enabled = true;
            btnRealHours.Enabled = false;

            HideStaffCost(false);

            FillTables();
        }

        private void Keuken_Load(object sender, EventArgs e)
        {
            //sets the text of the form
            this.Text = repository.GetCurrentUser().ToFriendlyString();

            //sets the dates to the current date to start with
            dtpDateTimePicker.Value = DateTime.Now;
            dtpDateTimePicker.Value = SetMonday(false);
            dtpDateTimePicker.Value = dtpDateTimePicker.Value.AddHours(10);

            dtpChosenDateForStaffMember.Value = DateTime.Now;

            //fills the comboboxes
            cbxStaffMemberName.Items.AddRange(repository.GetListOfStaffMembers());
            cbxWorkplace.Items.AddRange(repository.GetPlannableFunctionsAvailableToUser());

            //set 
            FillTables();
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
                SetDateLabels(dtpDateTimePicker.Value);
                FillTables();
                busy = false;
            }
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
            FillTables();
        }

        private void btnAddToPlanning_Click(object sender, EventArgs e)
        {
            int planningId = -1;

            //check tijd
            if ((dtpStartTime.Value.Hour == dtpEndTime.Value.Hour && dtpStartTime.Value.Minute < dtpEndTime.Value.Minute) || (dtpStartTime.Value.Hour < dtpEndTime.Value.Hour))
            {
                //check lege velden
                if (cbxStaffMemberName.Text != "" || cbxWorkplace.Text != "")
                {
                    //check of we realiteit toevoegen zo niet dan is het planning
                    if (!btnRealHours.Enabled)
                    {
                        //realiteit
                        planningId = repository.AddStaffMemberToPlanning(cbxWorkplace.Text, dtpChosenDateForStaffMember.Value, btnRealHours.Enabled, cbxStaffMemberName.Text, dtpStartTime.Value, dtpEndTime.Value, dtpBreakTime.Value);
                    }
                    else
                    {
                        //planning
                        planningId = repository.AddStaffMemberToPlanning(cbxWorkplace.Text, dtpChosenDateForStaffMember.Value, btnRealHours.Enabled, cbxStaffMemberName.Text, dtpStartTime.Value, dtpEndTime.Value);
                    }
                    FillTables(planningId);
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
            if (MessageBox.Show("Met de volgende actie gaan de geselecteerde rijen permanent verloren. Weet u zeker dat u door wilt gaan?","Weet u het zeker",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int[] idOfStaffMembers = repository.GetIdOfSelectedRow(new DataGridView[] { dgvFriday, dgvMonday, dgvSaturday, dgvSunday, dgvThursday, dgvTuesday, dgvWednesday });
                if (idOfStaffMembers.Length > 0)
                {
                    repository.DeleteStaffmembersFromPlanningByID(idOfStaffMembers);
                    FillTables();
                }
                else
                {
                    MessageBox.Show(error.NoRowsSelectedMessage());
                }
            }
        }

        private void btnStaffMemberOverview_Click(object sender, EventArgs e)
        {
            //TODO: show an overview of all the staffmembers a person can plan

        }
        private void cbxStaffMemberName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fill function with default function
            cbxWorkplace.Text = repository.GetDefaultFunctionById( repository.getIdFromName(cbxStaffMemberName.Text));
        }

        private void SelectionDGVChanged(object sender, EventArgs e)
        {
            DataGridView temp = (DataGridView)sender;
            repository.DeselectCells(new DataGridView[] {temp ,dgvFriday, dgvMonday, dgvSaturday, dgvSunday, dgvThursday, dgvTuesday, dgvWednesday });

        }

        #region Private Function not connected to events
        //Fill all tables with data from the database
        private void FillTables(int planningid = -1)
        {
            //TODO: Expected revenue show on screen

            //clear all text boxes before filling the right ones 
            clearDayOfWeekTextBoxes();

            #region row clears and filling tables and commentbox
            //rows clear
            dgvMonday.Rows.Clear();
            repository.FillPlanningtableWithData(dgvMonday, dtpDateTimePicker.Value, btnRealHours.Enabled, tbxCommentMonday, planningid);
            
            //rows clear
            dgvTuesday.Rows.Clear();
            repository.FillPlanningtableWithData(dgvTuesday, dtpDateTimePicker.Value.AddDays(1), btnRealHours.Enabled, tbxCommentTuesday, planningid);


            //rows clear
            dgvWednesday.Rows.Clear();
            repository.FillPlanningtableWithData(dgvWednesday, dtpDateTimePicker.Value.AddDays(2), btnRealHours.Enabled, tbxCommentWednesday, planningid);


            //rows clear
            dgvThursday.Rows.Clear();
            repository.FillPlanningtableWithData(dgvThursday, dtpDateTimePicker.Value.AddDays(3), btnRealHours.Enabled, tbxCommentThursday, planningid);


            //rows clear
            dgvFriday.Rows.Clear();
            repository.FillPlanningtableWithData(dgvFriday, dtpDateTimePicker.Value.AddDays(4), btnRealHours.Enabled, tbxCommentFriday, planningid);


            //rows clear
            dgvSaturday.Rows.Clear();
            //table fill
            repository.FillPlanningtableWithData(dgvSaturday, dtpDateTimePicker.Value.AddDays(5), btnRealHours.Enabled, tbxCommentSaturday, planningid);


            //rows clear
            dgvSunday.Rows.Clear();
            //table fill
            repository.FillPlanningtableWithData(dgvSunday, dtpDateTimePicker.Value.AddDays(6), btnRealHours.Enabled, tbxCommentSunday, planningid);
            #endregion

            #region fill textboxes
            FillTextboxes(dgvSunday, DayOfWeek.Sunday);
            FillTextboxes(dgvSaturday, DayOfWeek.Saturday);
            FillTextboxes(dgvFriday, DayOfWeek.Friday);
            FillTextboxes(dgvThursday, DayOfWeek.Thursday);
            FillTextboxes(dgvWednesday, DayOfWeek.Wednesday);

            FillTextboxes(dgvTuesday, DayOfWeek.Tuesday);
            FillTextboxes(dgvMonday, DayOfWeek.Monday);
            #endregion
        }

        private void FillTextboxes(DataGridView dgv, DayOfWeek dayOfWeek)
        {
            Dictionary<string, int> dir = new Dictionary<string, int>();

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersSunday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenSunday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersSunday.Text = dir["bediening"].ToString();
                        txtAmountOfStandBySunday.Text = dir["standby"].ToString();
                    }
                    break;

                case DayOfWeek.Monday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersMonday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenMonday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersMonday.Text = dir["bediening"].ToString();
                        txtAmountOfStandByMonday.Text = dir["standby"].ToString();
                    }
                    break;

                case DayOfWeek.Tuesday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersTuesday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenTuesday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersTuesday.Text = dir["bediening"].ToString();
                        txtAmountOfStandbyTuesday.Text = dir["standby"].ToString();
                    }
                    break;

                case DayOfWeek.Wednesday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersWednesday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenWednesday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersWednesday.Text = dir["bediening"].ToString();
                        txtAmountOfStandByWednesday.Text = dir["standby"].ToString();
                    }
                    break;

                case DayOfWeek.Thursday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersThursday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenThursday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersThursday.Text = dir["bediening"].ToString();
                        txtAmountOfStandByThursday.Text = dir["standby"].ToString();
                    }
                    break;

                case DayOfWeek.Friday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersFriday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenFriday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersFriday.Text = dir["bediening"].ToString();
                        txtAmountOfStandByFriday.Text = dir["standby"].ToString();
                    }
                    break;

                case DayOfWeek.Saturday:
                    dir = repository.CountNumbers(dgv);
                    if (dir.Count >= 1)
                    {
                        txtAmountOfCleanersSaturday.Text = dir["afwas"].ToString();
                        txtAmountOfKitchenSaturday.Text = (dir["denver keuken"] + dir["barco keuken"]).ToString();
                        txtAmountOfServersSaturday.Text = dir["bediening"].ToString();
                        txtAmountOfSStandBySaturday.Text = dir["standby"].ToString();
                    }
                    break;

                default:
                    break;
            }
        }

        private void clearDayOfWeekTextBoxes()
        {
            txtAmountOfCleanersFriday.Text = "";
            txtAmountOfCleanersMonday.Text = "";
            txtAmountOfCleanersSaturday.Text = "";
            txtAmountOfCleanersSunday.Text = "";
            txtAmountOfCleanersThursday.Text = "";
            txtAmountOfCleanersTuesday.Text = "";
            txtAmountOfCleanersWednesday.Text = "";
            txtAmountOfKitchenFriday.Text = "";
            txtAmountOfKitchenMonday.Text = "";
            txtAmountOfKitchenSaturday.Text = "";
            txtAmountOfKitchenSunday.Text = "";
            txtAmountOfKitchenThursday.Text = "";
            txtAmountOfKitchenTuesday.Text = "";
            txtAmountOfKitchenWednesday.Text = "";
            txtAmountOfServersFriday.Text = "";
            txtAmountOfServersMonday.Text = "";
            txtAmountOfServersSaturday.Text = "";
            txtAmountOfServersSunday.Text = "";
            txtAmountOfServersThursday.Text = "";
            txtAmountOfServersTuesday.Text = "";
            txtAmountOfServersWednesday.Text = "";
            txtAmountOfSStandBySaturday.Text = "";
            txtAmountOfStandByFriday.Text = "";
            txtAmountOfStandByMonday.Text = "";
            txtAmountOfStandBySunday.Text = "";
            txtAmountOfStandByThursday.Text = "";
            txtAmountOfStandbyTuesday.Text = "";
            txtAmountOfStandByWednesday.Text = "";
            txtExpectedRevenueFriday.Text = "";
            txtExpectedRevenueMonday.Text = "";
            txtExpectedRevenueSaturday.Text = "";
            txtExpectedRevenueSunday.Text = "";
            txtExpectedRevenueThursday.Text = "";
            txtExpectedRevenueTuesday.Text = "";
            txtExpectedRevenueWednesday.Text = "";
            tbxCommentFriday.Text = "";
            tbxCommentMonday.Text = "";
            tbxCommentSaturday.Text = "";
            tbxCommentSunday.Text = "";
            tbxCommentThursday.Text = "";
            tbxCommentTuesday.Text = "";
            tbxCommentWednesday.Text = "";
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
        private void HideStaffCost(bool hide)
        {
            if (hide)
            {
                //planning
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

                dgvMonday.Columns["pause_timeMonday"].Visible = false;
                dgvTuesday.Columns["pause_timeTuesday"].Visible = false;
                dgvWednesday.Columns["pause_timeWednesday"].Visible = false; 
                dgvThursday.Columns["pause_timeThursday"].Visible = false;
                dgvFriday.Columns["pause_timeFriday"].Visible = false;
                dgvSaturday.Columns["pause_timeSaturday"].Visible = false;
                dgvSunday.Columns["pause_timeSunday"].Visible = false;

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
                //real
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

                dgvMonday.Columns["pause_timeMonday"].Visible = true;
                dgvTuesday.Columns["pause_timeTuesday"].Visible = true;
                dgvWednesday.Columns["pause_timeWednesday"].Visible = true;
                dgvThursday.Columns["pause_timeThursday"].Visible = true;
                dgvFriday.Columns["pause_timeFriday"].Visible = true;
                dgvSaturday.Columns["pause_timeSaturday"].Visible = true;
                dgvSunday.Columns["pause_timeSunday"].Visible = true;

                pnlMonday.Hide();
                pnlTuesday.Hide();
                pnlWednesday.Hide();
                pnlThursday.Hide();
                pnlFriday.Hide();
                pnlSaturday.Hide();
                pnlSunday.Hide();
            }
        }


        #endregion

        
    }
}
