using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcoDenverPlanningSysteem.Screens
{
    public partial class Fiesta : Form
    {
        LogicalRepository repository;

        public Fiesta(LogicalRepository logical)
        {
            InitializeComponent();

            repository = logical;
        }
    }
}
