using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using MetroFramework.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : MetroForm
    {
        private readonly List<TagsElement> _tagsColletion;
        private readonly List<CSSElement> _cSSElements;
        private readonly List<cssPropertyValues> _cssPropertyValues;
        private readonly Stack<string> _tagStack = new Stack<string>();
        private readonly ListBox _autoCompleteListBox;
        private bool _hasPopUpStarted = false;
        private string _keyword = string.Empty;
        private string _cssKeyword = string.Empty;
        private string _cssValKeyword = string.Empty;
        private bool _candidateForAutoCorrect = true;
        public bool _isCSSStarted = false;
        public bool _cssValue = false;
        public bool html = true; public bool head = true; public bool body = true;

        [DllImport("user32")]

        private static extern int GetCaretPos(out Point p);
        private Point _cp;
        public Form1()
        {
            InitializeComponent();
            _autoCompleteListBox = new ListBox();

            _autoCompleteListBox.KeyDown += (e, r) =>
            {
                if (r.KeyCode == Keys.Escape)
                {
                    _autoCompleteListBox.Items.Clear();
                    _autoCompleteListBox.Hide();
                    _autoCompleteListBox.Focus();
                }
                else if (r.KeyCode == Keys.Enter)
                {
                    if (_isCSSStarted == true)
                    {
                        int cursorPosition = codeRichBox.SelectionStart - 1;
                        string incompleteKeyword = string.Empty;
                        if (cursorPosition == -1) return;
                        incompleteKeyword = codeRichBox.Text[cursorPosition--] + incompleteKeyword;
                        string text = _autoCompleteListBox.SelectedItem as string;
                        cursorPosition = codeRichBox.SelectionStart - 1;
                        codeRichBox.SelectionStart = cursorPosition - (incompleteKeyword.Length);
                        codeRichBox.SelectionLength = incompleteKeyword.Length;
                        codeRichBox.SelectedText = text;
                    }
                    else if (_cssValue == true)
                    {
                        int cursorPosition = codeRichBox.SelectionStart - 1;
                        string incompleteKeyword = string.Empty;
                        if (cursorPosition == -1) return;
                        incompleteKeyword = codeRichBox.Text[cursorPosition--] + incompleteKeyword;
                        string text = _autoCompleteListBox.SelectedItem as string;
                        cursorPosition = codeRichBox.SelectionStart - 1;
                        codeRichBox.SelectionStart = cursorPosition - (incompleteKeyword.Length);
                        codeRichBox.SelectionLength = incompleteKeyword.Length;
                        codeRichBox.SelectedText = text;
                    }
                    else
                    {
                        int cursorPosition = codeRichBox.SelectionStart - 1;
                        string incompleteKeyword = string.Empty;
                        if (cursorPosition == -1) return;
                        while (codeRichBox.Text[cursorPosition] != '<')
                            incompleteKeyword = codeRichBox.Text[cursorPosition--] + incompleteKeyword;
                        incompleteKeyword = '<' + incompleteKeyword;
                        string text = _autoCompleteListBox.SelectedItem as string;
                        cursorPosition = codeRichBox.SelectionStart - 1;
                        codeRichBox.SelectionStart = cursorPosition - (incompleteKeyword.Length - 1);
                        codeRichBox.SelectionLength = incompleteKeyword.Length;
                        codeRichBox.SelectedText = text ?? "";
                        if (text == "<html>")
                        {
                            var item = _tagsColletion.SingleOrDefault(x => x.OpeningTag == "<html>");
                            _tagsColletion.Remove(item);
                        }
                        if (text == "<body>")
                        {
                            var item = _tagsColletion.SingleOrDefault(x => x.OpeningTag == "<body>");
                            _tagsColletion.Remove(item);
                        }
                        if (text == "<head>")
                        {
                            var item = _tagsColletion.SingleOrDefault(x => x.OpeningTag == "<head>");
                            _tagsColletion.Remove(item);
                        }
                    }
                }
            };
            _tagsColletion = new List<TagsElement>
            {
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<html>",
                    ClosingTag = "</html>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<table>",
                    ClosingTag = "</table>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<div>",
                    ClosingTag = "</div>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<p>",
                    ClosingTag = "</p>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<h1>",
                    ClosingTag = "</h1>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<tr>",
                    ClosingTag = "</tr>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<td>",
                    ClosingTag = "</td>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<ol>",
                    ClosingTag = "</ol>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<li>",
                    ClosingTag = "</li>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<ul>",
                    ClosingTag = "</ul>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<br>",
                    ClosingTag = "</br>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<h2>",
                    ClosingTag = "</h2>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<h3>",
                    ClosingTag = "</h3>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<h4>",
                    ClosingTag = "</h4>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<h5>",
                    ClosingTag = "</h5>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<h6>",
                    ClosingTag = "</h6>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<th>",
                    ClosingTag = "</th>"
                },
                new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<body>",
                    ClosingTag = "</body>"
                },
                 new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<style>",
                    ClosingTag = "</style>"
                },
                  new TagsElement
                {
                    IsPairedTag = true,
                    OpeningTag = "<head>",
                    ClosingTag = "</head>"
                }
            };
            _cSSElements = new List<CSSElement>
            {
                new CSSElement
                {
                    PropertyName="color"
                },
                 new CSSElement
                {
                    PropertyName="clear"
                },
                  new CSSElement
                {
                    PropertyName="cursor"
                },
                  new CSSElement
                {
                    PropertyName="display"
                },
                  new CSSElement
                {
                    PropertyName="float"
                },
                   new CSSElement
                {
                    PropertyName="position"
                },
                    new CSSElement
                {
                    PropertyName="visibility"
                },
                new CSSElement
                {
                    PropertyName="width"
                },
                 new CSSElement
                {
                    PropertyName="height"
                },
                  new CSSElement
                {
                    PropertyName="background-color"
                }
                  ,
                  new CSSElement
                {
                    PropertyName="background-attachment"
                }
                  ,
                  new CSSElement
                {
                    PropertyName="background"
                }
                  ,
                  new CSSElement
                {
                    PropertyName="background-image"
                },
                    new CSSElement
                {
                    PropertyName="background-position"
                },
                      new CSSElement
                {
                    PropertyName="background-repeat"
                },
                   new CSSElement
                {
                    PropertyName="font-weight"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="font-variant"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="font-style"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="font-stretch"
                }
                    ,
                   new CSSElement
                {
                    PropertyName="font-size-adjust"
                }
                    ,
                   new CSSElement
                {
                    PropertyName="font-size"
                }
                    ,
                   new CSSElement
                {
                    PropertyName="font-family"
                }
                    ,
                   new CSSElement
                {
                    PropertyName="font"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="margin"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="margin-bottom"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="margin-left"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="margin-right"
                }
                   ,
                   new CSSElement
                {
                    PropertyName="margin-top"
                }

            };
            _cssPropertyValues = new List<cssPropertyValues>
            {
                new cssPropertyValues
                {
                    PropertyValue = "auto;"
                },
                new cssPropertyValues
                {
                    PropertyValue = "left;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "right;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "none;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "static;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "relative;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "absolute;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "fixed;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "visible;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "hidden;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "collapse;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "xx-small;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "x-small;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "small;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "medium;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "large;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "smaller;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "larger;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "normal;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "bold;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "bolder;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "lighter;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "100;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "200;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "300;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "400;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "500;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "600;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "700;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "800;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "900;"
                }
                  ,
                new cssPropertyValues
                {
                    PropertyValue = "px;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "%;"
                }
                 ,
                new cssPropertyValues
                {
                    PropertyValue = "red;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "blue;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "green;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "orange;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "purple;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "pink;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "yellow;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "black;"
                }
                ,
                new cssPropertyValues
                {
                    PropertyValue = "white;"
                }
            };
        }
        private void CodeRichBox_TextChange(object sender, EventArgs e)
        {
            if (liveViewCheckBox.Checked)
                webBrowser1.DocumentText = codeRichBox.Text;

            if (codeRichBox.TextLength == 0)
            {
                _autoCompleteListBox.Hide();
                return;
            }

            int cursorPos = codeRichBox.SelectionStart;

            if (cursorPos > 0)
            {
                var typedCharacter = codeRichBox.Text[cursorPos - 1];

                if (typedCharacter == '{')
                {
                    _isCSSStarted = true;
                    codeRichBox.SelectedText = "}";
                    _autoCompleteListBox.Items.Clear();
                    _cssKeyword = typedCharacter.ToString();
                    PopulateTextBoxFroCss(_cssKeyword);
                }
                if (typedCharacter == '"')
                {
                    _isCSSStarted = true;
                    codeRichBox.SelectedText = '"'.ToString();
                    _autoCompleteListBox.Items.Clear();
                    _cssKeyword = typedCharacter.ToString();
                    PopulateTextBoxFroCss(_cssKeyword);
                }
                if (typedCharacter == '}')
                {
                    _autoCompleteListBox.Hide();
                    _isCSSStarted = false;
                }
                if (typedCharacter == ':')
                {
                    _isCSSStarted = false;
                    _cssValue = true;
                    _autoCompleteListBox.Items.Clear();
                    _cssValKeyword = typedCharacter.ToString();
                    PopulateTextBoxFroCssValue(_cssValKeyword);
                }
                if (typedCharacter == ';')
                {
                    _isCSSStarted = true;
                    _cssValue = false;
                    _hasPopUpStarted = false;
                }

                if (typedCharacter == '<')
                {
                    _hasPopUpStarted = true;
                    _keyword = "<";
                    _autoCompleteListBox.Items.Clear();
                    PopulateTextBoxFroHtml(_keyword);
                }
                else if (typedCharacter == '>')
                {
                    _cssValue = false;
                    _isCSSStarted = false;
                    _autoCompleteListBox.Hide();
                    _hasPopUpStarted = false;
                    int newchar = codeRichBox.SelectionStart;
                }

                else
                {
                    if (_hasPopUpStarted)
                    {
                        _autoCompleteListBox.Items.Clear();
                        _keyword = _keyword + typedCharacter;
                        PopulateTextBoxFroHtml(_keyword);
                    }
                    else if (_isCSSStarted)
                    {
                        _autoCompleteListBox.Items.Clear();
                        _cssKeyword = typedCharacter.ToString();
                        PopulateTextBoxFroCss(_cssKeyword);
                    }
                    else if (_cssValue)
                    {
                        _autoCompleteListBox.Items.Clear();
                        _cssValKeyword = typedCharacter.ToString();
                        PopulateTextBoxFroCssValue(_cssValKeyword);
                    }
                }
            }
            //auto closing tag
            if (!_candidateForAutoCorrect) return;

            string keywordForAutoComplete = string.Empty;
            cursorPos = codeRichBox.SelectionStart;

            if (cursorPos > 0)
            {
                var typedCharacter = codeRichBox.Text[cursorPos - 1];

                int counter = cursorPos - 1;

                if (typedCharacter == '>')
                {
                    while (codeRichBox.Text[counter] != '<')
                    {
                        keywordForAutoComplete = codeRichBox.Text[counter--] + keywordForAutoComplete;
                        if (counter == -1) return;
                    }

                    cursorPos = codeRichBox.SelectionStart;

                    keywordForAutoComplete = '<' + keywordForAutoComplete;

                    var closing = _tagsColletion?.FirstOrDefault(p => p.OpeningTag.Equals(keywordForAutoComplete))?.ClosingTag;


                    codeRichBox.SelectionStart = cursorPos;

                    codeRichBox.SelectedText = Environment.NewLine;
                    codeRichBox.SelectedText = "\r";

                    codeRichBox.SelectedText = closing ?? "";

                    codeRichBox.SelectionStart = cursorPos;

                    codeRichBox.Focus();

                }
            }
        }
        private void PopulateTextBoxFroCssValue(string pName)
        {
            var list = _cssPropertyValues.Where(p => p.PropertyValue.StartsWith(pName));
            GetCaretPos(out _cp);
            _autoCompleteListBox.SetBounds(_cp.X, _cp.Y + 15, 150, (list.Count() * 13) + 13);
            foreach (var item in list)
            {
                if (item.PropertyValue.StartsWith(pName))
                {
                    _autoCompleteListBox.Items.Add(item.PropertyValue);
                }
            }
            if (_autoCompleteListBox.Items.Count == 0)
                _autoCompleteListBox.Hide();
            else
            {
                _autoCompleteListBox.SelectedIndex = -1;
                _autoCompleteListBox.Show();
                _autoCompleteListBox.Parent = codeRichBox;
            }
        }
        private void PopulateTextBoxFroCss(string startingString)
        {
            var list = _cSSElements.Where(p => p.PropertyName.StartsWith(startingString));
            GetCaretPos(out _cp);
            _autoCompleteListBox.SetBounds(_cp.X, _cp.Y + 15, 150, (list.Count() * 13) + 13);
            foreach (var item in list)
            {
                if (item.PropertyName.StartsWith(startingString))
                {
                    _autoCompleteListBox.Items.Add(item.PropertyName);
                }
            }
            if (_autoCompleteListBox.Items.Count == 0)
                _autoCompleteListBox.Hide();
            else
            {
                _autoCompleteListBox.SelectedIndex = -1;
                _autoCompleteListBox.Show();
                _autoCompleteListBox.Parent = codeRichBox;
            }
        }
        private void PopulateTextBoxFroHtml(string startingString)
        {
            var list = _tagsColletion.Where(p => p.OpeningTag.StartsWith(startingString) || p.ClosingTag.StartsWith(startingString));

            GetCaretPos(out _cp);
            _autoCompleteListBox.SetBounds(_cp.X, _cp.Y + 15, 150, (list.Count() * 13) + 13);

            foreach (var item in list)
            {
                if (item.OpeningTag.StartsWith(startingString))
                    _autoCompleteListBox.Items.Add(item.OpeningTag);
                if (item.ClosingTag.StartsWith(startingString))
                    _autoCompleteListBox.Items.Add(item.ClosingTag);

            }

            if (_autoCompleteListBox.Items.Count == 0)
                _autoCompleteListBox.Hide();
            else
            {
                _autoCompleteListBox.SelectedIndex = -1;
                _autoCompleteListBox.Show();
                _autoCompleteListBox.Parent = codeRichBox;
            }

        }
        private void CodeRichBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && _autoCompleteListBox.Visible)
            {
                _autoCompleteListBox.Focus();
                _autoCompleteListBox.SelectedIndex = 0;
            }

            if (e.KeyCode == Keys.Escape)
                _autoCompleteListBox.Hide();


            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Enter) _candidateForAutoCorrect = false;
            else _candidateForAutoCorrect = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Hide();
            BringToFront();
        }
        private void liveViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (liveViewCheckBox.Checked == true)
            {
                webBrowser1.Show();
            }
            else
            {
                webBrowser1.Hide();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (codeRichBox.Text != "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, codeRichBox.Text);
                    webBrowser1.DocumentText = codeRichBox.Text;
                }
            }
            else
                MessageBox.Show(@"Write SomeThing in the Compiler");
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = @"HTML files (*.html)|*.html",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string a = openFileDialog.ToString();
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            codeRichBox.Text = File.ReadAllText(openFileDialog.FileName);
                            saveFileDialog1.FileName = openFileDialog.FileName;
                            //richTextBox1.SaveFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                            webBrowser1.DocumentText = codeRichBox.Text;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        private void chromeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Create("temp.html").Close();
                var file = new FileStream("temp.html", FileMode.Open);
                file.Write(Encoding.ASCII.GetBytes(codeRichBox.Text), 0, codeRichBox.TextLength);
                file.Flush();
                file.Close();
                //File.WriteAllText(saveFileDialog1.FileName, codeRichBox.Text);
                Process.Start("chrome", file.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occur " + ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int index = 0;
            string temp = codeRichBox.Text;
            codeRichBox.Text = "";
            codeRichBox.Text = temp;

            while (index < codeRichBox.Text.LastIndexOf(textBox1.Text))
            {
                codeRichBox.Find(textBox1.Text, index, codeRichBox.TextLength, RichTextBoxFinds.None);
                codeRichBox.SelectionBackColor = Color.Yellow;
                index = codeRichBox.Text.IndexOf(textBox1.Text, index) + 1;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            codeRichBox.SelectionStart = 0;
            codeRichBox.SelectAll();
            codeRichBox.SelectionBackColor = Color.FromArgb(64, 64, 64);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            webBrowser1.Update();
        }
        public List<string> lstoftag;
        private void button4_Click(object sender, EventArgs e)
        {
            bool boolcheck = true;
            bool milgya = false;
            string[] data = codeRichBox.Text.Split('\n');
            lstoftag = new List<string>();
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

                            if (tag[1] == '/') //closing tag 
                            {
                                int incr = 0;
                                List<string> lstem = new List<string>();
                                milgya = false;

                                while (_tagStack.Count > 0)
                                {
                                    incr++;
                                    milgya = false;
                                    if (_tagStack.Peek().Length == tag.Length - 1)
                                    {
                                        milgya = true;
                                        string check = _tagStack.Peek();

                                        for (int l = 2; l < tag.Length - 1; l++)
                                        {
                                            if (check[l - 1] != tag[l])
                                            {
                                                lstoferrors.Add("Opening Tag Is Missing For Line :" + k);
                                                check = null;
                                                break;
                                            }
                                        }

                                        if (check != null)
                                            _tagStack.Pop();

                                        foreach (var t in lstem)
                                            _tagStack.Push(t);

                                        break;
                                    }
                                    else
                                    {
                                        lstem.Add(_tagStack.Pop());
                                        milgya = true;
                                    }
                                }

                                if (!milgya && incr != 0)
                                    lstoferrors.Add("Opening Tag Is Missing For Line :" + k);
                            }

                            else
                                _tagStack.Push(tag);

                            lstoftag.Add(tag);

                            opening = false;
                        }
                        else if (data[k][i] == '<')
                        {
                            lstoferrors.Add("Closing Braces Is Missing In Line  :" + k + "\tIndex No :" +
                                            priviousopening);

                            priviousopening = i;
                            int firstcharindex = codeRichBox.GetFirstCharIndexOfCurrentLine();
                            int currentline = codeRichBox.GetLineFromCharIndex(firstcharindex);
                            string currentlinetext = codeRichBox.Lines[currentline];

                            codeRichBox.Select(firstcharindex, currentlinetext.Length);
                            codeRichBox.SelectionBackColor = Color.Red;
                            milgya = true;
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
                        lstoferrors.Add(
                            "Opening Braces Is Missing In Line :" + k + "\tIndex No :" + priviousopening);
                        // tagstack.Push(" ");
                        int firstcharindex = codeRichBox.GetFirstCharIndexOfCurrentLine();
                        int currentline = codeRichBox.GetLineFromCharIndex(firstcharindex);
                        string currentlinetext = codeRichBox.Lines[currentline];

                        codeRichBox.Select(firstcharindex, currentlinetext.Length);
                        codeRichBox.SelectionBackColor = Color.Red;
                    }
                }

                if (opening)
                {
                    opening = false;
                    lstoferrors.Add("Opening/Closing Braces Is Missing In Line  :" + k + "\tIndex No :" +
                                    priviousopening);
                    // tagstack.Push(" ");
                }
            }

            if (lstoferrors.Count != 0)
            {
                boolcheck = false;

                for (int i = 0; i < lstoferrors.Count; i++) //1 
                    MessageBox.Show(lstoferrors[i].ToString());
            }
            else
            {
                int firstcharindex = codeRichBox.GetFirstCharIndexOfCurrentLine();
                int currentline = codeRichBox.GetLineFromCharIndex(firstcharindex);
                string currentlinetext = codeRichBox.Lines[currentline];

                codeRichBox.Select(firstcharindex, currentlinetext.Length);
                codeRichBox.SelectionBackColor = Color.FromArgb(64, 64, 64);

            }

            string filetext = File.ReadAllText("hardcode.txt");
            while (_tagStack.Count > 0)
            {
                if (filetext.Contains(_tagStack.Peek()))
                {
                    _tagStack.Pop();
                }
                else
                {
                    boolcheck = false;
                    MessageBox.Show(@"Closing Tag Not Found For This Tag : " + (_tagStack.Pop()));
                    int firstcharindex = codeRichBox.GetFirstCharIndexOfCurrentLine();
                    int currentline = codeRichBox.GetLineFromCharIndex(firstcharindex);
                    string currentlinetext = codeRichBox.Lines[currentline];
                    codeRichBox.Select(firstcharindex, currentlinetext.Length);
                    codeRichBox.SelectionBackColor = Color.Red;
                }
            }

            if (boolcheck)
                MessageBox.Show(@"No Error Found");

            string a = null;
            string d = null;
            string[] b = new string[lstoftag.Count()];
            for (int i = 0; i < lstoftag.Count(); i++)
            {
                a += (lstoftag[i].ToString() + "\n");
                b[i] = lstoftag[i].ToString();
            }

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = b[i].Replace("<", string.Empty);
                b[i] = b[i].Replace(">", string.Empty);
                b[i] = b[i].Replace(">", string.Empty);
                b[i] = b[i].Replace("/", string.Empty);
            }

            var output = b.GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .ToList();
            a = a.Replace("\n", Environment.NewLine);
            for (int i = 0; i < output.Count(); i++)
            {
                d += (output[i].ToString() + "\n");
            }
            MessageBox.Show(@"Following Keyword used in your HTML " + "\n" + d);
            MessageBox.Show(@"Following Tag Used in Your HTML Parser " + "\n" + a.ToString());
            MessageBox.Show(@"This is Your content(value) :" + "\n" + webBrowser1.Document.Body.InnerText);
        }
    }
}