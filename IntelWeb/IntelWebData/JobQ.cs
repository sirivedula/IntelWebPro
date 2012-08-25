using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;

namespace IntelWeb
{
    [Serializable]
    [DataContract]
    public class JobQ : IntelWebObject
    {
        private Guid _guidfield;
        private DateTime _lastchange;
        private string _lastChangeUser;
        private string _jobType;
        private string _jobResults;
        private string _jobStatus;
        private string _jobMessages;
        private Int32 _jobId;
        private DateTime _queuedtime;
        private DateTime? _starttime;
        private DateTime? _endtime;
        private string _filename;
        private string _tiername;
        private string _queueduser;

        #region NonTemplateCode
        #endregion

        protected JobQ()
        {

        }
        public JobQ(CurrentUser oUser, List<IntelWebField> oselectFieldsList, bool _internalFlag_LoadingFromDataSet)
        {
            base.loadObjectDefaults = _internalFlag_LoadingFromDataSet;
            Initialize(oUser);
            base.selectFieldsList = oselectFieldsList;
        }

        public JobQ(CurrentUser oUser)
        {
            Initialize(oUser);
        }
        public JobQ(CurrentUser oUser, string selectFields)
        {
            Initialize(oUser);
            base.selectFields = selectFields;
        }
        public override List<IntelWebField> myFields()
        {
            List<IntelWebField> _myFields = new List<IntelWebField>(); base.constructing = false;
            _myFields.Add(new IntelWebField("guidfield", System.Data.OleDb.OleDbType.Guid, 0, 0, 0, DataRowVersion.Default, false, (Guid.NewGuid())));
            _myFields.Add(new IntelWebField("lastchange", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, DateTime.Now));
            _myFields.Add(new IntelWebField("lastChangeUser", System.Data.OleDb.OleDbType.VarChar, 128, 128, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("jobType", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("jobResults", System.Data.OleDb.OleDbType.LongVarChar, 2147483647, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("jobStatus", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("jobMessages", System.Data.OleDb.OleDbType.VarChar, 1000, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("jobId", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("queuedtime", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("starttime", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("endtime", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("filename", System.Data.OleDb.OleDbType.VarChar, 1000, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("tiername", System.Data.OleDb.OleDbType.VarChar, 50, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("queueduser", System.Data.OleDb.OleDbType.VarChar, 128, 128, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));

            return _myFields;
        }
        private void Initialize(CurrentUser oUser)
        {
            base.User = oUser;
            base.primaryKey = new IntelWebField("guidfield", OleDbType.Guid, 50, 0, 0, DataRowVersion.Current, false, "");
            base.FieldList = this.myFields();
            base.tableName = "JobQ";
        }

        [DataMember]
        public Guid guidfield
        {
            get { return _guidfield; }
            set
            {
                _guidfield = value;
                base.addChange("guidfield", value);
            }
        }

        [DataMember]
        public DateTime lastchange
        {
            get { return _lastchange; }
            set
            {
                _lastchange = value;
                base.addChange("lastchange", value);
            }
        }

        [DataMember]
        public string lastChangeUser
        {
            get { return _lastChangeUser; }
            set
            {
                _lastChangeUser = value;
                base.addChange("lastChangeUser", value);
            }
        }

        [DataMember]
        public string jobType
        {
            get { return _jobType; }
            set
            {
                _jobType = value;
                base.addChange("jobType", value);
            }
        }

        [DataMember]
        public string jobResults
        {
            get { return _jobResults; }
            set
            {
                _jobResults = value;
                base.addChange("jobResults", value);
            }
        }

        [DataMember]
        public string jobStatus
        {
            get { return _jobStatus; }
            set
            {
                _jobStatus = value;
                base.addChange("jobStatus", value);
            }
        }

        [DataMember]
        public string jobMessages
        {
            get { return _jobMessages; }
            set
            {
                _jobMessages = value;
                base.addChange("jobMessages", value);
            }
        }

        [DataMember]
        public Int32 jobId
        {
            get { return _jobId; }
            set
            {
                _jobId = value;
                base.addChange("jobId", value);
            }
        }

        [DataMember]
        public DateTime queuedtime
        {
            get { return _queuedtime; }
            set
            {
                _queuedtime = value;
                base.addChange("queuedtime", value);
            }
        }

        [DataMember]
        public DateTime? starttime
        {
            get { return _starttime; }
            set
            {
                _starttime = value;
                base.addChange("starttime", value);
            }
        }

        [DataMember]
        public DateTime? endtime
        {
            get { return _endtime; }
            set
            {
                _endtime = value;
                base.addChange("endtime", value);
            }
        }

        [DataMember]
        public string filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                base.addChange("filename", value);
            }
        }

        [DataMember]
        public string tiername
        {
            get { return _tiername; }
            set
            {
                _tiername = value;
                base.addChange("tiername", value);
            }
        }

        [DataMember]
        public string queueduser
        {
            get { return _queueduser; }
            set
            {
                _queueduser = value;
                base.addChange("queueduser", value);
            }
        }
    }
}
