using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.Util;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Specialized;

namespace IntelWeb
{
    public interface IIntelWebObject
    {
        IntelWebField Field(string fieldname);
    }
   
    public class ObjectRights
    {
        private bool? _allowdelete;
        private bool? _allowadd;
        private bool? _allowEdit;
        private bool? _allowView;
        public Func<bool> readOnlyPredicate;  
        public bool? allowDelete
        {
            get
            {
                if (readOnlyPredicate != null)
                {
                    return !readOnlyPredicate.Invoke();
                }
                return _allowdelete;
            }
            internal set { _allowdelete = value; }
        }
        public bool? allowAdd
        {
            get
            {
                if (readOnlyPredicate != null)
                {
                    return !readOnlyPredicate.Invoke();
                }
                return _allowadd;
            }
            internal set { _allowadd = value; }
        }
        public bool? allowView
        {
            get { return _allowView; }
            internal set { _allowView = value; }
        }
        public bool? allowEdit
        {
            get
            {
                if (readOnlyPredicate != null)
                {
                    return !readOnlyPredicate.Invoke();
                }
                return _allowEdit;
            }
            set { _allowEdit = value; }
        }

    }

    [DataContract]
    public class IntelWebObject
    {
        private string _tablename = "";
        private bool _isNew = true;
        private bool _isDirty = false;
        private bool _constructing = true;
        private IntelWebField _primarykey;

        private List<IntelWebField> _fields;
        private List<IntelWebField> _selectFieldsList = new List<IntelWebField>();
        private bool _isDeleted = false;
        private OleDbException _lastDBError;
        private string _lasterrormessage;
        protected bool loadObjectDefaults = true;

        private CurrentUser _currentuser;
        private string _sql = "";
        private bool _hasPK = true;
        public int _cacheMinutes = 0;

        private List<ApplicationException> _saveerrors;
        private List<string> _warnings = new List<string>();

        public IntelWebObject Clone()
        {
            return (IntelWebObject)MemberwiseClone();
        }

        public void revertChanges()
        {
            this.constructing = true;
            List<IntelWebField> changed = this.changedFields;
            foreach (IntelWebField fld in changed)
            {
                var t = this.GetType().GetProperty(fld.name);
                t.SetValue(this, fld.previousValue, null);
                fld.version = System.Data.DataRowVersion.Current;
                fld.isDirty = false;
            }
            this.isDirty = false;
            this.constructing = false;
        }

        public void AddFriendlyWarning(string warning)
        {
            _warnings.Add(warning);
        }
        public List<IntelWebField> selectFieldsList
        {
            get { return _selectFieldsList; }
            set { _selectFieldsList = value; }
        }

        public string selectFields
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (this.selectFieldsList.Count != 0)
                {
                    foreach (IntelWebField xx in this.selectFieldsList)
                    {
                        sb.Append(xx.name + ",");
                    }
                }
                else
                {
                    foreach (IntelWebField x in this.FieldList)
                    {
                        sb.Append(x.name + ",");
                    }
                }

