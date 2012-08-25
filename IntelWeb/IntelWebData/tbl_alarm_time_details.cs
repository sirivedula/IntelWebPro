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
    public class tbl_alarm_time_details : IntelWebObject
    {
        private Int32 _time_details_id;
        private string _tier_code;
        private string _department_code;
        private string _site_code;
        private string _panel_code;
        private string _alarm_device_code;
        private DateTime _alarm_date;
        private string _alarm_hour;
        private string _alarm_time;
        private string _alarm_details;
        private string _alarm_comment;
        private string _created_by;
        private DateTime _created_date;
        private string _last_modified_by;
        private DateTime _last_modified_date;

        #region NonTemplateCode
        #endregion

        protected tbl_alarm_time_details()
        {

        }
        public tbl_alarm_time_details(CurrentUser oUser, List<IntelWebField> oselectFieldsList, bool _internalFlag_LoadingFromDataSet)
        {
            base.loadObjectDefaults = _internalFlag_LoadingFromDataSet;
            Initialize(oUser);
            base.selectFieldsList = oselectFieldsList;
        }

        public tbl_alarm_time_details(CurrentUser oUser)
        {
            Initialize(oUser);
        }
        public tbl_alarm_time_details(CurrentUser oUser, string selectFields)
        {
            Initialize(oUser);
            base.selectFields = selectFields;
        }
        public override List<IntelWebField> myFields()
        {
            List<IntelWebField> _myFields = new List<IntelWebField>(); base.constructing = false;
            _myFields.Add(new IntelWebField("time_details_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("tier_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("department_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("site_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("panel_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("alarm_device_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("alarm_date", System.Data.OleDb.OleDbType.DBDate, 0, 23, 3, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("alarm_hour", System.Data.OleDb.OleDbType.VarChar, 15, 15, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("alarm_time", System.Data.OleDb.OleDbType.VarChar, 15, 15, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("alarm_details", System.Data.OleDb.OleDbType.VarChar, 150, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("alarm_comment", System.Data.OleDb.OleDbType.VarChar, 250, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("created_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("created_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));
            _myFields.Add(new IntelWebField("last_modified_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("last_modified_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));

            return _myFields;
        }
        private void Initialize(CurrentUser oUser)
        {
            base.User = oUser;
            base.primaryKey = new IntelWebField("time_details_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null);
            base.FieldList = this.myFields();
            base.tableName = "tbl_alarm_time_details";
        }

        [DataMember]
        public Int32 time_details_id
        {
            get { return _time_details_id; }
            set
            {
                _time_details_id = value;
                base.addChange("time_details_id", value);
            }
        }

        [DataMember]
        public string tier_code
        {
            get { return _tier_code; }
            set
            {
                _tier_code = value;
                base.addChange("tier_code", value);
            }
        }

        [DataMember]
        public string department_code
        {
            get { return _department_code; }
            set
            {
                _department_code = value;
                base.addChange("department_code", value);
            }
        }

        [DataMember]
        public string site_code
        {
            get { return _site_code; }
            set
            {
                _site_code = value;
                base.addChange("site_code", value);
            }
        }

        [DataMember]
        public string panel_code
        {
            get { return _panel_code; }
            set
            {
                _panel_code = value;
                base.addChange("panel_code", value);
            }
        }

        [DataMember]
        public string alarm_device_code
        {
            get { return _alarm_device_code; }
            set
            {
                _alarm_device_code = value;
                base.addChange("alarm_device_code", value);
            }
        }

        [DataMember]
        public DateTime alarm_date
        {
            get { return _alarm_date; }
            set
            {
                _alarm_date = value;
                base.addChange("alarm_date", value);
            }
        }

        [DataMember]
        public string alarm_hour
        {
            get { return _alarm_hour; }
            set
            {
                _alarm_hour = value;
                base.addChange("alarm_hour", value);
            }
        }

        [DataMember]
        public string alarm_time
        {
            get { return _alarm_time; }
            set
            {
                _alarm_time = value;
                base.addChange("alarm_time", value);
            }
        }

        [DataMember]
        public string alarm_details
        {
            get { return _alarm_details; }
            set
            {
                _alarm_details = value;
                base.addChange("alarm_details", value);
            }
        }

        [DataMember]
        public string alarm_comment
        {
            get { return _alarm_comment; }
            set
            {
                _alarm_comment = value;
                base.addChange("alarm_comment", value);
            }
        }

        [DataMember]
        public string created_by
        {
            get { return _created_by; }
            set
            {
                _created_by = value;
                base.addChange("created_by", value);
            }
        }

        [DataMember]
        public DateTime created_date
        {
            get { return _created_date; }
            set
            {
                _created_date = value;
                base.addChange("created_date", value);
            }
        }

        [DataMember]
        public string last_modified_by
        {
            get { return _last_modified_by; }
            set
            {
                _last_modified_by = value;
                base.addChange("last_modified_by", value);
            }
        }

        [DataMember]
        public DateTime last_modified_date
        {
            get { return _last_modified_date; }
            set
            {
                _last_modified_date = value;
                base.addChange("last_modified_date", value);
            }
        }

    }
}
