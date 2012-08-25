using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IntelWeb;
using System.Reflection;
using IntelWeb.IntelWebObjects;
using System.Web;

namespace IntelWebImporter
{
    public class ImportRun
    {
        private List<ImportError> _importproblems = new List<ImportError>();
        private List<ImportedItem> _importeditems = new List<ImportedItem>();

        public ImportRun()
        {
            _startdate = DateTime.Now;
        }

        public string fullFileName { get; set; }
        public string UserName { get; set; }
        public string tierCode { get; set; }
        private ImportError error = new ImportError();
        private ImportedItem iitem;
        private int _line;
        private DateTime _startdate;
        private DateTime _enddate;

        private int dataLines
        {
            get
            {
                return _line;
            }
        }

        public void doImport(bool preview)
        {
            if (!File.Exists(fullFileName))
            {
                error = new ImportError();
                error.errorMessage = fullFileName + " does not exist.";
                _importproblems.Add(error);
                return;
            }
            CurrentUser cuser = new CurrentUser();
            cuser.userName = this.UserName;
            cuser.Load();
            using (StreamReader sr = File.OpenText(fullFileName))
            {
                String inputLine;
                /* Read Header */
                inputLine = sr.ReadLine();
                if (inputLine != null)
                {
                    Dictionary<string, string> headers = this.deviceHeaders;
                    string[] lineHeaders = inputLine.Split('\t');
                    int col = 0;
                    foreach (string str in lineHeaders)
                    {
                        if (headers.ContainsKey(str))
                        {
                            headers[str] = col.ToString();
                            col++;
                        }
                    }
                    Dictionary<string, string> mapFields = this.fieldsMap;
                    string deptcode="", sitecode="", panelcode="", alarmcode="", alarmtype="";
                    _line = 0;
                    while ((inputLine = sr.ReadLine()) != null)
                    {
                        /* Read File Each Line */
                        string[] lineFields = inputLine.Split('\t');
                        _line++;
                        tbl_alarm_time_details alarmDet = new tbl_alarm_time_details(cuser);
                        foreach(string fieldname in headers.Keys)
                        {

                            int idx;
                            int.TryParse(headers[fieldname], out idx);
                            object value = lineFields[idx];

                            PropertyInfo pi = alarmDet.GetType().GetProperty(mapFields[fieldname]);
                            if (pi != null)
                            {
                                value = IntelWebDataConverter.toType(lineFields[idx], pi.PropertyType, "Line: [" + _line.ToString() + "] Field: [" + fieldname + "] Value: [" + (lineFields[idx] ?? "") + "]");

                                pi.SetValue(alarmDet, value, null);

                                if (fieldname.Equals("DEPT/REG"))
                                {
                                    deptcode = value.ToString();
                                }

                                if (fieldname.Equals("SITE"))
                                {
                                    sitecode = value.ToString();
                                }

                                if (fieldname.Equals("PANEL"))
                                {
                                    panelcode = value.ToString();
                                }

                                if (fieldname.Equals("ALARM DEVICE"))
                                {
                                    alarmcode = value.ToString();
                                }
                                if (fieldname.Equals("ALARM TYPE"))
                                {
                                    alarmtype = value.ToString();
                                }
                            }
                        }
                        UpSert(deptcode, sitecode, panelcode, alarmcode, alarmtype, cuser);
                        alarmDet.tier_code = tierCode;
                        alarmDet.LoadSingle();
                        bool isnew = alarmDet.isNew;
                        string desc = alarmDet.ChangedFieldsDescription();
                        if (!alarmDet.save())
                        {
                            error = new ImportError();
                            error.errorMessage = alarmDet.saveErrorText;
                            error.fieldFQR = alarmDet.friendlySingluarName();
                            error.lineNumber = _line;
                            _importproblems.Add(error);
                        }
                        else
                        {
                            iitem = new ImportedItem(_line, isnew, alarmDet.friendlySingluarName(), desc);
                            _importeditems.Add(iitem);
                        }
                    }
                }
            }
            _enddate = DateTime.Now;
        }

