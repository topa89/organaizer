﻿using System;
using System.Windows.Forms;

namespace Organaizer
{
    public partial class DialogText : Form
    {
     
        public DialogText()
        {
            InitializeComponent();
        }

        private void AddNoteBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        ~DialogText(){

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBoxNote.Text = "";
            Close();
        }

    
    }
}
