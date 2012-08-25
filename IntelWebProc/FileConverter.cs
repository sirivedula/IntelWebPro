using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace IntelWebProc
{
    public class FileConverter
    {
        private string _lasterror = "";
        public string LastError
        {
            get
            {
                return _lasterror;
            }
        }

        public string SrcFile
        {
            get;
            set;
        }

        public bool ConvertXLSToText()
        {
            bool result = true;
            Excel.Application oe = new Excel.Application();
            oe.Visible = false;
            oe.EnableEvents = false; //critical to prevent macros from running
            oe.DisplayAlerts = false; //prevents any ui from occuring
            Excel.Workbook wb = oe.Workbooks.Open(SrcFile, 0, false, Missing.Value, "", "", true, Missing.Value, "", false, false, 0, false);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            if (ws.Name.ToLower() == "cover sheet" || ws.Name.ToLower() == "cover page")
            {
                if (wb.Worksheets.Count > 1)
                {
                    ws = (Excel.Worksheet)wb.Worksheets[2];
                }
            }
            Excel.Range r = ws.UsedRange;
            Array tarray = (Array)r.Value;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(r);

            try
            {
                int rows = tarray.GetUpperBound(0);
                int columns = tarray.GetUpperBound(1);
                bool lineBlank;
                string tempValue;
                string Line;
                StringBuilder Lines = new StringBuilder(1);
                for (int rc = 0; rc < rows; rc++)
                {
                    lineBlank = true;
                    Line = "";
                    for (int c = 0; c < columns; c++)
                    {
                        try
                        {
                            tempValue = tarray.GetValue(rc + 1, c + 1).ToString();
                        }
                        catch (Exception)
                        {
                            tempValue = "";
                        }
                        Line += tempValue + (((c + 1) == columns) ? "" : "\t");
                        if (tempValue.Trim() != "")
                        {
                            lineBlank = false;
                        }
                    }
                    if (!lineBlank)
                    {
                        Lines.Append(Line + "\r\n");
                    }
                }
                wb.Close(false, Missing.Value, Missing.Value);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                oe.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oe);
                result = WriteToDestFile(Lines);
            }
            catch (Exception ex)
            {
                result = false;
                _lasterror = ex.Message.Split('\n')[0];
            }
            return result;
        }

        private string _destfilepath;
        public string DestFile
        {
            get { return _destfilepath; }
            set { _destfilepath = value; }
        }

        private string _filetext;
        public string fileText
        {
            get
            {
                return _filetext;
            }
        }

        private bool WriteToDestFile(StringBuilder lines)
        {
            _filetext = lines.ToString();
            if (string.IsNullOrEmpty(_destfilepath))
            {
                return true;
            }
            bool result = true;
            try
            {
                StreamWriter sw = File.CreateText(DestFile);
                sw.Write(lines.ToString());
                sw.Close();
            }
            catch (Exception ex)
            {
                _lasterror = ex.Message.Split('\n')[0];
                result = false;
            }
            return result;
        }
    }
}
