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
    public partial class Form3 : Form
    {
        public Form1 form1;
        public Form3(Form1 f)
        {
            this.form1 = f;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            form1.search(true, textBox1.Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            form1.search(false, textBox1.Text);
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = form1.havewordsearch();
        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.clearcolor(textBox1.Text);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            form1.clearcolor(textBox1.Text);
        }
    }
}
