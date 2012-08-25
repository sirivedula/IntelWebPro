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
    public class lkup_department : IntelWebObject
    {
        private Int32 _department_id;
        private string _department_code;
        private string _department_code_description;
        private string _first_name;
        private string _last_name;
        private string _address1;
        private string _address2;
        private string _city;
        private string _state_code;
        private string _zip_code;
        private string _mobile_number;
        private string _land_number;
        private string _land_number_extension;
        private string _fax_number;
        private string _email_id;
        private string _created_by;
        private DateTime _created_date;
        private string _last_modified_by;
        private DateTime _last_modified_date;

        #region NonTemplateCode
        
        #endregion

        protected lkup_department()
        {

        }
        public lkup_department(CurrentUser oUser, List<IntelWebField> oselectFieldsList, bool _internalFlag_LoadingFromDataSet)
        {
            base.loadObjectDefaults = _internalFlag_LoadingFromDataSet;
            Initialize(oUser);
            base.selectFieldsList = oselectFieldsList;
        }

        public lkup_department(CurrentUser oUser)
        {
            Initialize(oUser);
        }
        public lkup_department(CurrentUser oUser, string selectFields)
        {
            Initialize(oUser);
            base.selectFields = selectFields;
        }
        public override List<IntelWebField> myFields()
        {
            List<IntelWebField> _myFields = new List<IntelWebField>(); base.constructing = false;
            _myFields.Add(new IntelWebField("department_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("department_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("department_code_description", System.Data.OleDb.OleDbType.VarChar, 100, 100, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("first_name", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("last_name", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("address1", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("address2", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("city", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("state_code", System.Data.OleDb.OleDbType.Char, 2, 2, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("zip_code", System.Data.OleDb.OleDbType.VarChar, 15, 15, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("mobile_number", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("land_number", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("land_number_extension", System.Data.OleDb.OleDbType.VarChar, 5, 5, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("fax_number", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("email_id", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("created_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("created_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));
            _myFields.Add(new IntelWebField("last_modified_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("last_modified_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));

            return _myFields;
        }
        private void Initialize(CurrentUser oUser)
        {
            base.User = oUser;
            base.primaryKey = new IntelWebField("department_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null);
            base.FieldList = this.myFields();
            base.tableName = "lkup_department";
        }

        [DataMember]
        public Int32 department_id
        {
            get { return _department_id; }
            set
            {
                _department_id = value;
                base.addChange("department_id", value);
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
        public string department_code_description
        {
            get { return _department_code_description; }
            set
            {
                _department_code_description = value;
                base.addChange("department_code_description", value);
            }
        }

        [DataMember]
        public string first_name
        {
            get { return _first_name; }
            set
            {
                _first_name = value;
                base.addChange("first_name", value);
            }
        }

        [DataMember]
        public string last_name
        {
            get { return _last_name; }
            set
            {
                _last_name = value;
                base.addChange("last_name", value);
            }
        }

        [DataMember]
        public string address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                base.addChange("address1", value);
            }
        }

        [DataMember]
        public string address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                base.addChange("address2", value);
            }
        }

        [DataMember]
        public string city
        {
            get { return _city; }
            set
            {
                _city = value;
                base.addChange("city", value);
            }
        }

        [DataMember]
        public string state_code
        {
            get { return _state_code; }
            set
            {
                _state_code = value;
                base.addChange("state_code", value);
            }
        }

        [DataMember]
        public string zip_code
        {
            get { return _zip_code; }
            set
            {
                _zip_code = value;
                base.addChange("zip_code", value);
            }
        }

        [DataMember]
        public string mobile_number
        {
            get { return _mobile_number; }
            set
            {
                _mobile_number = value;
                base.addChange("mobile_number", value);
            }
        }

        [DataMember]
        public string land_number
        {
            get { return _land_number; }
            set
            {
                _land_number = value;
                base.addChange("land_number", value);
            }
        }

        [DataMember]
        public string land_number_extension
        {
            get { return _land_number_extension; }
            set
            {
                _land_number_extension = value;
                base.addChange("land_number_extension", value);
            }
        }

        [DataMember]
        public string fax_number
        {
            get { return _fax_number; }
            set
            {
                _fax_number = value;
                base.addChange("fax_number", value);
            }
        }

        [DataMember]
        public string email_id
        {
            get { return _email_id; }
            set
            {
                _email_id = value;
                base.addChange("email_id", value);
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
