using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Fonts;
using MetroFramework.Forms;
using MetroFramework.Animation;
using MetroFramework.Controls;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            {
                Stream myStream = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;


                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string a = openFileDialog1.ToString();
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
                                saveFileDialog1.FileName = openFileDialog1.FileName;
                                //richTextBox1.SaveFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                                webBrowser1.DocumentText = richTextBox1.Text;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }


        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.FileName != "")
                {
                    webBrowser1.DocumentText = richTextBox1.Text;
                    File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
                }
                else
                {
                    MessageBox.Show("Save the File First");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save the File First");

            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
                    webBrowser1.DocumentText = richTextBox1.Text;
                }


            }
            else
            {
                MessageBox.Show("Write SomeThing in the Compiler");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {

                File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
                Process.Start("chrome", saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occur " + ex.ToString());
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            try {

                File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
                Process.Start("firefox", saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occur " + ex.ToString());
            }
        }
        Stack<string> tagstack = new Stack<string>();
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            bool boolcheck = true;

            {

                // string inline = 
                string[] data = File.ReadAllLines(saveFileDialog1.FileName);
                List<string> lstoftag = new List<string>();
                List<string> lstoferrors = new List<string>();
                int priviousopening = 0;
                bool opening = false;

                for (int k = 0; k < data.Length; k++)
                {
                    for (int i = 0; i < data[k].Length; i++)
                    {

                        if (opening)
                        {
                            if (data[k][i] == '>')
                            {

                                string tag = "";
                                for (int j = priviousopening; j <= i; j++)
                                {
                                    tag += data[k][j];
                                }
                                if (tag[1] == '/')//closing tag 
                                {
                                    List<string> lstem=new List<string>();
                                    bool milgya = false;
                                    while (tagstack.Count>0)
                                    {
                                        milgya = false;
                                        if (tagstack.Peek().Length == tag.Length - 1)
                                        {
                                            milgya = true;
                                            string check = tagstack.Peek();

                                            for (int l = 2; l < tag.Length - 1; l++)
                                            {
                                                if (check[l - 1] != tag[l])
                                                {
                                                    //error

                                                    lstoferrors.Add("Opening Tag Is Missing For Line :" + k);
                                                    check = null;
                                                    break;

                                                }
                                            }
                                            if (check != null)
                                            {
                                                tagstack.Pop();

                                            }
                                            for (int j = 0; j < lstem.Count; j++)
                                            {
                                                tagstack.Push(lstem[j]);
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            lstem.Add(tagstack.Pop());
                                        }
                                        
                                    }
                                    if (!milgya)
                                    {
                                        lstoferrors.Add("Opening Tag Is Missing For Line h :" + k);

                                    }




                                }

                                else
                                    tagstack.Push(tag);
                                //
                                lstoftag.Add(tag);

                                opening = false;
                            }
                            else if (data[k][i] == '<')
                            {
                                lstoferrors.Add("Closing Braces Is Missing In Line  :" + k + "\tIndex No :" + priviousopening);
                               
                                priviousopening = i;
                                int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();
                                int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);
                                string currentlinetext = richTextBox1.Lines[currentline];

                                richTextBox1.Select(firstcharindex, currentlinetext.Length);
                                richTextBox1.SelectionBackColor = Color.Red;
                                // tagstack.Push(" ");
                            }
                        }
                        else if (data[k][i] == '<')
                        {
                            priviousopening = i;
                            opening = true;
                        }
                        else if (data[k][i] == '>')
                        {
                            priviousopening = i;
                            lstoferrors.Add("Opening Braces Is Missing In Line :" + k + "\tIndex No :" + priviousopening);
                            // tagstack.Push(" ");
                            int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();
                            int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);
                            string currentlinetext = richTextBox1.Lines[currentline];

                            richTextBox1.Select(firstcharindex, currentlinetext.Length);
                            richTextBox1.SelectionBackColor = Color.Red;

                        }
                    }
                    if (opening)
                    {
                        opening = false;
                        lstoferrors.Add("Opening/Closing Braces Is Missing In Line  :" + k + "\tIndex No :" + priviousopening);
                        // tagstack.Push(" ");



                    }
                  
                    

                }
                if (lstoferrors.Count != 0)
                {
                    boolcheck = false;

                    for (int i = 0; i < lstoferrors.Count; i++)
                    {
                        MessageBox.Show(lstoferrors[i].ToString());
                    }
                  
                }
                else
                {
                    int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();
                    int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);
                    string currentlinetext = richTextBox1.Lines[currentline];

                    richTextBox1.Select(firstcharindex, currentlinetext.Length);
                    richTextBox1.SelectionBackColor = Color.White;
                    
                }
            }
            string filetext = File.ReadAllText("hardcode.txt");
            while (tagstack.Count>0)
            {
                
            
                if (filetext.Contains(tagstack.Peek()))
                {
                    tagstack.Pop();
                }
                else
                {
                    boolcheck = false;
                    MessageBox.Show("Closing Tag Not Found For This Tag : "+(tagstack.Pop()));
                    int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();
                    int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);
                    string currentlinetext = richTextBox1.Lines[currentline];

                    richTextBox1.Select(firstcharindex, currentlinetext.Length);
                    richTextBox1.SelectionBackColor = Color.Red;


                }
            }

            if (boolcheck)
            {
                MessageBox.Show("No Error Found");
            }
        }

    }
}
