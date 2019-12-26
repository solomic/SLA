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
                PatternLike     = LoadPattern("Pattern.txt");
                PatternExcept   = LoadPattern("Except.txt");
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
        List<FileLine> LoadPattern(string filename)
        {
            List<FileLine> fl = new List<FileLine>();
            //заглушка
            if (filename== "Pattern.txt")
            {
                fl.Add(new FileLine("Error", @"^(.*)\t(Error)\t(1)"));
                fl.Add(new FileLine("Error_Adpt", @"^(.*)\t(EAISiebAdptErr)\t(1)"));

                fl.Add(new FileLine("StpExec_Task", @"^(StpExec)\t(Task)"));
                fl.Add(new FileLine("StpExec_End", @"^(StpExec)\t(End)"));
                fl.Add(new FileLine("StpExec_TaskArg", @"^(StpExec)\t(TaskArg)"));
                fl.Add(new FileLine("StpExec_Create", @"^(StpExec)\t(Create)"));
                fl.Add(new FileLine("StpExec_Cond", @"^(StpExec)\t(Cond)"));
                fl.Add(new FileLine("EngInv_EngInv", @"^(EngInv)\t(EngInv)"));
                fl.Add(new FileLine("EngInv_Arg", @"^(EngInv)\t(Arg)"));
                fl.Add(new FileLine("PrcExec_PrcExec", @"^(PrcExec)\t(PrcExec)"));                
                fl.Add(new FileLine("TskNav_NavPath", @"^(TskNav)\t(NavPath)"));
                fl.Add(new FileLine("TskNav_Oper", @"^(TskNav)\t(Oper)"));
                fl.Add(new FileLine("PrcExec_Create", @"^(PrcExec)\t(Create)"));
                fl.Add(new FileLine("PrcExec_PropSet", @"^(PrcExec)\t(PropSet)"));                
                //^(EngInv)\t(Arg)(.|\n)*?\n\n
                //fl.Add(new FileLine("WFProcess", @"^(.*)\t(Create)(.*)(Реализация определения процесса)(.*)"));
                fl.Add(new FileLine("WFStep", @"^(.*)\t(Create)(.*)(Реализация определения шага)(.*)"));
                //fl.Add(new FileLine("TaskStep", @"^(TskNav)\t(Oper).*Ядро задач запрошено для перехода к следующему шагу"));                
            }
            if (filename == "Except.txt")
            {
                fl.Add(new FileLine("Except", @"^(.*)(model\.cpp\s\(10057\)\))\s(SBL-DAT-00222)"));
                
            }
            return fl;
        }
        bool Parse(string line)
        {
            bool ret = false;
            try
            {                
                if (line == "")
                    return false;
                //проверяем на исключения
                foreach (var item in PatternExcept)
                {
                    if (Regex.IsMatch(line, item.Line, RegexOptions.IgnoreCase))
                    {
                        return false;
                    }
                }
                foreach (var item in PatternLike)
                {
                    if (Regex.IsMatch(line, item.Line, RegexOptions.IgnoreCase))
                    {
                        return true;
                       // ParseLineValue(item.Type, line);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
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