        private void UpSert(string Dept, string Site, string Panel, string Alarm, string AlarmType, CurrentUser cuser)
        {
            /* Find If Department Exists or Insert it */
            lkup_department dept = new lkup_department(cuser);
            dept.department_code = Dept;
            dept.LoadSingle();
            if (dept.isNew)
            {
                dept.department_code_description = Dept;
                string desc = dept.ChangedFieldsDescription();
                if (!dept.save())
                {
                    error = new ImportError();
                    error.errorMessage = dept.saveErrorText;
                    error.fieldFQR = dept.friendlySingluarName();
                    error.lineNumber = _line;
                    _importproblems.Add(error);
                }
                else
                {
                    iitem = new ImportedItem(_line, true, dept.friendlySingluarName(), desc);
                    _importeditems.Add(iitem);
                }
            }

            lkup_site site = new lkup_site(cuser);
            site.department_code = Dept;
            site.site_code = Site;
            site.LoadSingle();
            if (site.isNew)
            {
                site.site_code_description = Site;
                string desc = site.ChangedFieldsDescription();
                if (!site.save())
                {
                    error = new ImportError();
                    error.errorMessage = site.saveErrorText;
                    error.fieldFQR = site.friendlySingluarName();
                    error.lineNumber = _line;
                    _importproblems.Add(error);
                }
                else
                {
                    iitem = new ImportedItem(_line, true, site.friendlySingluarName(), desc);
                    _importeditems.Add(iitem);
                }
            }

            lkup_panel panel = new lkup_panel(cuser);
            panel.panel_code = Panel;
            panel.site_code = Site;
            panel.LoadSingle();
            if (panel.isNew)
            {
                panel.alarm_device_code = Alarm;
                panel.panel_code_description = Panel;
                string desc = panel.ChangedFieldsDescription();
                if (!panel.save())
                {
                    error = new ImportError();
                    error.errorMessage = panel.saveErrorText;
                    error.fieldFQR = panel.friendlySingluarName();
                    error.lineNumber = _line;
                    _importproblems.Add(error);
                }
                else
                {
                    iitem = new ImportedItem(_line, true, panel.friendlySingluarName(), desc);
                    _importeditems.Add(iitem);
                }
            }

            lkup_alarm_device alarm = new lkup_alarm_device(cuser);
            alarm.panel_code = Panel;
            alarm.alarm_device_code = Alarm;
            alarm.alarm_type_code = AlarmType;
            alarm.LoadSingle();
            if (alarm.isNew)
            {                
                alarm.alarm_device_code_description = Alarm;
                string desc = alarm.ChangedFieldsDescription();
                if (!alarm.save())
                {
                    error = new ImportError();
                    error.errorMessage = alarm.saveErrorText;
                    error.fieldFQR = alarm.friendlySingluarName();
                    error.lineNumber = _line;
                    _importproblems.Add(error);
                }
                else
                {
                    iitem = new ImportedItem(_line, true, alarm.friendlySingluarName(), desc);
                    _importeditems.Add(iitem);
                }
            }
        }

        private Dictionary<string, string> deviceHeaders
        {
            get
            {
                Dictionary<String, String> Results = new Dictionary<string, string>();
                Results.Add("DATE", "1");
                Results.Add("HOUR","2");
                Results.Add("ALARM TIME","3");
                Results.Add("ALARM TYPE","4");
                Results.Add("ALARM DETAILS","5");
                Results.Add("ALARM DEVICE", "6");
                Results.Add("PANEL", "7");
                Results.Add("SITE", "8");
                Results.Add("DEPT/REG", "9");

                return Results;
            }
        }

        private Dictionary<string, string> fieldsMap
        {
            get
            {
                Dictionary<string, string> Result = new Dictionary<string, string>();
                Result.Add("DATE", "alarm_date");
                Result.Add("HOUR", "alarm_hour");
                Result.Add("ALARM TIME", "alarm_time");
                Result.Add("ALARM TYPE", "alarm_comment");
                Result.Add("ALARM DETAILS", "alarm_details");
                Result.Add("ALARM DEVICE", "alarm_device_code");
                Result.Add("PANEL", "panel_code");
                Result.Add("SITE", "site_code");
                Result.Add("DEPT/REG", "department_code");

                return Result;
            }
        }

