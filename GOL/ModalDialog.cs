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
    public partial class ModalOptions : Form
    {
        public ModalOptions()
        {
            InitializeComponent();
        }

        public int Number
        {
            get { return (int)numericUpDownNum.Value; }
            set { numericUpDownNum.Value = value; }
        }
    }
}