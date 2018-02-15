using BarcoDenverPlanningSysteem.Classes.Error;
using BarcoDenverPlanningSysteem.Screens;
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
    public partial class Login : Form
    {
        LogicalRepository _logicRepo = new LogicalRepository();
        ErrorHandler error = new ErrorHandler();

        public Login()
        {
            InitializeComponent();
        }

        private void tbInlogCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btLogIn_Click(object sender, EventArgs e)
        {
            Form formToOpen = null;

            switch (_logicRepo.CheckLoginCode(tbInlogCode.Text))
            {
                case Workplace.Directie:
                    {
                        formToOpen = new Directie(_logicRepo);
                        break;
                    }
                case Workplace.Denver:
                    {
                        formToOpen = new Keuken(_logicRepo);
                        break;
                    }
                case Workplace.Barco:
                    {
                        formToOpen = new Keuken(_logicRepo);
                        break;
                    }
                case Workplace.Keuken:
                    {
                        formToOpen = new Keuken(_logicRepo);
                        break;
                    }
                case Workplace.NoFunctionDetected:
                    {
                        break;
                    }

                case Workplace.Fiesta:
                    {
                        formToOpen = new Keuken(_logicRepo);
                        break;
                    }
            }

            if (formToOpen != null)
            {
                this.Hide();
                formToOpen.Show();
            }
            else
            {
                MessageBox.Show(error.LoginErrorMessage());
            }
        }
    }
}
