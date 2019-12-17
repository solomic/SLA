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
        List<SearchBuffer> SB;      

        public string ProgressState;
        public long position;
        long FileSize;

        string[] PatternLike;
        string[] PatternExcept;

        public LogFile(string infilePath)
        {
            
            try
            {
                filePath        = infilePath;
                FileInfo info   = new FileInfo(filePath);
                FileSize        = info.Length;
                SB              = new List<SearchBuffer>();
                PatternLike     = File.ReadAllLines("Pattern.txt");
                PatternExcept   = File.ReadAllLines("Except.txt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Parse(string line)
        {
            try
            {
                if (line == "")
                    return;
                //проверяем на исключения
                foreach (var item in PatternExcept)
                {
                    if (Regex.IsMatch(line, item, RegexOptions.IgnoreCase))
                    {
                        return;
                    }
                }
                foreach (var item in PatternLike)
                {
                    if (Regex.IsMatch(line, item, RegexOptions.IgnoreCase))
                    {
                        return;
                        if (Regex.IsMatch(line, cPattern.pError, RegexOptions.IgnoreCase))
                        {
                            Match m = Regex.Match(line, cPattern.pErrorParse, RegexOptions.IgnoreCase);
                            if (m.Success)
                            {
                                DateTime dt;
                                DateTime.TryParse(m.Groups[5].Value + " " + m.Groups[6].Value, out dt);
                                SB.Add(new SearchBuffer(counter, line, 0, 0, "Error", "Error", dt, m.Groups[7].Value, m.Groups[8].Value));

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
                                SB.Add(new SearchBuffer(counter, line, 0, 0, "WF Process", m.Groups[9].Value, dt, null, null));

                            }
                            continue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
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
               
                //Read the contents of the file into a stream
                var fileStream = File.OpenRead(filePath);                

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        counter++;
                        position = reader.BaseStream.Position;
                        Parse(line);
                    }
                }
                ProgressState = "Success";

            }
            catch (Exception err)
            {
                ProgressState = "Error";
                throw err;
            }
       
           
        }


    }
}
