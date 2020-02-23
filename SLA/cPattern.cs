using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLA
{
    static class Mess
    {
        public static void MessageSuccess()
        {
            MessageBox.Show("ОК");
        }
        public static void Progress()
        {
            MessageBox.Show("ОК");
        }
    }

    class Pattern
    {
        public long Lvl;
        public string Type;
        public string Ptrn;
        public string Prs;
        public bool Excpt;

        public Pattern(long Lvl, string Type, string Ptrn, string Prs, bool Excpt)
        {
            this.Lvl = Lvl;
            this.Type = Type;
            this.Ptrn = Ptrn;
            this.Prs = Prs;
            this.Excpt = Excpt;
        }

           
    }
    static class PatternList
    {
        public static List<Pattern> Pat;
      
        static PatternList()
        {
            List<Pattern> Pat = new List<Pattern>();
            Pat.Add(new Pattern(1, "Except", @"^(.*)(model\.cpp\s\(10057\)\))\s(SBL-DAT-00222)", "", true));

            Pat.Add(new Pattern(1, "Error", @"^(.*)\t(Error)\t(1)","",false));
            Pat.Add(new Pattern(1, "Error_Adpt", @"^(.*)\t(EAISiebAdptErr)\t(1)", "", false));

            Pat.Add(new Pattern(1, "StpExec_Task", @"^(StpExec)\t(Task)", "", false));
            Pat.Add(new Pattern(1, "StpExec_End", @"^(StpExec)\t(End)", "", false));
            Pat.Add(new Pattern(1, "StpExec_TaskArg", @"^(StpExec)\t(TaskArg)", "", false));
            Pat.Add(new Pattern(1, "StpExec_Create", @"^(StpExec)\t(Create)", "", false));
            Pat.Add(new Pattern(1, "StpExec_Cond", @"^(StpExec)\t(Cond)", "", false));
            Pat.Add(new Pattern(1, "EngInv_EngInv", @"^(EngInv)\t(EngInv)", "", false));
            Pat.Add(new Pattern(1, "EngInv_Arg", @"^(EngInv)\t(Arg)", "", false));
            Pat.Add(new Pattern(1, "PrcExec_PrcExec", @"^(PrcExec)\t(PrcExec)", "", false));
            Pat.Add(new Pattern(1, "TskNav_NavPath", @"^(TskNav)\t(NavPath)", "", false));
            Pat.Add(new Pattern(1, "TskNav_Oper", @"^(TskNav)\t(Oper)", "", false));
            Pat.Add(new Pattern(1, "PrcExec_Create", @"^(PrcExec)\t(Create)", "", false));
            Pat.Add(new Pattern(1, "PrcExec_PropSet", @"^(PrcExec)\t(PropSet)", "", false));

            

            //^(EngInv)\t(Arg)(.|\n)*?\n\n
            //fl.Add(new FileLine("WFProcess", @"^(.*)\t(Create)(.*)(Реализация определения процесса)(.*)"));
            //fl.Add(new FileLine("WFStep", @"^(.*)\t(Create)(.*)(Реализация определения шага)(.*)"));
            //fl.Add(new FileLine("TaskStep", @"^(TskNav)\t(Oper).*Ядро задач запрошено для перехода к следующему шагу"));   
        }
        static bool Parse(string line)
        {
            bool Ret = false;
            try
            {
                if (line == "")
                    return false;
                //проверяем на исключения
                //foreach (var item in PatternExcept)
                //{
                //    if (Regex.IsMatch(line, item.Line, RegexOptions.IgnoreCase))
                //    {
                //        return false;
                //    }
                //}
                foreach (var item in Pat)
                {
                    if (Regex.IsMatch(line, item.Ptrn, RegexOptions.IgnoreCase))
                    {
                        //отправляем строку на детальный разбор
                        Ret = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ret;
           
        }
    }

}
