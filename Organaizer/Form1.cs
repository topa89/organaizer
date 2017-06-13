﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Organaizer
{
    public partial class Form1 : Form
    {
        List<TextBox> txtboxList = new List<TextBox>();
        List<CheckBox> chkboxList = new List<CheckBox>();
        public CheckBox chkbox;
        private TextBox txtboxNote;
        private TextBox txtBoxCheckBox;
        public List<string> timeRemind = new List<string>();
        public List<string> textRemind = new List<string>();
        public List<string> dateRemind = new List<string>();
        int positionChkBox = 0;
        int j = 0;
        bool checkPressedButton = true;


        public Form1()
        {
            InitializeComponent();
            panel1.HorizontalScroll.Enabled = false;
            panel1.VerticalScroll.Enabled = true;
            panel1.VerticalScroll.Visible = false;
            timer1.Interval = 1000;
            timer1.Stop();
            buttonShowReminders.Enabled = false;
            //NotificShow(5, "Проверка");
        }

        void NotificShow(int secondClose, string text) // Метод для вызова уведомления.
        {
            Tasks notific = new Tasks(secondClose, text);
            notific.Visible = true;
        }


        public void CreateLbl(string text)
        {
            txtboxNote = new TextBox();
            txtboxNote.Multiline = true;
            txtboxNote.WordWrap = true;
            txtboxNote.ReadOnly = true;
            txtboxNote.Size = new Size(panel1.ClientSize.Width, panel1.ClientSize.Height/5);
            txtboxNote.Text = text;
            txtboxNote.Anchor = ( AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
            txtboxNote.Location = new Point(0, j);
            j += panel1.ClientSize.Width/3;
          
            txtboxNote.Font = new Font(txtboxNote.Font.Name, 8, FontStyle.Regular);
            txtboxNote.BackColor = Color.White;
            txtboxNote.DoubleClick += new EventHandler(TextBoxDoubleClicked);
             
            panel1.Controls.Add(txtboxNote);

            txtboxList.Add(txtboxNote);
        }

        private void TextBoxDoubleClicked(object sender, EventArgs e)
        {
            //creating changed textbox
            TextBox doubleClickedTextBox = (TextBox)sender; 
            DialogText f = new DialogText();
            f.textBoxNote.Text = doubleClickedTextBox.Text;
            f.textBoxNote.SelectionStart = f.textBoxNote.TextLength+1;
            f.ShowDialog();

            if (f.textBoxNote.TextLength == 0)
            {
                txtboxList.Remove(doubleClickedTextBox);
                panel1.Controls.Clear();
                j = 0;
               foreach (TextBox txtbox in txtboxList)
                {
                    txtbox.Location = new Point(3, j);
                    j += 80;
                    panel1.Controls.Add(txtbox);
                    //label.Text = j.ToString();
                }
                
            }
            else doubleClickedTextBox.Text = f.textBoxNote.Text;
            f.Dispose();
            
        }
         
        //создание заметки    
        private void button1_Click(object sender, EventArgs e)
        {
            DialogText f = new DialogText();
            f.ShowDialog();
            if (f.textBoxNote.Text == "") ;
            else CreateLbl(f.textBoxNote.Text);
        }

        //обработка нажатия checkbox
        private void PressCheckBox(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;

            if (checkbox.Checked)
            {
                //изменение checkbox
                checkbox.Enabled = false;
                checkbox.Font = new Font(checkbox.Font, FontStyle.Strikeout);
               
                chkboxList.Remove(checkbox);
                chkboxList.Add(checkbox);

                positionChkBox = 0;
                panel2.Controls.Clear();
               foreach (CheckBox checkboxlst in chkboxList)
                {
                    checkboxlst.Location = new Point(0, positionChkBox);
                    positionChkBox += 22;
                    checkboxlst.Size = new Size(panel2.ClientSize.Height, 25);
                    panel2.Controls.Add(checkboxlst);
                    checkboxlst.Show();
                }
            }
        }

        //создане checkbox
        private void CreateCheckBox()
        {
            CheckBox chckbox = new CheckBox();
            TextBox txtbox = new TextBox();
            txtbox.KeyPress += new KeyPressEventHandler(textBoxForChkbox_Click);
            chckbox.Click += new EventHandler(PressCheckBox);
            chckbox.Location = new Point(0, positionChkBox);
            positionChkBox += 2;
            txtbox.Location = new Point(20, positionChkBox);
            txtbox.Size = new Size(panel2.ClientSize.Height, 10);
            chckbox.Size = new Size(panel2.ClientSize.Height, 25);
            positionChkBox += 20;
            panel2.Controls.Add(txtbox);
            txtbox.Show();
            chkbox = chckbox;
            panel2.Controls.Add(chckbox);
            chckbox.Show();
            chkboxList.Add(chckbox);
            txtbox.Focus();
            txtBoxCheckBox = txtbox;
           
        }

     //кнопка создания checkbox
        private void button2_Click(object sender, EventArgs e)
        {
            CreateCheckBox();
        }

        //выбор даты
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

        //обработка textbox для checkbox, вписывание данных
        private void textBoxForChkbox_Click(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (txtBoxCheckBox.Text == "")
                {
                    txtBoxCheckBox.Dispose();
                    chkbox.Dispose();
                    positionChkBox -= 22;
                    chkboxList.Remove(chkbox);
                }

                else
                {
                    chkbox.Text = txtBoxCheckBox.Text.ToString();
                  
                    txtBoxCheckBox.Dispose(); //уничтожение обьекта
                    CreateCheckBox();
                }
            }

            else if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                MessageBox.Show("Нажат Escape");
            }
        }

        //код для напоминаний
        private void button3_Click(object sender, EventArgs e)
        {
            ReminderAdd reminder = new ReminderAdd();
            reminder.ShowDialog();
            timeRemind.Add(reminder.timePicker.Value.ToShortTimeString());
            textRemind.Add(reminder.textBoxReminder.Text.ToString());
            dateRemind.Add(reminder.datePicker.Value.ToShortDateString());
            CheckTimer();
            buttonShowReminders.Enabled = true;
            CreateRemindCard(dateRemind[dateRemind.Count - 1], timeRemind[timeRemind.Count - 1], textRemind[textRemind.Count - 1]);
        }

        //создание карты напоминания
        void CreateRemindCard(string date, string time, string text)
        {
            TextBox txtbox = new TextBox();
            panel3.Controls.Add(txtbox);
            txtbox.Multiline = true;
            txtbox.Text = "Время: " + time + "Текст: " + text;
            txtbox.Size = new Size(152, 76);
        }

        //проверка обьектов на наличие
        void CheckTimer()
        {
            if (timeRemind.Count == 0) { timer1.Stop(); buttonShowReminders.Enabled = false; }
            else timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckTimer();
            for (int i = 0; i < timeRemind.Count; i++)
            {
                
                if (timeRemind[i] == DateTime.Now.ToShortTimeString())
                {
                    NotificShow(10, textRemind[i]);
                    timeRemind.Remove(timeRemind[i]);
                    textRemind.Remove(textRemind[i]);
                    dateRemind.Remove(dateRemind[i]);
                    break;
                  
                }
            }
        }

        private void buttonShowReminders_Click(object sender, EventArgs e)
        {
            if (checkPressedButton)
            {
                groupBox4.Visible = true;
                panel4.Visible = true;
                int x = 0;
                panel4.Controls.Clear();
                for (int i = 0; i < textRemind.Count; i++)
                {
                    TextBox textbox = new TextBox();
                    textbox.Multiline = true;
                    textbox.Text = textRemind[i].ToString() + "\n" + timeRemind[i].ToString();
                    textbox.Location = new Point(3, x);
                    textbox.Size = new Size(152, 76);
                    x += 80;
                    panel4.Controls.Add(textbox);
                }
                buttonShowReminders.Text = "Скрыть";
                checkPressedButton = false;
            }
            else
            {
                buttonShowReminders.Text = "Показать все";
                groupBox4.Visible = false;
                panel4.Visible = false;
                checkPressedButton = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

   
}