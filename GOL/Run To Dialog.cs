using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL
{
    public partial class Run_To_Dialog : Form
    {
        public Run_To_Dialog()
        {
            InitializeComponent();
        }

        public int PickGeneration
        {
            get { return (int)numericUpDownGen.Value; }
            set { numericUpDownGen.Value = value; }
        }
    }
}