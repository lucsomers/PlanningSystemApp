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
    public partial class Denver : Form
    {
        LogicalRepository repository;

        public Denver(LogicalRepository logical)
        {
            InitializeComponent();

            repository = logical;
        }
    }
}
