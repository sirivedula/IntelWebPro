using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using IntelWeb;
using System.Data.OleDb;
using System.Data;

namespace IntelWebSite
{
    public partial class wfPanelData : System.Web.UI.Page
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
            string fromdate = Request.QueryString["fdate"];
            string todate = Request.QueryString["tdate"];

            tbl_alarm_time_details alarmDet = new tbl_alarm_time_details(cuser);
            IntelWebField tierFld = alarmDet.Field("tier_code");
            tierFld.fieldValue = tiercode;
            IntelWebField deptFld = alarmDet.Field("department_code");
            deptFld.fieldValue = deptcode;
            IntelWebField siteFld = alarmDet.Field("site_code");
            siteFld.fieldValue = sitecode;
            IntelWebField fdateFld = new IntelWebField("todate", OleDbType.DBDate, 10, 10, 0, DataRowVersion.Current, false, fromdate);
            IntelWebField tdateFld = new IntelWebField("todate", OleDbType.DBDate, 10, 10, 0, DataRowVersion.Current, false, todate);
            List<IntelWebField> bparam = new List<IntelWebField>();
            bparam.Add(tierFld);
            bparam.Add(deptFld);
            bparam.Add(siteFld);
            bparam.Add(fdateFld);
            bparam.Add(tdateFld);
            List<IntelWebObject> alarmDets = alarmDet.Load("tier_code=? and department_code=? and site_code=? and alarm_date between ? and ?", "panel_code", bparam);
            if (alarmDets.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                /* Panels Table */
                sb.Append("<table><tr>");
                var distPanels = alarmDets.Select(x => x.Field("panel_code").fieldValue ?? "").Distinct();
                foreach (string panel in distPanels)
                {
                    sb.Append("<td>" + AlaramDeviceHTML(sitecode, panel, alarmDets) + "</td>");
                }

                sb.Append("</tr></table>");

                Response.Write(sb.ToString());
            }
            else
            {
                Response.Write("There are no details found.");
            }
        }

        private string AlaramDeviceHTML(string site, string panel, List<IntelWebObject> Details)
        {
            StringBuilder sb1 = new StringBuilder();

            /* Total Devices For the Alaram */
            int totalDevices = Details.FindAll(x => (x.Field("panel_code").fieldValue ?? "").Equals(panel)).Count();

            var distDevices = Details.FindAll(x => (x.Field("panel_code").fieldValue ?? "").Equals(panel)).Select(x => x.Field("alarm_device_code").fieldValue ?? "").Distinct();
            foreach (string device in distDevices)
            {
                int Cnt = Details.FindAll(x => ((x.Field("panel_code").fieldValue ?? "").Equals(panel) && (x.Field("alarm_device_code").fieldValue ?? "").Equals(device))).Count();
                sb1.Append("<tr><td style=\"background-color:#DFE6EC;cursor:pointer;\" onclick=\"showDetails('" + JSUtil.EnquoteJS(site) + "','" + JSUtil.EnquoteJS(panel) + "','" + JSUtil.EnquoteJS(device) + "')\" >" + HttpUtility.HtmlEncode(device) + " - " + Cnt.ToString() + "</td></tr>");

            }
            string result = "<table><tr><td style=\"background-color:#243646;color:#FFF;\">Panel - " + HttpUtility.HtmlEncode(panel) + " - " + totalDevices.ToString() + "</td></tr>" + sb1.ToString() + "</tr></table>";
            return result;
        }

    }
}
