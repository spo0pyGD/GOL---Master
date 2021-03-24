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
    public partial class ModalOptions : Form
    {
        public ModalOptions()
        {
            InitializeComponent();
        }

        //A prop for each numeric up/down box
        public int TimerInterval
        {
            get { return (int)numericUpDownTimerInterval.Value; }
            set { numericUpDownTimerInterval.Value = value; }
        }

        public int UniverseHeight
        {
            get { return (int)numericUpDownUniverseHeight.Value; }
            set { numericUpDownUniverseHeight.Value = value; }
        }

        public int UniverseWidth
        {
            get { return (int)numericUpDownUniverseWidth.Value; }
            set { numericUpDownUniverseWidth.Value = value; }
        }
    }
}