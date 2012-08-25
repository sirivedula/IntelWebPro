using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Reflection;
using System.ComponentModel;

namespace IntelWeb
{
    public class DataBase
    {
        public string dbConString
        {
            get
            {
                return ConfigurationSettings.AppSettings["dbConString"];
            }
        }

        private OleDbConnection _dbCon;
        public OleDbConnection dbCon
        {
            get
            {
                if (_dbCon == null)
                {
                    OleDbConnectionStringBuilder consb = new OleDbConnectionStringBuilder(this.dbConString);
                    consb.Add("Application Name", Utils.UserName??"");
                    _dbCon = new OleDbConnection(consb.ToString());
                }
                if (_dbCon.State != ConnectionState.Open) _dbCon.Open();
                return _dbCon;
            }
        }

        private OleDbCommand _dbCmd;
        public OleDbCommand dbCmd
        {
            get
            {
                if (this.dbCon.State != ConnectionState.Open)
                {
                    this.dbCon.Open();
                }
                _dbCmd = new OleDbCommand();
                _dbCmd.Connection = this.dbCon;
                return _dbCmd;
            }
        }

    }
}
