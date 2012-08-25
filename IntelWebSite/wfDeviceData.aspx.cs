using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using IntelWeb;
using System.Data;
using System.Data.OleDb;

namespace IntelWebSite
{
    public partial class wfDeviceData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            CurrentUser cuser = new CurrentUser();
            cuser.userName = "Balaji";
            cuser.Load();

            string tiercode = Request.QueryString["tiercode"];
            string deptcode = Request.QueryString["deptcode"];
            string sitecode = Request.QueryString["sitecode"];
            string panelcode = Request.QueryString["panelcode"];
            string devicecode = Request.QueryString["devicecode"];
            string fromdate = Request.QueryString["fdate"];
            string todate = Request.QueryString["tdate"];

            tbl_alarm_time_details alarmDet = new tbl_alarm_time_details(cuser);
            IntelWebField tierFld = alarmDet.Field("tier_code");
            tierFld.fieldValue = tiercode;
            IntelWebField deptFld = alarmDet.Field("department_code");
            deptFld.fieldValue = deptcode;
            IntelWebField siteFld = alarmDet.Field("site_code");
            siteFld.fieldValue = sitecode;
            IntelWebField panelFld = alarmDet.Field("panel_code");
            panelFld.fieldValue = panelcode;
            IntelWebField deviceFld = alarmDet.Field("alarm_device_code");
            deviceFld.fieldValue = devicecode;
            IntelWebField fdateFld = new IntelWebField("todate", OleDbType.DBDate, 10, 10, 0, DataRowVersion.Current, false, fromdate);
            IntelWebField tdateFld = new IntelWebField("todate", OleDbType.DBDate, 10, 10, 0, DataRowVersion.Current, false, todate);
            List<IntelWebField> bparam = new List<IntelWebField>();
            bparam.Add(tierFld);
            bparam.Add(deptFld);
            bparam.Add(siteFld);
            bparam.Add(panelFld);
            bparam.Add(deviceFld);
            bparam.Add(fdateFld);
            bparam.Add(tdateFld);
            List<IntelWebObject> alarmDets = alarmDet.Load("tier_code=? and department_code=? and site_code=? and panel_code=? and alarm_device_code=? and alarm_date between ? and ?", "alarm_date,alarm_time", bparam);
            if (alarmDets.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table style=\"width:100%;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"webgrid\"><thead class=\"titlegrid\"><tr><th>Device</th><th>Alarm Date</th><th>Alarm Hour</th><th>Alaram Time</th><th>Alarm Details</th><th>Alarm Comment</th></tr>");
                int rowCnt = 0;
                foreach (tbl_alarm_time_details alarm in alarmDets)
                {
                    string trclass = (rowCnt % 2 == 0 ? "" : " class=\"gridodd\" ");
                    sb.Append("<tr" + trclass + "><td>" + HttpUtility.HtmlEncode(alarm.alarm_device_code) + "</td><td>" + HttpUtility.HtmlEncode(alarm.alarm_date.ToString("MM/dd/yyyy")) + "</td><td>" + HttpUtility.HtmlEncode(alarm.alarm_hour) + "</td><td>" + HttpUtility.HtmlEncode(alarm.alarm_time) + "</td><td>" + HttpUtility.HtmlEncode(alarm.alarm_details) + "</td><td>" + HttpUtility.HtmlEncode(alarm.alarm_comment) + "</td></tr>");
                    rowCnt++;
                }
                sb.Append("</table>");

                Response.Write(sb.ToString());
            }
            else
            {
                Response.Write("There are no details found.");
            }

        }
    }
}
