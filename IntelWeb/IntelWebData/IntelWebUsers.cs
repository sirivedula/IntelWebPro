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
    public class IntelWebUsers : IntelWebObject
    {
        private Guid _guidfield;
        private DateTime _lastchange;
        private string _lastChangeUser;
        private string _UserName;
        private string _Password;
        private string _emailAddress;

        #region NonTemplateCode
        #endregion

        protected IntelWebUsers()
        {

        }
        public IntelWebUsers(CurrentUser oUser, List<IntelWebField> oselectFieldsList, bool _internalFlag_LoadingFromDataSet)
        {
            base.loadObjectDefaults = _internalFlag_LoadingFromDataSet;
            Initialize(oUser);
            base.selectFieldsList = oselectFieldsList;
        }

        public IntelWebUsers(CurrentUser oUser)
        {
            Initialize(oUser);
        }
        public IntelWebUsers(CurrentUser oUser, string selectFields)
        {
            Initialize(oUser);
            base.selectFields = selectFields;
        }
        public override List<IntelWebField> myFields()
        {
            List<IntelWebField> _myFields = new List<IntelWebField>(); base.constructing = false;
            _myFields.Add(new IntelWebField("guidfield", System.Data.OleDb.OleDbType.Guid, 0, 0, 0, DataRowVersion.Default, false, (Guid.NewGuid())));
            _myFields.Add(new IntelWebField("lastchange", System.Data.OleDb.OleDbType.DBTimeStamp, 0, 23, 3, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("lastChangeUser", System.Data.OleDb.OleDbType.VarChar, 128, 128, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("UserName", System.Data.OleDb.OleDbType.VarChar, 50, 50, 0, DataRowVersion.Current, false, null));
            _myFields.Add(new IntelWebField("Password", System.Data.OleDb.OleDbType.VarChar, 128, 128, 0, DataRowVersion.Current, true, null));
            _myFields.Add(new IntelWebField("emailAddress", System.Data.OleDb.OleDbType.VarChar, 128, 128, 0, DataRowVersion.Current, true, null));

            return _myFields;
        }
        private void Initialize(CurrentUser oUser)
        {
            base.User = oUser;
            base.primaryKey = new IntelWebField("guidfield", OleDbType.Guid, 50, 0, 0, DataRowVersion.Current, false, "");
            base.FieldList = this.myFields();
            base.tableName = "Users";
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
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                base.addChange("UserName", value);
            }
        }

        [DataMember]
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                base.addChange("Password", value);
            }
        }

        [DataMember]
        public string emailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                base.addChange("emailAddress", value);
            }
        }

    }
}