        public string toHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<style>
.importresults table{border-collapse:collapse;border-left:1px dotted #CCC;border-right:1px dotted #CCC;border-top:1px dotted #CCC;empty-cells: show;margin-left:26px;}
.importresults table td table{border-right:0px;border-top:0px;}
.importresults table td table td{border-bottom:0px;}
.importresults td{vertical-align:top;border-bottom:1px dotted #CCC;padding-left:8px;padding-right:2px;border-left:1px dotted #EEE;}
.importresults th{vertical-align:top;border-bottom:1px dotted #CCC;text-align:left;background-color:#524F77;color:#FFF;padding-left:8px;padding-right:2px;border-left:1px solid #FFF;border-right:0px solid #CCC;}
.imptotal{text-align:right;}
.importresults h2{width:100%;border-bottom:1px solid #CCC;background-color:#646192;padding:5px;color:#FFF;font-family:Tahoma;font-size:12px;}
#impobjectssummary{border-collapse:collapse;border-spacing:0px;}
#impobjectssummary th{font-style:italic;background-color:#FFF;border-bottom:1px dotted #CCC;}
.importresults tr.Even{background-color:#E0DFEA;}
.importresults tr.Odd{background-color:#A4A2C1;}
.err {border:3px solid #F00;padding:12px;font-weight:bold;font-size:20px;}
#impresultsproperties th { background-color:#524F77; color:#FFF;}
</style>");

            sb.AppendLine("<div class=\"importresults\">");

            sb.Append("<div class=\"impsummary\"><h2>Import Summary</h2>");
            sb.Append("<table id=\"impresultsproperties\">");

            sb.Append("<tr><th>Started</th><td>" + HttpUtility.HtmlEncode(_startdate.ToString()) + "</td></tr>");
            sb.Append("<tr><th>Finished</th><td>" + HttpUtility.HtmlEncode(_enddate.ToString()) + "</td></tr>");
            sb.Append("<tr><th>Lines in File</th><td>" + HttpUtility.HtmlEncode(dataLines.ToString()) + "</td></tr>");
            sb.Append("<tr><th>Imported Items</th><td>" + HttpUtility.HtmlEncode(_importeditems.Count.ToString()) + "</td></tr>");
            sb.Append("<tr><th>Error Items</th><td>" + HttpUtility.HtmlEncode(_importproblems.Count.ToString()) + "</td></tr>");
            sb.Append("</table></div>");
            sb.AppendLine("<div class=\"impproblems\"><h2>Import Problems</h2>");
            int linecount = 0;
            if (_importproblems.Count.Equals(0))
            {
                sb.Append("<div class=\"success\">There were no problems with the import.</div>");
            }
            else
            {
                sb.Append("<table>");

                sb.Append(ImportError.toTableRowHeader());
                linecount = 0;
                foreach (ImportError ie in _importproblems)
                {
                    linecount += 1;
                    sb.Append(ie.toTableRow(linecount % 2 == 0 ? "Even" : "Odd"));
                }
                sb.Append("</table>");
            }
            sb.Append("</div>");
            sb.AppendLine("<div class=\"impitems\"><h2>Imported Items</h2>");
            if (_importeditems.Count.Equals(0))
            {
                sb.Append("<div class=\"noimportitems\">There were no items imported.</div>");
            }
            else
            {
                sb.Append("<table>");
                sb.Append(ImportedItem.toTableRowHeader());
                linecount = 0;
                foreach (ImportedItem ii in _importeditems)
                {
                    linecount += 1;
                    sb.Append(ii.toTableRow(linecount % 2 == 0 ? "Even" : "Odd"));
                }
                sb.Append("</table>");
            }
            sb.Append("</div>");
            sb.Append("</div>");


            return sb.ToString();
        }
    }
}