                string tstring = sb.ToString();
                if (tstring != "")
                {
                    tstring = tstring.Substring(0, tstring.Length - 1);
                }
                return tstring;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim().Equals(""))
                {
                    _selectFieldsList.Clear();
                    _selectFieldsList = this.FieldList;
                    return;
                }
                _selectFieldsList.Clear();
                String[] atemp = value.Split(',');
                bool priKeyPresent = false;
                for (int x = 0; x < atemp.Length; x++)
                {
                    IntelWebField tparam = this.Field(atemp[x]);
                    if (tparam != null)
                    {
                        if (this.primaryKey != null && tparam.name.Equals(primaryKey.name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            priKeyPresent = true;
                        }
                        _selectFieldsList.Add(tparam);
                    }
                }
                if (this.primaryKey != null && !priKeyPresent)
                {
                    _selectFieldsList.Add(this.primaryKey);
                }
            }
        }


        public List<string> friendlyWarnings
        {
            get
            {
                if (_warnings == null)
                {
                    return new List<string>();
                }
                return _warnings;
            }
        }

        internal void AddSaveErrors(List<ApplicationException> se)
        {
            if (_saveerrors == null)
            {
                _saveerrors = se;

            }
            else
            {
                _saveerrors.AddRange(se);
            }
        }

        internal void AddSaveError(string ex)
        {
            if (_saveerrors == null)
            {
                _saveerrors = new List<ApplicationException>();
            }
            _saveerrors.Add(new ApplicationException(ex));
        }

        internal virtual void afterSave()
        {

        }

        internal virtual void afterDelete()
        {

        }

        internal virtual System.Data.CommandType commandType 
        {
            get
            {
                return System.Data.CommandType.Text;
            }
        }

        internal void removeChangedField(string fieldname)
        {
            IntelWebField tfield = this.Field(fieldname);
            if (tfield != null)
            {
                tfield.version = System.Data.DataRowVersion.Current;
            }

        }

        public virtual NameValueCollection FieldChoices(string fieldname)
        {
            return null;
        }

        internal NameValueCollection fillFieldChoices(Func<IntelWebObject, KeyValuePair<string, string>> fn, List<IntelWebObject> records)
        {
            var result = new System.Collections.Specialized.NameValueCollection(records.Count);
            foreach (var record in records)
            {
                var t = fn.Invoke(record);
                result.Add(t.Key, t.Value);
            }
            return result;
        }

        public string saveErrorText
        {
            get
            {
                if (_saveerrors == null) { return ""; }
                StringBuilder sb = new StringBuilder();

                foreach (ApplicationException ex in _saveerrors)
                {
                    sb.Append(ex.Message + "\n");

                }
                return sb.ToString();
            }
        }

        public List<ApplicationException> saveErrors
        {
            get
            {
                if (_saveerrors != null && _saveerrors.Count > 0)
                {
                    return _saveerrors;
                }
                return null;

            }
            set { _saveerrors = value; }
        }

        public virtual List<IntelWebField> myFields()
        {
            throw new ApplicationException("myFields not implemented in base class");
        }

        public IntelWebObject()
        {
            _hasPK = true;

        }

        public bool hasPK
        {
            get { return _hasPK; }
            set { _hasPK = value; }
        }

        private void setPK(object myobj)
        {
            PropertyInfo pki = myobj.GetType().GetProperty("primaryKey");
            IntelWebField pk = (IntelWebField)pki.GetValue(myobj, null);
            if (pk != null)
            {
                PropertyInfo pkfieldref = myobj.GetType().GetProperty(pk.name);
                pk.fieldValue = pkfieldref.GetValue(myobj, null);
            }
        }

        public string lastErrorMessage
        {
            get
            {
                if (_lasterrormessage == "")
                {
                    return _lasterrormessage;
                }
                if (_lastDBError != null)
                {
                    return _lastDBError.Message;
                }
                return "";
            }
            set
            {
                _lasterrormessage = value;
            }

        }

        public void reload()
        {
            List<IntelWebField> tlist = new List<IntelWebField>(1);
            tlist.Add(this.primaryKey);
            this.LoadSingle(this.primaryKey.name + " = ?", tlist);
        }

        public CurrentUser User
        {
            get
            {
                return _currentuser;
            }
            set
            {
                _currentuser = value;
            }
        }

        public string sanitizedError()
        {
            return _lastDBError.Message;
        }

        public OleDbException lastDBError
        {
            get { return _lastDBError; }
        }

        public IntelWebField Field(string fieldname)
        {
            if (this.FieldList == null) { this.FieldList = myFields(); }
            foreach (IntelWebField x in this.FieldList)
            {
                if (fieldname.Equals(x.name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return x;
                }
            }
            return null;
        }

        public static IntelWebObject FindOne(List<IntelWebObject> searchList, string searchField, object searchValue)
        {
            PropertyInfo t;
            object tvalue;
            if (searchList.Count > 0)
            {
                object x = searchList[0];
                MethodInfo tparam = x.GetType().GetMethod("Field");

                IntelWebField rParam = (IntelWebField)tparam.Invoke(x, new object[] { searchField });
                t = x.GetType().GetProperty(rParam.name);

                foreach (IntelWebObject xx in searchList)
                {
                    tvalue = t.GetValue(xx, null);

                    if (tvalue.Equals(searchValue))
                    {
                        return xx;
                    }
                }
            }

            return null;
        }

        public static List<IntelWebObject> FindMany(List<IntelWebObject> searchList, string searchField, object searchValue)
        {
            PropertyInfo t;
            object tvalue;
            List<IntelWebObject> retList = new List<IntelWebObject>();

            if (searchList.Count > 0)
            {
                object x = searchList[0];
                MethodInfo tparam = x.GetType().GetMethod("Field");
                IntelWebField rParam = (IntelWebField)tparam.Invoke(x, new object[] { searchField });
                t = x.GetType().GetProperty(rParam.name);

                foreach (IntelWebObject xx in searchList)
                {
                    tvalue = t.GetValue(xx, null);
                    if (tvalue.Equals(searchValue))
                    {

                        retList.Add(xx);
                    }
                }
            }
            return retList;
        }

        public List<IntelWebObject> Load(string whereclause, string sortorder, string p1name, object p1value, string p2name, object p2value, string p3name, object p3value)
        {
            if (!this.User.loggedIn) { new List<object>(); }
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            IntelWebField p2 = Field(p2name).Clone();
            p2.fieldValue = p2value;
            otemp.Add(p2);
            IntelWebField p3 = Field(p3name).Clone();
            p3.fieldValue = p3value;
            otemp.Add(p3);
            return Load(whereclause, sortorder, otemp);
        }

        public List<IntelWebObject> Load(string whereclause, string sortorder, string p1name, object p1value, string p2name, object p2value)
        {
            if (!this.User.loggedIn) { new List<object>(); }
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            IntelWebField p2 = Field(p2name).Clone();
            p2.fieldValue = p2value;
            otemp.Add(p2);
            return Load(whereclause, sortorder, otemp);
        }

        public List<IntelWebObject> Load(string whereclause, string sortorder)
        {
            if (!this.User.loggedIn) { new List<object>(); }
            List<IntelWebField> otemp = new List<IntelWebField>();
            return Load(whereclause, sortorder, otemp);
        }

        public List<IntelWebObject> Load(string whereclause, string sortorder, string p1name, object p1value)
        {
            if (!this.User.loggedIn) { new List<object>(); }
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            return Load(whereclause, sortorder, otemp);
        }

        public List<IntelWebObject> Load()
        {
            if (!this.User.loggedIn) { new List<object>(); }
            string whereclause = "";
            foreach (IntelWebField x in this.changedFields)
            {
                whereclause += x.name + " = ? and ";
            }
            if (whereclause != "")
            {
                whereclause = whereclause.Substring(0, whereclause.Length - 4);
            }
            return Load(whereclause, "", this.changedFields);
        }

        private string hashfilterparams(List<IntelWebField> filterparams)
        {
            string res = "";
            foreach (IntelWebField x in filterparams)
            {
                res += x.name + '=' + (x.fieldValue ?? "").GetHashCode().ToString() + '&';
            }

            return res;
        }
        public List<IntelWebObject> Load(string whereclause, string sortorder, List<IntelWebField> filterparams)
        {
            if (!this.User.loggedIn) { return null; }
            OleDbCommand tcommand;
            if (commandType == System.Data.CommandType.StoredProcedure)
            {
                tcommand = new OleDbCommand(this.tableName, this.connection());
            }
            else
            {
                tcommand = new OleDbCommand(this.FullSelectStatement(whereclause, sortorder), this.connection());
            }
            tcommand.CommandType = commandType;
            IntelWebParameter tempParam = new IntelWebParameter();
            if (filterparams != null)
            {
                foreach (IntelWebField apfield in filterparams)
                {
                    tcommand.Parameters.Add(apfield.parameter);
                }
            }
            List<IntelWebObject> objcol = new List<IntelWebObject>();
            try
            {
                    OleDbDataReader oread = tcommand.ExecuteReader();
                    tcommand.Parameters.Clear();

                    while (oread.Read())
                    {
                        IntelWebObject myobj = (IntelWebObject)Activator.CreateInstance(this.GetType(), new object[] { this.User, this.selectFieldsList, false });
                        this.Construct(oread, myobj);
                        objcol.Add(myobj);
                    }
                    oread.Close();
            }
            catch (OleDbException ex)
            {
                _lastDBError = ex;
                throw ex;
            }
            return objcol;
        }

        public IntelWebObject getSingle(string whereclause, List<IntelWebField> filterparams)
        {
            this.LoadSingle(whereclause, filterparams);
            return this;
        }
        public IntelWebObject getSingle(string whereclause, string p1name, object p1value, string p2name, object p2value, string p3name, object p3value)
        {
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();

            p1.fieldValue = p1value;
            otemp.Add(p1);
            IntelWebField p2 = Field(p2name).Clone();
            p2.fieldValue = p2value;
            otemp.Add(p2);
            IntelWebField p3 = Field(p3name).Clone();
            p3.fieldValue = p3value;
            otemp.Add(p3);
            return getSingle(whereclause, otemp);
        }
        public IntelWebObject getSingle(string whereclause, string p1name, object p1value, string p2name, object p2value)
        {
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            IntelWebField p2 = Field(p2name).Clone();
            p2.fieldValue = p2value;
            otemp.Add(p2);
            return getSingle(whereclause, otemp);
        }
        public IntelWebObject getSingle(string whereclause, string p1name, object p1value)
        {
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            return getSingle(whereclause, otemp);
        }
        public void LoadSingle(string whereclause, string p1name, object p1value, string p2name, object p2value, string p3name, object p3value)
        {
            if (!this.User.loggedIn) { return; }
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            IntelWebField p2 = Field(p2name).Clone();
            p2.fieldValue = p2value;
            otemp.Add(p2);
            IntelWebField p3 = Field(p3name).Clone();
            p3.fieldValue = p3value;
            otemp.Add(p3);
            LoadSingle(whereclause, otemp);
        }
        public void LoadSingle(string whereclause, string p1name, object p1value, string p2name, object p2value)
        {
            if (!this.User.loggedIn) { return; }
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            IntelWebField p2 = Field(p2name).Clone();
            p2.fieldValue = p2value;
            otemp.Add(p2);
            LoadSingle(whereclause, otemp);
        }

        public void LoadSingle(string whereclause, string p1name, object p1value)
        {
            if (!this.User.loggedIn) { return; }
            List<IntelWebField> otemp = new List<IntelWebField>();
            IntelWebField p1 = Field(p1name).Clone();
            p1.fieldValue = p1value;
            otemp.Add(p1);
            LoadSingle(whereclause, otemp);
        }


        public void LoadSingle(string whereclause, List<IntelWebField> filterparams)
        {
            if (!this.User.loggedIn) { return; }
            OleDbCommand tcommand;
            bool ReadOnly = false;
            if (commandType == System.Data.CommandType.StoredProcedure)
            {
                tcommand = new OleDbCommand(this.tableName, this.connection());
            }
            else
            {
                ReadOnly = !this.User.inTrans();
                tcommand = new OleDbCommand(this.FullSelectStatement(whereclause, ""), this.connection());
            }
            tcommand.CommandType = commandType;
            if (filterparams != null)
            {
                foreach (IntelWebField apfield in filterparams)
                {
                    tcommand.Parameters.Add(apfield.parameter);
                }
            }
            if (!ReadOnly && (this.User.inTrans()))
            {
                tcommand.Transaction = this.User.dbTrans();
            }
            OleDbDataReader oread = tcommand.ExecuteReader();
            tcommand.Parameters.Clear();
            oread.Read();

            this.Construct(oread, this);

            oread.Close();
        }

        public IntelWebObject getSingle()
        {
            string whereclause = "";
            foreach (IntelWebField x in this.changedFields)
            {
                whereclause += x.name + " = ? and ";
            }
            if (whereclause != "")
            {
                whereclause = whereclause.Substring(0, whereclause.Length - 4);
            }
            return getSingle(whereclause, this.changedFields);
        }

        public void LoadSingle()
        {
            if (!this.User.loggedIn) { return; }
            string whereclause = "";
            foreach (IntelWebField x in this.changedFields)
            {
                whereclause += x.name + " = ? and ";
            }
            if (whereclause != "")
            {
                whereclause = whereclause.Substring(0, whereclause.Length - 4);
            }
            LoadSingle(whereclause, this.changedFields);
        }

        private string FullSelectStatement(string whereclause, string sortorder)
        {
            string tempstring = this.SelectStatement;
            SimpleSQLParser ssp = new SimpleSQLParser(this.SelectStatement);
            if (whereclause != "")
            {
                ssp.addPart(whereclause, SimpleSQLParser.SQLPartType.where);

            }
            if (sortorder != "")
            {
                ssp.addPart(sortorder, SimpleSQLParser.SQLPartType.orderby);

            }

            return ssp.SQL;
        }

        public override string ToString()
        {           
            StringBuilder o = new StringBuilder();
            foreach (IntelWebField x in this.FieldList)
            {
                o.AppendLine(x.name + " [" + x.type.ToString() + "] = " + ((DBNull.Equals(x.fieldValue, null) ? "{null}" : x.fieldValue.ToString())));
            }
            o.AppendLine("isDirty = " + this.isDirty.ToString());
            o.AppendLine("isNew = " + this.isNew.ToString());
            o.AppendLine("isDeleted = " + this.isDeleted.ToString());
            o.AppendLine("Current User = " + this.User.userName);
            if (this.lastDBError != null)
            {
                o.AppendLine("Last Database Error = " + this.lastDBError.ToString());
            }

            return o.ToString();
        }

        private void Construct(OleDbDataReader oReader, object myobj)
        {
            PropertyInfo t = null;
            object tvalue = new object();
            Type myType = myobj.GetType();
            ObjectRights objectRights = new ObjectRights();
            t = myType.GetProperty("constructing");
            t.SetValue(myobj, true, null);
            if (oReader.HasRows)
            {
                List<IntelWebField> tempList;
                if (this.selectFieldsList.Count > 0)
                {
                    tempList = this.selectFieldsList;
                }
                else
                {
                    tempList = this.FieldList;
                }

                t = myType.GetProperty("isNew");
                t.SetValue(myobj, false, null);
                foreach (IntelWebField x in tempList)
                {
                        t = myType.GetProperty(x.name);
                        tvalue = oReader.GetValue(oReader.GetOrdinal(x.name));
                        if (DBNull.Equals(DBNull.Value, tvalue))
                        {
                            tvalue = null;
                        }
                        t.SetValue(myobj, tvalue, null);
                }
                if (_hasPK) { setPK(myobj); }
                t = myType.GetProperty("isDirty");
                t.SetValue(myobj, false, null);
            }
            else
            {
                t = myType.GetProperty("isNew");
                t.SetValue(myobj, true, null);
                t = myType.GetProperty("isDirty");
                t.SetValue(myobj, true, null);
            }
            PropertyInfo tprop = myType.GetProperty("constructing");
            tprop.SetValue(myobj, false, null);

        }

        public bool constructing
        {
            get { return _constructing; }
            set { _constructing = value; }

        }

        public string SelectStatement
        {
            get
            {
                if (this.SQL != "")
                {
                    return this.SQL;

                }
                else
                {
                    string tstring = this.selectFields;
                    return "Select " + tstring + " from [" + this.tableName + "]";
                }
            }
        }
        public bool isDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        public string SQL
        {
            get { return _sql; }
            set { _sql = value; }
        }


        public void beginTrans()
        {
            this.User.beginTrans();
        }

        public void commitTrans()
        {
            this.User.commitTrans();
        }

        public void rollbackTrans()
        {
            this.User.rollbackTrans();
        }

        public OleDbConnection connection()
        {
            return this.User.dbConnection();
        }

        [DataMember]
        public IntelWebField primaryKey
        {
            set { _primarykey = value; }
            get { return _primarykey; }
        }

        public List<IntelWebField> defaultFields
        {
            get
            {
                List<IntelWebField> tlist = new List<IntelWebField>();
                foreach (IntelWebField x in _fields)
                {
                    if (x.version == System.Data.DataRowVersion.Default)
                    {
                        tlist.Add(x);
                    }
                }
                return tlist;
            }
        }

        public bool isDirtyField(string fieldname)
        {
            foreach (IntelWebField tparam in this.changedFields)
            {
                if (tparam.name.Equals(fieldname, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public List<IntelWebField> changedFields
        {
            get
            {
                List<IntelWebField> tlist = new List<IntelWebField>();
                foreach (IntelWebField x in _fields)
                {
                    if (x.version == System.Data.DataRowVersion.Proposed)
                    {
                        tlist.Add(x);
                    }

                }
                return tlist;
            }
        }

        public void applyTo(IntelWebObject applyToObject, List<String> excludeFields)
        {
            var ApplyToType = applyToObject.GetType();
            foreach (var fld in this.FieldList)
            {
                if (excludeFields == null || !excludeFields.Exists(x => x.Equals(fld.name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var destFieldObj = applyToObject.Field(fld.name);
                    if (destFieldObj != null)
                    {
                        var prop = ApplyToType.GetProperty(destFieldObj.name);
                        if (prop != null)
                        {
                            prop.SetValue(applyToObject, fld.fieldValue, null);
                        }
                    }
                }
            }
        }

        public virtual void onFieldChanged(string fieldname, object oldValue, object newValue)
        {
            //do nothing - only to be used in derived objects
        }

        public void addChange(string fieldName, object fieldValue)
        {
            IntelWebField x = this.Field(fieldName);
            string overrideValue = null;
            if (this.constructing)
            {
                x.fieldValue = fieldValue;
                if (!this.isNew && x.version == System.Data.DataRowVersion.Default)
                {
                    x.version = System.Data.DataRowVersion.Current;
                }
            }
            else
            {
                if (!DBNull.Equals(x.fieldValue, fieldValue) && !(x.type == OleDbType.Char && DBNull.Equals(((string)(x.fieldValue ?? "")).PadRight(x.size), ((string)(fieldValue ?? "")).PadRight(x.size))))
                {
                    x.version = System.Data.DataRowVersion.Proposed;
                    object oldValue;
                    if (overrideValue != null)
                    {
                        oldValue = overrideValue;
                        x.fieldValue = overrideValue;
                    }
                    else
                    {
                        oldValue = fieldValue;
                        x.fieldValue = fieldValue;
                    }
                    this.isDirty = true;
                }
            }
        }

        public virtual string friendlySingluarName()
        {
            return this.tableName;
        }

        public virtual string friendlyPluralName()
        {
            return this.tableName;
        }

        public virtual List<ApplicationException> validateRecord()
        {
            List<ApplicationException> tlist = new List<ApplicationException>();
            foreach (IntelWebField f in FieldList)
            {
                if (!f.nullable)
                {
                    if (f.fieldValue == null)
                    {

                        tlist.Add(new ApplicationException("The field " + f.name + " cannot be blank."));
                    }
                }
            }

            return null;
        }

        public List<IntelWebField> FieldList
        {   get { return _fields; }
            set
            {
                _fields = value;
                object tvalue;
                if (loadObjectDefaults)
                {
                    foreach (IntelWebField x in _fields)
                    {
                        if (x.version == System.Data.DataRowVersion.Default)
                        {
                            PropertyInfo tt = this.GetType().GetProperty(x.name);
                            tvalue = x.fieldValue;
                            if (DBNull.Equals(DBNull.Value, tvalue))
                            {
                                tvalue = null;
                            }
                            tt.SetValue(this, tvalue, null);
                        }
                    }
                }
            }
        }

        [DataMember]
        public string tableName
        {
            get { return _tablename; }
            set { _tablename = value; }
        }

        public virtual bool save()
        {
            string tsql = "";
            string sql2 = "";
            string finalsql = "";

            if (!this.isDirty)
            {
                return true;
            }
            if (this.primaryKey == null)
            {
                AddSaveError("The object has no primary key set.");
                return false;
            }
            _saveerrors = validateRecord();
            if (_saveerrors != null)
            {
                return false;
            }

            foreach (IntelWebField x in this.changedFields)
            {
                tsql += x.name;
                if (!this.isNew)
                {
                    tsql += " = ?";
                }
                else
                {
                    sql2 += "?,";
                }
                tsql += ",";
            }

            if (this.isNew)
            {
                foreach (IntelWebField x in this.defaultFields)
                {
                    if (!x.isAutoGenerated)
                    {
                        if (x.name.Equals("last_modified_by", StringComparison.InvariantCultureIgnoreCase))
                        {
                            x.fieldValue = this.User.userName;
                        }
                        tsql += x.name + ",";
                        sql2 += "?,";
                    }
                }
            }
            if (sql2 != "") { sql2 = sql2.Substring(0, sql2.Length - 1); }
            if (tsql != "") { tsql = tsql.Substring(0, tsql.Length - 1); }
            if (this.isNew)
            {
                finalsql = "Insert into [" + this.tableName + "] (" + tsql + ") values (" + sql2 + ")";
            }
            else
            {
                finalsql = "Update [" + this.tableName + "] set " + tsql;
            }


            OleDbCommand tCommand = new OleDbCommand(finalsql, this.connection());
            foreach (IntelWebField webfield in this.changedFields)
            {
                tCommand.Parameters.Add(webfield.parameter);
            }

            if (this.isNew)
            {
                foreach (IntelWebField webfield2 in this.defaultFields)
                {
                    //Console.Write("'" + (webfield2.fieldValue != null ? webfield2.fieldValue.ToString() : "") + "',");
                    tCommand.Parameters.Add(webfield2.parameter);
                }
            }
            else
            {
                finalsql += " where " + this.primaryKey.name + " = ?";
                tCommand.Parameters.Add(this.primaryKey.parameter);
                tCommand.CommandText = finalsql;
            }

            try
            {
                if (this.User.inTrans())
                {
                    tCommand.Transaction = this.User.dbTrans();
                }
                int returnvalue = tCommand.ExecuteNonQuery();
                tCommand.Parameters.Clear();

                setPK(this);

                this.afterSave();
                this.isDirty = false;
                this.isNew = false;

                return true;
            }
            catch (OleDbException ex)
            {
                tCommand.Parameters.Clear();
                _lastDBError = ex;
                AddSaveError(ex.Message);

                return false;
            }

        }

        public virtual bool delete()
        {
            string finalsql = "Delete from [" + this.tableName + "] where " + this.primaryKey.name + " = ?";
            OleDbCommand tCommand = new OleDbCommand(finalsql, this.connection());
            tCommand.Parameters.Add(this.primaryKey.parameter);
            if (this.User.inTrans())
            {
                tCommand.Transaction = this.User.dbTrans();
            }
            try
            {
                tCommand.ExecuteNonQuery();
                this.afterDelete();
                this.isDirty = false;
                this.isDeleted = true;
                return true;
            }
            catch (Exception ex)
            {
                AddSaveError("Delete was not succesfull --> " + ex.Message);
                return false;
            }

        }

        [DataMember]
        public bool isNew
        {
            get { return _isNew; }
            set { _isNew = value; }

        }

        public bool isDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (!value)
                {
                    foreach (IntelWebField x in this.changedFields)
                    {
                        x.version = System.Data.DataRowVersion.Current;
                        x.previousValue = x.fieldValue;
                    }
                }

            }
        }

        private string fixFieldValueForOutput(object value, IntelWebField field)
        {
            if (field.parameter.DbType.Equals(System.Data.DbType.Date))
            {
                if (value == null || value.Equals(DateTime.MinValue))
                {
                    return "(Null)";
                }
                DateTime m = (DateTime)value;
                return m.Month.ToString() + "/" + m.Day.ToString() + "/" + m.Year.ToString();
            }
            else
            {
                return (value ?? "").ToString();
            }
        }

        public string ChangedFieldsDescription()
        {
            int changecount = 0;

            StringBuilder sb = new StringBuilder();

            foreach (IntelWebField changed in this.changedFields)
            {
                string fieldname = changed.name;
                if (changed.fieldValue != null)
                {
                    if (!this.isNew)
                    {
                        sb.AppendLine(fieldname + ": " + this.fixFieldValueForOutput(changed.previousValue, changed) + " » " + this.fixFieldValueForOutput(changed.fieldValue, changed));
                    }
                    else
                    {
                        sb.AppendLine(fieldname + ": " + this.fixFieldValueForOutput(changed.fieldValue, changed));
                    }
                    changecount += 1;

                }
                else
                {
                    sb.AppendLine(changed.name + ": " + this.fixFieldValueForOutput(changed.previousValue, changed) + " » (Null)");
                    changecount += 1;
                }

            }
            if (changecount == 0)
            {
                sb.Append("No changes");
            }
            return sb.ToString();
        }

        public string ToRecordString()
        {
            StringBuilder result = new StringBuilder();

            foreach (IntelWebField fld in this.FieldList)
            {
                if (!DBNull.Equals(fld.fieldValue, null))
                {
                    result.AppendLine(fld.name + ": " + fld.fieldValue.ToString());
                }
            }
            return result.ToString();
        }

    }
}
