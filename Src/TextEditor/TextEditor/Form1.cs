using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using OpenWithSingleInstance;
using System.Runtime.InteropServices;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        private static string openFilePath = "", fileTextSaved = "";
        private static bool justSaved = true, justSavedbefore = true, onopenwith = false;
        private static DialogResult result;
        private static ContextMenu contextMenu = new ContextMenu();
        private static MenuItem menuItem;
        private static string wordsearch;
        private static string wordreplace;
        private static int pos = -1;
        private static string filename = "";
        public Form1(string filePath)
        {
            InitializeComponent();
            fileText.LanguageOption = RichTextBoxLanguageOptions.AutoFont;
            fileText.Font = new Font("Arial", 11);
            if (filePath != null)
            {
                onopenwith = true;
                OpenFileWith(filePath);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.WM_COPYDATA)
            {
                COPYDATASTRUCT _dataStruct = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                string _strMsg = Marshal.PtrToStringUni(_dataStruct.lpData, _dataStruct.cbData / 2);
                OpenFileWith(_strMsg);
            }
            base.WndProc(ref m);
        }
        public void OpenFileWith(string filePath)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    string txt = File.ReadAllText(filePath, Encoding.UTF8);
                    fileText.Text = txt;
                    filename = filePath;
                    openFilePath = filePath;
                    this.Text = filePath;
                    fileTextSaved = fileText.Text;
                    justSaved = true;
                }
            }
            else
            {
                string txt = File.ReadAllText(filePath, Encoding.UTF8);
                fileText.Text = txt;
                filename = filePath;
                openFilePath = filePath;
                this.Text = filePath;
                fileTextSaved = fileText.Text;
                justSaved = true;
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "New", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    fileText.Clear();
                    this.Text = "TextEditor";
                    openFilePath = ""; 
                    fileTextSaved = fileText.Text;
                    justSaved = true;
                }
            }
            else
            {
                fileText.Clear();
                this.Text = "TextEditor";
                openFilePath = "";
                fileTextSaved = fileText.Text;
                justSaved = true;
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Filter = "All Files(*.*)|*.*";
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        string txt = File.ReadAllText(op.FileName, Encoding.UTF8);
                        fileText.Text = txt;
                        filename = op.FileName;
                        openFilePath = op.FileName;
                        this.Text = op.FileName;
                        fileTextSaved = fileText.Text;
                        justSaved = true;
                    }
                }
            }
            else
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "All Files(*.*)|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    string txt = File.ReadAllText(op.FileName, Encoding.UTF8);
                    fileText.Text = txt;
                    filename = op.FileName;
                    openFilePath = op.FileName;
                    this.Text = op.FileName;
                    fileTextSaved = fileText.Text;
                    justSaved = true;
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFilePath == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                File.WriteAllText(openFilePath, fileText.Text, Encoding.UTF8);
                fileTextSaved = fileText.Text;
                justSaved = true;
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "All Files(*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sf.FileName, fileText.Text, Encoding.UTF8);
                filename = sf.FileName;
                openFilePath = sf.FileName;
                this.Text = sf.FileName;
                fileTextSaved = fileText.Text;
                justSaved = true;
            }
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Cut();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Copy();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Paste();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Undo();
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileText.Redo();
        }
        private void fileText_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fileText.ContextMenu = contextMenu;
            }
        }
        private void changeCursor(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void cutAction(object sender, EventArgs e)
        {
            fileText.Cut();
        }
        private void copyAction(object sender, EventArgs e)
        {
            if (fileText.SelectedText != "")
                Clipboard.SetText(fileText.SelectedText);
        }
        private void pasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                fileText.SelectedText = Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Exit", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
            if (filename != "")
            {
                using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\tempsave"))
                {
                    createdfile.WriteLine(filename);
                }
            }
        }
        private void fileText_TextChanged(object sender, EventArgs e)
        {
            if (fileTextSaved != fileText.Text)
                justSaved = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            menuItem = new MenuItem("Cut");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(cutAction);
            menuItem = new MenuItem("Copy");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(copyAction);
            menuItem = new MenuItem("Paste");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(pasteAction);
            fileText.ContextMenu = contextMenu;
        }
        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this);
            wordsearch = fileText.SelectedText;
            form3.Show();
        }
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            wordreplace = fileText.SelectedText;
            form2.Show();
        }
        public void search(bool fromstart, string word)
        {
            justSavedbefore = justSaved;
            if (fromstart)
                pos = fileText.Find(word, 0, RichTextBoxFinds.None);
            else
                pos = fileText.Find(word, fileText.SelectionStart + 1, RichTextBoxFinds.None);
            if (pos >= 0)
            {
                fileText.Select(pos, word.Length);
                fileText.SelectionBackColor = Color.Yellow;
                fileText.ScrollToCaret();
            }
            justSaved = justSavedbefore;
        }
        public string havewordsearch()
        {
            return wordsearch;
        }
        public void replaceAll(string searchText, string replacetext)
        {
            try
            {
                pos = fileText.Find(searchText, 0, RichTextBoxFinds.None);
                if (pos >= 0)
                {
                    fileText.Select(pos, searchText.Length);
                    fileText.ScrollToCaret();
                    if (fileText.SelectedText == searchText)
                        fileText.SelectedText = replacetext;
                }
                while (pos >= 0)
                {
                    pos = fileText.Find(searchText, fileText.SelectionStart + 1, RichTextBoxFinds.None);
                    if (pos >= 0)
                    {
                        fileText.Select(pos, searchText.Length);
                        fileText.ScrollToCaret();
                        if (fileText.SelectedText == searchText)
                            fileText.SelectedText = replacetext;
                    }
                }
            }
            catch { }
        }
        public void next(bool fromstart, string searchText)
        {
            justSavedbefore = justSaved;
            if (fromstart)
                pos = fileText.Find(searchText, 0, RichTextBoxFinds.None);
            else
                pos = fileText.Find(searchText, fileText.SelectionStart + 1, RichTextBoxFinds.None);
            if (pos >= 0)
            {
                fileText.Select(pos, searchText.Length);
                fileText.SelectionBackColor = Color.Yellow;
                fileText.ScrollToCaret();
            }
            justSaved = justSavedbefore;
        }
        public void replace(string searchText, string replacetext)
        {
            if (fileText.SelectedText == searchText)
                fileText.SelectedText = replacetext;
        }
        public void clearcolor(string word)
        {
            justSavedbefore = justSaved;
            try
            {
                string tempTxt = fileText.Text;
                fileText.Clear();
                fileText.Text = tempTxt;
                fileText.Select(pos, word.Length);
            }
            catch { }
            justSaved = justSavedbefore;
        }
        public string havewordreplace()
        {
            return wordreplace;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                if (!onopenwith)
                {
                    if (File.Exists(Application.StartupPath + @"\tempsave"))
                    {
                        using (System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tempsave"))
                        {
                            filename = file.ReadLine();
                        }
                        if (filename != "")
                        {
                            string txt = File.ReadAllText(filename, Encoding.UTF8);
                            fileText.Text = txt;
                            openFilePath = filename;
                            this.Text = filename;
                            fileTextSaved = fileText.Text;
                            justSaved = true;
                        }
                    }
                }
            }
            catch { }
        }
    }
}