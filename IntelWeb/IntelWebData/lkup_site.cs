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
    public class lkup_site : IntelWebObject
    {
        private Int32 _site_id;
        private string _department_code;
        private string _site_code;
        private string _site_code_description;
        private string _building_name;
        private string _facility_contact;
        private string _path;
        private string _address1;
        private string _address2;
        private string _city;
        private string _state_code;
        private string _zip_code;
        private string _country_code;
        private string _mobile_number;
        private string _land_number;
        private string _land_number_extension;
        private string _fax_number;
        private string _email_id;
        private Int32? _latitude_small;
        private Int32? _longitude_smal;
        private decimal? _latitude_whole;
        private decimal? _longitude_whole;
        private string _created_by;
        private DateTime _created_date;
        private string _last_modified_by;
        private DateTime _last_modified_date;

        #region NonTemplateCode

        #endregion

        protected lkup_site()
        {

        }
        public lkup_site(CurrentUser oUser, List<IntelWebField> oselectFieldsList, bool _internalFlag_LoadingFromDataSet)
        {
            base.loadObjectDefaults = _internalFlag_LoadingFromDataSet;
            Initialize(oUser);
            base.selectFieldsList = oselectFieldsList;
        }

        public lkup_site(CurrentUser oUser)
        {
            Initialize(oUser);
        }
        public lkup_site(CurrentUser oUser, string selectFields)
        {
            Initialize(oUser);
            base.selectFields = selectFields;
        }
        public override List<IntelWebField> myFields()
        {
            List<IntelWebField> _myFields = new List<IntelWebField>(); base.constructing = false;
            _myFields.Add(new IntelWebField("site_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("department_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("site_code", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("site_code_description", System.Data.OleDb.OleDbType.VarChar, 100, 100, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("building_name", System.Data.OleDb.OleDbType.VarChar, 250, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("facility_contact", System.Data.OleDb.OleDbType.VarChar, 250, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("path", System.Data.OleDb.OleDbType.VarChar, 250, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("address1", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("address2", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("city", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("state_code", System.Data.OleDb.OleDbType.Char, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("zip_code", System.Data.OleDb.OleDbType.VarChar, 15, 15, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("country_code", System.Data.OleDb.OleDbType.VarChar, 250, 0, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("mobile_number", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("land_number", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("land_number_extension", System.Data.OleDb.OleDbType.VarChar, 5, 5, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("fax_number", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("email_id", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("latitude_small", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("longitude_smal", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("latitude_whole", System.Data.OleDb.OleDbType.Decimal, 0, 9, 4, DataRowVersion.Default, true, decimal.Parse("0")));
            _myFields.Add(new IntelWebField("longitude_whole", System.Data.OleDb.OleDbType.Decimal, 0, 9, 4, DataRowVersion.Default, true, decimal.Parse("0")));
            _myFields.Add(new IntelWebField("created_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("created_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));
            _myFields.Add(new IntelWebField("last_modified_by", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Default, false, ((this.User == null ? "" : this.User.userName))));
            _myFields.Add(new IntelWebField("last_modified_date", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Default, false, (DateTime.Now)));

            return _myFields;
        }
        private void Initialize(CurrentUser oUser)
        {
            base.User = oUser;
            base.primaryKey = new IntelWebField("site_id", System.Data.OleDb.OleDbType.Integer, 0, 10, 0, DataRowVersion.Current, false, null);
            base.FieldList = this.myFields();
            base.tableName = "lkup_site";
        }

        [DataMember]
        public Int32 site_id
        {
            get { return _site_id; }
            set
            {
                _site_id = value;
                base.addChange("site_id", value);
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
        public string site_code_description
        {
            get { return _site_code_description; }
            set
            {
                _site_code_description = value;
                base.addChange("site_code_description", value);
            }
        }

        [DataMember]
        public string building_name
        {
            get { return _building_name; }
            set
            {
                _building_name = value;
                base.addChange("building_name", value);
            }
        }

        [DataMember]
        public string facility_contact
        {
            get { return _facility_contact; }
            set
            {
                _facility_contact = value;
                base.addChange("facility_contact", value);
            }
        }

        [DataMember]
        public string path
        {
            get { return _path; }
            set
            {
                _path = value;
                base.addChange("path", value);
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
        public string country_code
        {
            get { return _country_code; }
            set
            {
                _country_code = value;
                base.addChange("country_code", value);
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
        public Int32? latitude_small
        {
            get { return _latitude_small; }
            set
            {
                _latitude_small = value;
                base.addChange("latitude_small", value);
            }
        }

        [DataMember]
        public Int32? longitude_smal
        {
            get { return _longitude_smal; }
            set
            {
                _longitude_smal = value;
                base.addChange("longitude_smal", value);
            }
        }

        [DataMember]
        public decimal? latitude_whole
        {
            get { return _latitude_whole; }
            set
            {
                _latitude_whole = value;
                base.addChange("latitude_whole", value);
            }
        }

        [DataMember]
        public decimal? longitude_whole
        {
            get { return _longitude_whole; }
            set
            {
                _longitude_whole = value;
                base.addChange("longitude_whole", value);
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
