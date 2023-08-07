using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TextEditor
{
    public partial class Form2 : Form
    {
        public Form1 form1;
        public Form2(Form1 f)
        {
            this.form1 = f;
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = form1.havewordreplace();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            form1.next(true, textBox1.Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            form1.next(false, textBox1.Text);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            form1.replaceAll(textBox1.Text, textBox2.Text);
            this.Hide();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            form1.replace(textBox1.Text, textBox2.Text);
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.clearcolor(textBox1.Text);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            form1.clearcolor(textBox1.Text);
        }
    }
}
