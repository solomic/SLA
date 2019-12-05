using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLA
{
    public partial class fMain : Form
    {
        string fileContent = string.Empty;
        string filePath = string.Empty;
        string fileName = string.Empty;

        List<SearchBuffer> SB;

        public fMain()
        {
            InitializeComponent();
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            openFile();
        }

        public int analyzeFile(string filepath)
        {
            Console.WriteLine("Begin analyze: " + DateTime.Now.ToString());
            SB = new List<SearchBuffer>();
            int counter = 0;
            string line;
            //Read the contents of the file into a stream
            var fileStream = File.OpenRead(filepath);

            using (StreamReader reader = new StreamReader(fileStream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    counter++;
                    Console.WriteLine("Read line: " + counter.ToString());
                    if (Regex.IsMatch(line, cPattern.pError, RegexOptions.IgnoreCase))
                    {
                        Match m = Regex.Match(line, cPattern.pErrorParse, RegexOptions.IgnoreCase);
                        if (m.Success)
                        {
                            DateTime dt;
                            DateTime.TryParse(m.Groups[5].Value + " " + m.Groups[6].Value, out dt);
                            SB.Add(new SearchBuffer(counter, line, 0, 0, "Error", "Error", dt, m.Groups[7].Value, m.Groups[8].Value));
                           // sLineResult.Items.Add("Line " + counter + ":\t" + line + "\n");
                        }                        
                        continue;
                    }
                    if (Regex.IsMatch(line, cPattern.pWFProcess, RegexOptions.IgnoreCase))
                    {
                        Match m = Regex.Match(line, cPattern.pWFProcessParse, RegexOptions.IgnoreCase);
                        if (m.Success)
                        {
                            DateTime dt;
                            DateTime.TryParse(m.Groups[5].Value + " " + m.Groups[7].Value, out dt);
                            SB.Add(new SearchBuffer(counter, line, 0, 0, "WF Process", m.Groups[9].Value, dt,null,null));
                           // sLineResult.Items.Add("Line " + counter + ":\t" + line + "\n");
                        }                        
                        continue;
                    }



                }
            }
            Console.WriteLine("End analyze: " + DateTime.Now.ToString());
            return 0;
        }

        private void openFile()
        {
            
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files(*.*)|*.*|SQL files (*.sql)|*.sql|txt files(*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //tabConrol.TabPages.Add(openFileDialog.FileName);
                    //tabConrol.SelectedTab.Tag = openFileDialog.FileName;
                    fileName = openFileDialog.FileName;
                    
                    //FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    //FileViewer viewer = new FileViewer(stream);
                    //viewer.Parent = panel1;
                    //viewer.Dock = DockStyle.Fill;
                    //RichTextBox rtb = new RichTextBox();
                    //tabConrol.SelectedTab.Controls.Add(rtb);                    
                    //rtb.Anchor = (AnchorStyles.Top| AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left);
                    //rtb.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);

                    ////Get the path of specified file
                    //filePath = openFileDialog.FileName;

                    //int counter = 0;
                    //string line;
                    ////Read the contents of the file into a stream
                    //var fileStream = openFileDialog.OpenFile();

                    //using (StreamReader reader = new StreamReader(fileStream))
                    //{
                    //    while ((line = reader.ReadLine()) != null)
                    //    {
                    //        //System.Console.WriteLine(line);
                    //        counter++;
                    //        if (Regex.IsMatch(line,cPattern.pError1,RegexOptions.IgnoreCase))
                    //        {
                    //            // System.Console.WriteLine(line);
                    //            sLineResult.Items.Add("Line "+counter+":\t"+line + "\n");
                    //         }
                    //    }

                    //}



                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           // Task.Run(() => analyzeFile(fileName));
            Task<int> task1 = new Task<int>(() => analyzeFile(fileName), TaskCreationOptions.AttachedToParent);            
            task1.Start();

            //analyzeFile(fileName);

        }
     

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            
        }
    }
}
