using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLA
{
    static class cPattern
    {
        //Ошибка Error 1            
        static public readonly string pError = @"^(.*)\t(Error)\t(1)";
        static public readonly string pErrorParse = @"^(.*)\t(Error)\t(1)\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})\s*(\d{2}\:\d{2}\:\d{2})\t\(.*\)\s*(.*)\:(.*)(.*)";

        //WF
        static public readonly string pWFProcess = @"^(.*)\t(Create).*Реализация определения процесса";
        static public readonly string pWFProcessParse = @"^(.*)\t(.*)\t(\d{1})\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})(\s*)(\d{2}\:\d{2}\:\d{2})\t(Реализация определения процесса)\s*([A-Za-z\s\-]*)";

    }
}
