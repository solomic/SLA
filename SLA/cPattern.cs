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
        
        //StpExec	Create	4	0000450a5ded113c:0	2019-12-09 08:08:16	Реализация определения шага (Log On?).
        //task
        static public readonly string pTaskStep = @"^(TskNav)\t(Oper).*Ядро задач запрошено для перехода к следующему шагу";
        static public readonly string pTaskStepParse = @"^(TskNav)\t(Oper)\t(\d{1})\t([\d|\w]{16}\:\d{1})\t(\d{4}\-\d{2}\-\d{2})(\s*)(\d{2}\:\d{2}\:\d{2})\t(Ядро задач запрошено для перехода к следующему шагу:)\s*(.*).$";
        //TskNav  Oper    3	0000450a5ded113c:0	2019-12-09 08:08:06	Ядро задач запрошено для перехода к следующему шагу: Task View 0.


        //PrcExec	Create	4	0000450a5ded113c:0	2019-12-09 08:08:06	Реализация определения процесса JET RB Client Identification Task.
        //TskNav NavPath	4	0000450a5ded113c:0	2019-12-09 08:08:06	Добавление шага Start Hrono к пути отстающего стека.

        //StpExec	Task	4	0000450a5ded113c:0	2019-12-09 08:08:06	Переход к представлению задачи: JET RB Client Identification Search Task View.
        //EngInv	EngInv	3	0000450a5ded113c:0	2019-12-09 08:08:16	Для выполнения метода RunProcess запрошено ядро потоков операций.
    }
}
