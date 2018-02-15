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
    public partial class Barco : Form
    {
        LogicalRepository repository;

        public Barco(LogicalRepository logical)
        {
            InitializeComponent();

            repository = logical;
        }
    }
}
