using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace IntelWebProc
{
    public class DownloadFile
    {
        public string errorMsg { get; set; }
        public string ftpServerIP { get; set; }
        public string remoteDir { get; set; }
        public string ftpUserId { get; set; }
        public string ftpPassword { get; set; }
        public string localDestnDir { get; set; }

        public DownloadFile()
        {
            errorMsg = "";
        }

        public bool Download(string file)
        {
            bool result = true; 
            try
            {                
                string uri = "ftp://" + ftpServerIP + "/" + remoteDir + "/" + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    result = false;
                }       
                FtpWebRequest reqFTP;                
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + remoteDir + "/" + file));
                reqFTP.Credentials = new NetworkCredential(ftpUserId, ftpPassword);                
                reqFTP.KeepAlive = false;                
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;                                
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;                 
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                FileStream writeStream = new FileStream(localDestnDir + "\\" + file, FileMode.Create);                
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);               
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }                
                writeStream.Close();
                response.Close(); 
            }
            catch (WebException wEx)
            {
                errorMsg = wEx.Message;
                result = false;
            }

            return result;
        }

    }
}
