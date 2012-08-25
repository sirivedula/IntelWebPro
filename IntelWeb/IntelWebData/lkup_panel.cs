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
    public class lkup_panel : IntelWebObject
    {
        private Int32 _panel_id;
        private string _site_code;
        private string _panel_code;
        private string _panel_code_description;
        private string _alarm_device_code;
        private string _created_by;
        private DateTime _created_date;
        private string _last_modified_by;
        private DateTime _last_modified_date;

        #region NonTemplateCode

        #endregion

        protected lkup_panel()
        {

        }
        public lkup_panel(CurrentUser oUser, List<IntelWebField> oselectFieldsList, bool _internalFlag_LoadingFromDataSet)
        {
            base.loadObjectDefaults = _internalFlag_LoadingFromDataSet;
            Initialize(oUser);
            base.selectFieldsList = oselectFieldsList;
        }

        public lkup_panel(CurrentUser oUser)
        {
            Initialize(oUser);
        }
        public lkup_panel(CurrentUser oUser, string selectFields)
        {
            Initialize(oUser);
            base.selectFields = selectFields;
        }
        public override List<IntelWebField> myFields()
        {
            List<IntelWebField> _myFields = new List<IntelWebField>(); base.constructing = false;
            _myFields.Add(new IntelWebField("panel_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("site_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("panel_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("panel_code_description", System.Data.OleDb.OleDbType.VarChar, 100, 100, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("alarm_device_code", System.Data.OleDb.OleDbType.VarChar, 100, 100, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("created_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("created_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));
            _myFields.Add(new IntelWebField("last_modified_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("last_modified_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));

            return _myFields;
        }
        private void Initialize(CurrentUser oUser)
        {
            base.User = oUser;
            base.primaryKey = new IntelWebField("panel_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null);
            base.FieldList = this.myFields();
            base.tableName = "lkup_panel";
        }

        [DataMember]
        public Int32 panel_id
        {
            get { return _panel_id; }
            set
            {
                _panel_id = value;
                base.addChange("panel_id", value);
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
        public string panel_code_description
        {
            get { return _panel_code_description; }
            set
            {
                _panel_code_description = value;
                base.addChange("panel_code_description", value);
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
