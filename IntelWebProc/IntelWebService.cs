using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using IntelWeb;

namespace IntelWebProc
{
    public partial class IntelWebService : ServiceBase
    {
        public IntelWebService()
        {
            InitializeComponent();

        }

        public void StartUp()
        {
            cuser.userName = "Balaji";
            cuser.Load();

            /* Start After a Min and every 5 min check queue */
            myTimer = new Timer(CheckJobQ, this, 60, 10*1000);
        }

        public void ShutDown()
        {
            myTimer.Dispose();
        }

        protected override void OnStart(string[] args)
        {
            this.StartUp();
        }

        protected override void OnStop()
        {
            this.ShutDown();
        }

        private Timer myTimer;
        private CurrentUser cuser = new CurrentUser();

        private void CheckJobQ(object state)
        {
            JobQ tempJob = new JobQ(cuser);
            tempJob.jobStatus = "active";
            tempJob.LoadSingle();
            Console.Write("{1} Checking Job Queue...{0} Jobs Found.\n", (tempJob.isNew ? 0 : 1), DateTime.Now);
            if (!tempJob.isNew)
            {
                tempJob.jobStatus = "running";
                tempJob.starttime = DateTime.Now;
                tempJob.save();
                Importer objImp = new Importer();
                objImp.CJobQ = tempJob;
                Thread oThread = new Thread(new ThreadStart(objImp.doImport));
                oThread.Start();
            }
        }
    }


    public class Importer
    {
       public JobQ CJobQ { get; set; }
       public string lastError = "";

       public void doImport()
       {
           if (CJobQ.filename.Length > 0)
           {
               string localPath = "D:\\Balaji";
               string sourceFile = localPath + "\\" + CJobQ.filename;
               DownloadFile objDownload = new DownloadFile();
               objDownload.ftpServerIP = "iweb.vasbal.com";
               objDownload.ftpUserId = "gsquote";
               objDownload.ftpPassword = "gs@quote";
               objDownload.localDestnDir = localPath;
               objDownload.remoteDir = "httpdocs/data";

               if (objDownload.Download(CJobQ.filename))
               {
                   string filext = System.IO.Path.GetExtension(sourceFile);
                   string destFilename = localPath + "\\" + System.IO.Path.GetFileName(sourceFile.Replace(".xls", ".txt"));
                   if (filext.Equals(".xls", StringComparison.InvariantCultureIgnoreCase))
                   {
                       //Make Text File From Excel
                       FileConverter fileConv = new FileConverter();
                       fileConv.SrcFile = sourceFile;
                       fileConv.DestFile = destFilename; 
                       if (System.IO.File.Exists(sourceFile))
                       {
                           fileConv.ConvertXLSToText();
                       }
                   }
                   IntelWebImporter.ImportRun imp = new IntelWebImporter.ImportRun();
                   imp.fullFileName = destFilename;
                   imp.UserName = "Balaji";
                   imp.tierCode = CJobQ.tiername;
                   imp.tierCode = CJobQ.tiername;
                   try
                   {
                       imp.doImport(true);
                       CJobQ.jobResults = imp.toHTML();
                       CJobQ.jobStatus = "Done";
                   }
                   catch (Exception ex)
                   {
                       CJobQ.jobResults = ex.Message;
                       CJobQ.jobStatus = "error";
                   }
               }
               else
               {
                   CJobQ.jobStatus = "error";
                   CJobQ.jobResults = "Download File Error";
               }
               CJobQ.endtime = DateTime.Now;
               CJobQ.save();
           }
           else
           {
               CJobQ.endtime = DateTime.Now;
               CJobQ.jobStatus = "error";
               CJobQ.save();
               lastError = "File does not exists.";
           }
       }

    }

}
