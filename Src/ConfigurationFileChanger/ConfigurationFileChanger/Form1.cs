namespace ConfigurationFileChanger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string fromstring = ".";
            string replacestring = "";
            string[] splitstring = new string[14];
            splitstring = textBox1.Text.Split(", ");
            int inc = 0;
            List<string> newsplitstring = new List<string>();
            foreach (string valuestring in splitstring)
            {
                int pFrom = valuestring.IndexOf(fromstring);
                string newvaluestring;
                if (pFrom > 0)
                {
                    int pTo = valuestring.Length;
                    string result = valuestring.Substring(pFrom, pTo - pFrom);
                    newvaluestring = valuestring.Replace(result, replacestring);
                }
                else
                {
                    newvaluestring = valuestring;
                }
                newsplitstring.Add(newvaluestring);
                inc++;
            }
            textBox2.Text = "";
            foreach (string newvaluestring in newsplitstring)
            {
                textBox2.Text += newvaluestring + ", ";
            }
        }
    }
}