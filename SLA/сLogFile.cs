using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLA
{
    class сLogFile
    {
        String filePath;
        String fileName;
        int counter = 0;
        string line;
        List<SearchBuffer> SB;        
        public string ProgressState;
        public long position;
        long FileSize;


        public сLogFile(string infilePath)
        {
            filePath = infilePath;
            FileInfo info = new FileInfo(filePath);            
            FileSize = info.Length;
            SB = new List<SearchBuffer>();
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
                        //Console.WriteLine("Read line: " + counter.ToString());
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
                                SB.Add(new SearchBuffer(counter, line, 0, 0, "WF Process", m.Groups[9].Value, dt, null, null));
                                // sLineResult.Items.Add("Line " + counter + ":\t" + line + "\n");
                            }
                            continue;
                        }



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
