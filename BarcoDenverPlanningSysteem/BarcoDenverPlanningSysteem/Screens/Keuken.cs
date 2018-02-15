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

        private DateTime lastDate;

        private bool firsttime;

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
            firsttime = true;

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
        }

        private void Keuken_Load(object sender, EventArgs e)
        {
            dtpDateTimePicker.Value = SetMonday(DateTime.Now);
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
            if (!firsttime)
            {
                DateTime dt;
                if (dtpDateTimePicker.Value < lastDate)
                {
                    dt = dtpDateTimePicker.Value.AddDays(-6);
                    dt = SetMonday(dt);
                }
                else
                {
                    dt = dtpDateTimePicker.Value.AddDays(6);
                    dt = SetMonday(dt);
                }

                firsttime = true;
                lastDate = dt;
                SetDateLabels(dt);
                dtpDateTimePicker.Value = dt;
            }
            else
            {
                firsttime = false;
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

        private DateTime SetMonday(DateTime temp)
        {
            DateTime tempDateTime = temp;

            int amountOfDays = 0;

            switch (tempDateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    amountOfDays = 6;
                    break;
                case DayOfWeek.Monday:
                    amountOfDays = 0;
                    break;
                case DayOfWeek.Tuesday:
                    amountOfDays = 1;
                    break;
                case DayOfWeek.Wednesday:
                    amountOfDays = 2;
                    break;
                case DayOfWeek.Thursday:
                    amountOfDays = 3;
                    break;
                case DayOfWeek.Friday:
                    amountOfDays = 4;
                    break;
                case DayOfWeek.Saturday:
                    amountOfDays = 5;
                    break;
                default:
                    break;
            }

            DateTime dt = tempDateTime.AddDays(-amountOfDays);

            return dt;
        }

        private void btnMonthOverview_Click(object sender, EventArgs e)
        {

        }

        private void btnPlanning_Click(object sender, EventArgs e)
        {
            btnPlanning.Enabled = false;
            btnRealHours.Enabled = true;

            HideStaffCost(true);
        }
    }
}
