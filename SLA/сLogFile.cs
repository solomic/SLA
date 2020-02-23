using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLA
{
   
    class LogFile
    {
        String filePath;
        String fileName;
        int counter = 0;
        string line;
        List<LineBuffer> SB;      

        public string ProgressState;
        public long position;
        long FileSize;

        List<FileLine> PatternLike;
        List<FileLine> PatternExcept;
        Dictionary<string, string> PatternParse;

        struct FileLine
        {
            public string Type;
            public string Line;
            public FileLine(string type,string line)
            {
                Type = type;
                Line = line;
            }

        }

        public LogFile(string infilePath)
        {
            
            try
            {
                filePath        = infilePath;
                FileInfo info   = new FileInfo(filePath);
                FileSize        = info.Length;
                SB              = new List<LineBuffer>();               
                PatternParse    = LoadPatternParse("Parse.txt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Dictionary<string, string> LoadPatternParse(string filename)
        {
            Dictionary<string, string> ps = new Dictionary<string, string>();
            ps.Add("Error", @"^(.*)\t(Error)\t(1)\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})\s*(\d{2}\:\d{2}\:\d{2})\t\(.*\)\s*(.*)\:(.*)(.*)");
            ps.Add("WFProcess", @"^(.*)\t(.*)\t(\d{1})\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})(\s*)(\d{2}\:\d{2}\:\d{2})\t(Реализация определения процесса)\s*([A-Za-z\s\-]*)");
            ps.Add("WFStep", @"^(.*)\t(.*)\t(\d{1})\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})(\s*)(\d{2}\:\d{2}\:\d{2})\t(Реализация определения шага)\s*([A-Za-z\s\-]*)");
            ps.Add("TaskStep", @"^(TskNav)\t(Oper)\t(\d{1})\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})(\s*)(\d{2}\:\d{2}\:\d{2})\t(Ядро задач запрошено для перехода к следующему шагу:)\s*(.*).$");
            return ps;
        }
        
        
        void ParseLineValue(string type,string line)
        {
            if (type=="Error")
            {
                Match m = Regex.Match(line, PatternParse[type], RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    DateTime dt;
                    DateTime.TryParse(m.Groups[5].Value + " " + m.Groups[6].Value, out dt);
                    SB.Add(new LineBuffer(counter, line, 0, 0, "Error", "Error", dt, m.Groups[7].Value, m.Groups[8].Value));

                }
                return;
            }
            if (type == "WFProcess")
            {
                Match m = Regex.Match(line, PatternParse[type], RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    DateTime dt;
                    DateTime.TryParse(m.Groups[5].Value + " " + m.Groups[7].Value, out dt);
                    SB.Add(new LineBuffer(counter, line, 0, 0, "WF Process", m.Groups[9].Value, dt, null, null));

                }
                return;
            }
        }


        public byte getPosition()
        {
            return  Convert.ToByte(position*100/ FileSize);
        }
        public void Analyze()
        {
            
            try
            {
                ProgressState = "Analyzing";
                Console.WriteLine("Begin analyze: " + DateTime.Now.ToString());
                fileName = Path.GetFileName(filePath);
                var FileWrite=Path.GetDirectoryName(filePath)+@"\" + Path.GetFileNameWithoutExtension(filePath) + "_lite.txt";
               
                //Read the contents of the file into a stream
                var fileStream = File.OpenRead(filePath);
                var fileStreamLite = File.OpenWrite(FileWrite);
                
                StreamWriter writer = new StreamWriter(fileStreamLite);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    var AllLine = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        counter++;
                        position = reader.BaseStream.Position;
                        if (line != "")
                        {
                            AllLine += (line+Environment.NewLine);
                            continue;
                        }
                        
                        if (Parse(AllLine))
                        {
                            writer.WriteLine(AllLine);
                        }
                        AllLine = "";
                    }
                    writer.Flush();
                    writer.Close();
                }
                
                ProgressState = "Success";
                Console.WriteLine("End analyze: " + DateTime.Now.ToString());
                Mess.MessageSuccess();
                
            }
            catch (Exception err)
            {
                ProgressState = "Error";
                throw err;
            }
       
           
        }


    }
}
