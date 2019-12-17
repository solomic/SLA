using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLA
{
    
    class LineBuffer
    {
        long LineNum;
        string Buffer;
        int Level;
        int ParentLevel;
        string Type; //WF step, Task step, Error,BS start,BS end
        string Name;
        DateTime? dt;
        string ErrorCode;
        string ErrorMessage;
        public LineBuffer(long cLineNum, string str, int iLevel, int iParentLevel, string sType,string sName,DateTime? dDt, string sErrorCode,string sErrorMessage)
        {
            LineNum = cLineNum;
            Buffer = str;
            Level = iLevel;
            ParentLevel = iParentLevel;
            Type = sType;
            Name = sName;
            dt = dDt;
            ErrorCode = sErrorCode;
            ErrorMessage = sErrorMessage;
        }

       

    }
}
