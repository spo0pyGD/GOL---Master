﻿using System;
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
    public partial class TimerDLG : Form
    {
        public TimerDLG()
        {
            InitializeComponent();
        }

        public int TimerInterval
        {
            get { return (int)numericUpDownTimerInterval.Value; }
            set { numericUpDownTimerInterval.Value = value; }
        }
    }
}
