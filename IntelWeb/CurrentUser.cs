using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace IntelWeb
{
    public class CurrentUser
    {

        private bool _loggedIn = true;

        private string _username;
        private string _IPAddress = "";
        private ApplicationException _error;
        private List<ApplicationException> _errors = new List<ApplicationException>();
        private OleDbTransaction _dbtrans;
        private Boolean _intrans = false;
        public Guid? IntelWebSession { get; set; }

        public void SleepWithMax(Int32 sleepMilliseconds, Int32 maxMillisecondstosleep)
        {
            System.Threading.Thread.Sleep(Math.Min(sleepMilliseconds, maxMillisecondstosleep));
        }

        public string hashedPassword(string password)
        {
            MD5 md = MD5.Create();
            string shash = this.userName + password;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] result = md.ComputeHash(encoding.GetBytes(shash));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();

        }

        public Boolean loginIsValid(string password)
        {
            return true;
        }

        public IntelWebObject protoObject(string objectType)
        {
            IntelWebObject tobj = new IntelWebObject();
            Type mytype = Type.GetType("IntelWeb." + objectType);
            if (mytype == null)
            {
                throw new ApplicationException("The type '" + objectType + "' could not be found.");
            }
            tobj = (IntelWebObject)Activator.CreateInstance(mytype, new object[] { this });
            return tobj;
        }

        public Boolean inTrans()
        {
            return _intrans;
        }

        public OleDbTransaction dbTrans()
        {
            return _dbtrans;
        }

        public OleDbConnection dbConnection()
        {
            DataBase db = new DataBase();
            return db.dbCon;
        }

        public ApplicationException lastError
        {
            get { return _error; }
            set
            {
                _errors.Add(value);
                _error = value;
            }
        }

        public bool loggedIn
        {
            get { return _loggedIn; }
            private set { _loggedIn = value; }
        }
        public string userName
        {
            get { return _username; }
            set { _username = value; }
        }

        public void rollbackTrans()
        {
            _dbtrans.Rollback();
            _intrans = false;
        }

        public void commitTrans()
        {
            _dbtrans.Commit();
            _intrans = false;
        }

        public void beginTrans()
        {
            if (_dbtrans != null && _intrans)
            {
                throw new ApplicationException("There is a transaction already in progress");
            }
            _intrans = true;
            _dbtrans = this.dbConnection().BeginTransaction();
        }

        private IntelWebUsers _intelWebUser;

        public void Load()
        {
            _loggedIn = true;
           _intelWebUser = new IntelWebUsers(this);
           _intelWebUser.UserName = this.userName;
           _intelWebUser.LoadSingle();             
        }

        public IntelWebUsers HRUser
        {
            get { return _intelWebUser; }
        }

        public CurrentUser()
        {
            _username = "";
            _loggedIn = false;
        }

        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }

        public void Dispose()
        {
            try
            {
                if (_intrans)
                {
                    _dbtrans.Rollback();
                }
            }
            catch (Exception) { }
            finally
            {
                if (_dbtrans != null) { _dbtrans.Dispose(); }
            }
        }

    }
}
