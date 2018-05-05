﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biomet_Project
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            PreviewKeyDown += HandleEsc;
        }

        private void HandleEsc(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
