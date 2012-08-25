using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using IntelWeb;
using System.Text;

namespace IntelWebSite
{
    public partial class wfIntelWebImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.UseMultiPartForm = true;
        }

        private CurrentUser cuser;

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            string SaveFile="";
            cuser = new CurrentUser();
            cuser.userName = "Balaji";
            cuser.Load();
            lkup_tier tier = new lkup_tier(cuser);
            List<IntelWebObject> Tiers = tier.Load();
            this.ltrTiers.Text = string.Join("", Tiers.Cast<lkup_tier>().Select(x => "<option value=\"" + HttpUtility.HtmlEncode(x.tier_code) + "\">" + HttpUtility.HtmlEncode(x.tier_code) + "</option>").ToArray());

            if (IsPostBack)
            {
                bool createJob = false;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFile File1 = Request.Files[0];
                    if (File1.ContentLength > 0)
                    {
                        SaveFile = System.IO.Path.GetFileName(File1.FileName);
                        if (System.IO.Path.GetExtension(SaveFile).Equals(".xls", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string SaveLocation = Server.MapPath("Data") + "\\" + SaveFile;
                            try
                            {
                                File1.SaveAs(SaveLocation);
                                createJob = true;
                            }
                            catch (Exception ex)
                            {
                                ltrMessage.Text = "<div style=\"border:1px solid #FF0000;\">Error: " + HttpUtility.HtmlEncode(ex.Message) + "</div>";
                            }
                        }
                        else
                        {
                            ltrMessage.Text = "<div style=\"border:1px solid #FFFF00;\">Impoter support's only Excel files.</div>";
                        }
                    }
                    else
                    {
                        string xlsText = Request.Form["xlsTextArea"];
                        if (!string.IsNullOrEmpty(xlsText))
                        {
                            SaveFile = randomFile() + ".txt";
                            StreamWriter outfile = new StreamWriter(Server.MapPath("Data") + "\\" + SaveFile);
                            outfile.Write(xlsText);
                            outfile.Close();
                            createJob = true;
                        }
                    }
                }
                if (createJob)
                {
                    JobQ job = new JobQ(cuser);
                    job.jobType = "Import";
                    job.jobStatus = "active";
                    job.tiername = Request.Form["tiername"];
                    job.filename = SaveFile;
                    if (job.save())
                    {
                        ltrMessage.Text = "<div style=\"border:1px solid #00FF00;\">The file has been uploaded.</div>";
                    }
                    else
                    {
                        ltrMessage.Text = "<div style=\"border:1px solid #FF0000;\">Error: " + HttpUtility.HtmlEncode(job.saveErrorText) + "</div>";
                    }
                }
            }

            ltrJobStatus.Text = JobStatusHTML();
        }

        private string JobStatusHTML()
        {
            StringBuilder sb = new StringBuilder();
            JobQ jobs = new JobQ(cuser);
            jobs.selectFields = "jobtype,jobstatus,jobid,queuedtime,starttime,endtime,filename,queueduser,tiername";
            List<IntelWebObject> jobList = (List<IntelWebObject>) jobs.Load("", "queuedtime desc");
            sb.Append("<div class=\"centered\"><table class=\"webgrid\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><thead class=\"gridheader\"><tr><th>Job Id</th><th>Job Type</th><th>Job Status</th><th>Queued Time</th><th>Start Time</th><th>End Time</th><th>File Name</th><th>Queued User</th><th>Tier Name</th><th>Results</th></tr></thead>");
            int rowCnt = 0;
            foreach (JobQ job in jobList)
            {                
                sb.Append("<tr " + ((rowCnt % 2 == 0)?"":"class=\"gridodd\"") + "><td>" + HttpUtility.HtmlEncode(job.jobId.ToString()) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.jobType) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.jobStatus) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.queuedtime.ToString("MM/dd/yyyy hh:mm")) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode((job.starttime.HasValue ? job.starttime.Value.ToString("MM/dd/yyyy hh:mm"):"")) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.endtime.HasValue ? job.endtime.Value.ToString("MM/dd/yyyy hh:mm"):"") + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.filename) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.queueduser) + "</td>");
                sb.Append("<td>" + HttpUtility.HtmlEncode(job.tiername) + "</td>");
                sb.Append("<td>" + (job.jobStatus.Equals("done", StringComparison.InvariantCultureIgnoreCase)?"<a href=\"javascript:void(0);\" onclick=\"showResults();\">Download</a>":"") + "</tr>");
                rowCnt++;
            }
            sb.Append("</table></div>");
            return sb.ToString();
        }

        private string randomFile()
        {
            string result = "";
            string randomStr = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                result += randomStr[rnd.Next(randomStr.Length - 1)];
            }
            return result;
        }
    }
}
