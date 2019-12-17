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
        LogFile logFile;
        public fMain()
        {
            InitializeComponent();
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            openFile();
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
                    logFile = new LogFile(openFileDialog.FileName);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Task.Run(() => analyzeFile(fileName));
            Task task1 = new Task(() => logFile.Analyze(), TaskCreationOptions.AttachedToParent);
            task1.Start();
            
            //task1.Wait();
            MessageBox.Show(logFile.ProgressState);
            
            
            //analyzeFile(fileName);

        }
     

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(logFile.getPosition().ToString()+" "+ logFile.ProgressState);
        }
    }
}
