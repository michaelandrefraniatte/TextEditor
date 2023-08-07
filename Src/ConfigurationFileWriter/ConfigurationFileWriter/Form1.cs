using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigurationFileWriter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" & textBox2.Text != "")
            {
                try
                {
                    using (var output = System.IO.File.Create(textBox2.Text))
                    {
                        foreach (var file in System.IO.Directory.GetFiles(textBox1.Text, "*"))
                        {
                            using (var input = System.IO.File.OpenRead(file))
                            {
                                input.CopyTo(output);
                            }
                        }
                    }
                }
                catch (Exception error) 
                { 
                    MessageBox.Show(error.ToString()); 
                }
            }
            else { MessageBox.Show("error!"); }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }
    }
}
