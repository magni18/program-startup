using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramStartup
{
    public partial class Form1 : Form
    {
        private TextElementManager textElementManager;

        public Form1()
        {
            InitializeComponent();

            this.AllowDrop = true;

            var elementList = new List<RichTextBox>
            {
                richTextBox1,
                richTextBox2,
                richTextBox3, 
                richTextBox4, 
                richTextBox5,
                richTextBox6,
                richTextBox7, 
                richTextBox8,
                richTextBox9,
                richTextBox10,
                richTextBox11
            };

            textElementManager = new TextElementManager(elementList);

            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var text = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            textElementManager.SetTextOfCurrentElement(text[0]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textElementManager.ReadState();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textElementManager.RemoveElement();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textElementManager.RunCommands();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textElementManager.AddElement();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            textElementManager.WriteState();
        }
    }
}
