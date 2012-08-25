using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Globalization;
using IntelWeb;
using IntelWeb.IntelWebObjects;

namespace IntelWebSite
{
    public class WebGrid
    {
        private List<IntelWebObject> _data;
        private NameValueCollection _form;
        private List<ApplicationException> _loadErrors = new List<ApplicationException>();
        public string afterSaveErrors { get; set; }
        public string updateDataErrors { get; set; }
        private CurrentUser _user;
        private IntelWebObject _protoObject;
        private IntelWebField _pkfield;
        private List<DisplayField> _fields = new List<DisplayField>();
        private string _gridName;
        private WebGridTable _gridTable = new WebGridTable();
        private string _uiFormShowJS;
        private string _uiFormName;
        private string _ajaxURL = "SaveGrid.aspx";
        private string _objectId; 
        private string _afterSubmitJS;
        private string _onAddJS;
        public List<IntelWebObject> deleteQueue = new List<IntelWebObject>();
        public List<IntelWebField> serverOnlyFields = new List<IntelWebField>();

        public string objectType { get; set; }
        public ControlCollection controls { get; set; }
        public string onAddJS
        {
            get { return _onAddJS; }
            set { _onAddJS = value; }
        }
        public string afterSubmitJS
        {
            get { return _afterSubmitJS; }
            set { _afterSubmitJS = value; }
        }
        public string objectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }
        public string ajaxURL
        {
            get { return _ajaxURL; }
            set { _ajaxURL = value; }
        }
        public string uiFormName
        {
            get { return _uiFormName; }
            set { _uiFormName = value; }
        }
        public string uiFormShowJS
        {
            get { return _uiFormShowJS; }
            set { _uiFormShowJS = value; }
        }
        public IntelWebObject protoObject
        {
            get { return _protoObject; }
            set { _protoObject = value; }
        }
        public List<DisplayField> headerFields()
        {
            var result = new List<DisplayField>();
            result.AddRange(this.fields.FindAll(x => x.isHeaderField));
            return result;
        }
        public WebGridTable gridTable
        {
            get
            {
                return _gridTable;
            }
            set
            {
                _gridTable = value;
            }
        }
        public string gridName
        {
            get { return _gridName; }
            set { _gridName = value; }
        }

        public List<DisplayField> fields
        {
            get { return _fields; }
            set { _fields = value; }
        }
        public List<ApplicationException> loadErrors
        {
            get { return _loadErrors; }
        }
        public CurrentUser User
        {
            get { return _user; }
            set { _user = value; }
        }

        public NameValueCollection form
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
            }
        }

        public List<IntelWebObject> data
        {
            get { return _data; }
            set { _data = value; }
        }

        public WebGrid(string type, CurrentUser myUser, string myGridName)
        {
            this.User = myUser;
            this.objectType = type;
            _protoObject = this.User.protoObject(this.objectType);
            _pkfield = _protoObject.primaryKey;
            _gridName = myGridName;
        }

        public List<DisplayField> fieldsFromList(List<string> fieldNames)
        {
            var result = new List<DisplayField>();
            foreach (var fieldName in fieldNames)
            {
                var fld = _protoObject.Field(fieldName);
                if (fld == null)
                {
                    throw new ApplicationException("There is no field in the " + _protoObject.tableName + " named [" + fieldName + "]");
                }
                DisplayField df = new DisplayField(this, fld.name.ToLower(), fld.name, null, false, fld.name.ToLower());
                result.Add(df);
            }
            return result;
        }

        public int submittedRecordCount
        {
            get
            {
                decimal decimalresult = 0;
                if (this.form.Get(this._gridName + "_recordCount") != null)
                {
                    decimal.TryParse(this.form.Get(this._gridName + "_recordCount"), out decimalresult);
                }
                return Convert.ToInt32(decimalresult);
            }
        }

        public IntelWebField primaryKey
        {
            get
            {
                return _pkfield;
            }
        }

        public object convertStringToType(string value, Type type)
        {
            var convertedVal = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            if (DBNull.Equals(DBNull.Value, convertedVal))
            {
                return null;
            }
            return convertedVal;
        }

        public bool UpdateData()
        {
            this.updateDataErrors = "";
            int recCnt = this.submittedRecordCount;
            var myType = _protoObject.GetType();
            for (int counter = 0; counter < recCnt; counter++)
            {
                string keyvalue = this.form.Get(this._gridName + "_pk_" + counter.ToString());

                string isNewString = this.form.Get(this._gridName + "_isNew_" + counter.ToString());
                string isDeletedString = this.form.Get(this._gridName + "_isDeleted_" + counter.ToString());

                IntelWebObject tpObj = null;

                if (!String.IsNullOrEmpty(isNewString) && isNewString == "true")
                {
                    string tempPK = this.form.Get(this._gridName + "_temppk_" + counter.ToString());

                    tpObj = this.User.protoObject(this.objectType);
                    if (_data == null) { _data = new List<IntelWebObject>(); }
                    _data.Add(tpObj);
                }
                else
                {
                    if (_data != null)
                    {                        
                        tpObj = _data.Find(x => x.primaryKey.fieldValue.Equals(WebDataConverter.toType(keyvalue, typeof(Int32))));
                    }
                }
                if (tpObj != null)
                {
                    if (!String.IsNullOrEmpty(isDeletedString) && isDeletedString == "true")
                    {
                        deleteQueue.Add(tpObj);
                    }
                    foreach (DisplayField field in this.fields)
                    {
                        if (field.submitField)
                        {
                            string formFieldName = this._gridName + "_r" + counter.ToString() + "_" + field.fieldName;
                            var fieldValue = this.form.Get(formFieldName);
                            var tProp = myType.GetProperties().FirstOrDefault(x => x.Name.Equals(field.fieldName, StringComparison.InvariantCultureIgnoreCase));
                            try
                            {
                                var convertedVal = WebDataConverter.toType(fieldValue, tProp.PropertyType);
                                tProp.SetValue(tpObj, convertedVal, null);
                            }
                            catch (Exception)
                            {
                                updateDataErrors += "The value [" + fieldValue + "] that was submitted for the field [" + tProp.Name + "] was not a valid.\n";
                            }
                        }
                    }
                    foreach (IntelWebField wpField in this.serverOnlyFields)
                    {
                        var tProp = myType.GetProperties().FirstOrDefault(x => x.Name.Equals(wpField.name, StringComparison.InvariantCultureIgnoreCase));
                        tProp.SetValue(tpObj, wpField.fieldValue, null);
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public string gridScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.Append("webGrid.grids.push({name:'" + JSUtil.EnquoteJS(this.gridName) + "'");
            sb.Append(",objectId:'" + JSUtil.EnquoteJS(this.objectId) + "'");
            sb.Append(",tableId:''");
            sb.Append(",ajaxURL:'" + JSUtil.EnquoteJS(this.ajaxURL) + "'");
            sb.Append(",fieldDefs:" + this.gridFieldsJS());
            sb.Append(",extraColumns:[]");

            sb.Append(",data: " + this.dataJS() + "");
            if (!String.IsNullOrEmpty(this.afterSubmitJS))
            {
                sb.Append(",afterSubmit: " + this.afterSubmitJS);
            }
            sb.Append(",showUIForm: " + ((this.uiFormShowJS == null) ? "undefined" : this.uiFormShowJS) + "});");

            return sb.ToString();
        }

        public string dataJS()
        {
            StringBuilder sb = new StringBuilder();
            int counter = -1;

            if (this.data != null)
            {
                foreach (IntelWebObject wbObj in this.data)
                {
                    counter++;
                    sb.Append("{_id:" + counter.ToString() + ",_pk:" + JSUtil.SerializeJSProperty(wbObj.primaryKey));
                    sb.Append(",fields:{");
                    foreach (DisplayField field in this.fields)
                    {
                        var myValue = wbObj.Field(field.fieldName);
                        if (myValue != null)
                        {
                            if (field.formatFunction != null)
                            {
                                sb.Append("'" + JSUtil.EnquoteJS(field.fieldName) + "':" + JSUtil.SerializeJSProperty(field.formatFunction.Invoke(wbObj)));
                            }
                            else
                            {
                                if (myValue.fieldValue == null)
                                {
                                    sb.Append("'" + JSUtil.EnquoteJS(field.fieldName) + "':null");
                                }
                                else
                                {
                                    sb.Append("'" + JSUtil.EnquoteJS(field.fieldName) + "':" + JSUtil.SerializeJSProperty(myValue));
                                }
                            }
                            sb.Append(",");
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine("}}");
                    sb.Append(",");
                }
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return "[" + sb.ToString() + "]";
        }

        public bool LoadData()
        {
            int myCount = this.submittedRecordCount;
            List<IntelWebField> primaryKeys = new List<IntelWebField>(); 
            StringBuilder sb = new StringBuilder();
            int recCnt = this.submittedRecordCount;
            for (int counter = 0; counter < recCnt; counter++)
            {
                string isNewString = this.form.Get(this._gridName + "_isNew_" + counter.ToString());
                if (String.IsNullOrEmpty(isNewString) || isNewString != "true")
                {

                    sb.Append("?,");

                    string keyvalue = this.form.Get(this._gridName + "_pk_" + counter.ToString());
                    object convertedKey;
                    switch (this.primaryKey.type)
                    {
                        case System.Data.OleDb.OleDbType.Guid:
                            convertedKey = new Guid(keyvalue);
                            break;
                        case System.Data.OleDb.OleDbType.Integer:
                            convertedKey = int.Parse(keyvalue);
                            break;
                        default:
                            convertedKey = keyvalue;
                            break;
                    }
                    IntelWebField paramFld = new IntelWebField(this._gridName + "_pk_" + counter.ToString(), this.primaryKey.type, this.primaryKey.size, 0, 0, System.Data.DataRowVersion.Current, false, convertedKey);
                    primaryKeys.Add(paramFld);
                }
            }

            try
            {
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    IntelWebObject tobj = this.User.protoObject(objectType);

                    _data = tobj.Load(this.primaryKey.name + " in (" + sb.ToString() + ")", "", primaryKeys);
                }
            }
            catch (ApplicationException ex)
            {
                this.loadErrors.Add(ex);
                return false;
            }
            return true;
        }

        public string gridFieldsJS()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DisplayField df in this.fields)
            {
                sb.Append(df.toJS());
                sb.Append(",");
            }
            if (sb.Length > 0) { sb.Remove(sb.Length - 1, 1); }
            return "[" + sb.ToString() + "]";
        }

    }

    public class DisplayField
    {
        private Func<IntelWebObject, object> _formatFunction;

        private string _displayName;
        private string _fieldName;
        private string _jsFormatFunction;
        private string _jsValidationFunction;
        private string _defaultValue;
        private int _maxLength;
        private bool _allowNull;
        private bool _isHeaderField;
        private bool _submitField = true;
        private string _calculation;
        private IntelWebField _defaultField;
        private string _formFieldName;

        public DisplayField()
        {

        }

        public DisplayField(WebGrid myGrid, string fieldName, string myDisplayName, string myJSFormatFunction, bool myIsHeaderField, string myFormFieldName)
        {
            if (myGrid.controls != null)
            {
                var control = GridDefUtil.getControl(myGrid.controls, myFormFieldName);
                if (control == null)
                {
                    _formFieldName = myFormFieldName;
                }
                else
                {
                    _formFieldName = control.ClientID;
                }
            }
            else
            {
                _formFieldName = myFormFieldName;
            }
            _jsFormatFunction = myJSFormatFunction;
            _displayName = myDisplayName;
            var myField = myGrid.protoObject.Field(fieldName);
            if (myField == null)
            {
                throw new ApplicationException("The field [" + fieldName + "] does not exist as a field in the prototype for [" + myGrid.protoObject.GetType().Name + "]");
            }
            _fieldName = fieldName;

            _allowNull = myField.nullable;
            if (myField.type == System.Data.OleDb.OleDbType.VarChar || myField.type == System.Data.OleDb.OleDbType.VarWChar || myField.type == System.Data.OleDb.OleDbType.Char || myField.type == System.Data.OleDb.OleDbType.LongVarChar || myField.type == System.Data.OleDb.OleDbType.LongVarWChar)
            {
                _maxLength = myField.parameter.Size;
            }
            else
            {
                _maxLength = 0;
            }
            _isHeaderField = myIsHeaderField;
            if (myField.version == System.Data.DataRowVersion.Default)
            {
                _defaultField = myField;

            }
        }
        public string formFieldName
        {
            get { return _formFieldName; }
            set { _formFieldName = value; }
        }
        public bool submitField
        {
            get { return _submitField; }
            set { _submitField = value; }
        }
        public string calculation
        {
            get { return _calculation; }
            set { _calculation = value; }
        }

        public bool isHeaderField
        {
            get { return _isHeaderField; }
            set { _isHeaderField = value; }
        }
        public string jsValidationFunction
        {
            get { return _jsValidationFunction; }
            set { _jsValidationFunction = value; }
        }
        public string defaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        public int maxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
        public bool allowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }
        public string jsFormatFunction
        {
            get { return _jsFormatFunction; }
            set { _jsFormatFunction = value; }
        }
        public string displayname
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public string fieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        public Func<IntelWebObject, object> formatFunction
        {
            get { return _formatFunction; }
            set { _formatFunction = value; }
        }
        public string toJS(string extraPropertyName, string javaEscapedExtraPropertyValue)
        {
            return "{name:'" + JSUtil.EnquoteJS(this.fieldName) +
               "',displayName:'" + JSUtil.EnquoteJS(this.displayname) +
               "',formatter:" + ((this.jsFormatFunction != null) ? "function(obj){" + this.jsFormatFunction + "}" : "null") +
               ",maxLength:" + this.maxLength.ToString() +
               ",allowBlank:" + (this.allowNull ? "true" : "false") +
               ",defaultValue:" + ((_defaultField != null) ? JSUtil.SerializeJSProperty(_defaultField) : "'" + JSUtil.EnquoteJS(this.defaultValue) + "'") +
               ",validation:" + ((this.jsValidationFunction != null) ? "function(obj){" + this.jsValidationFunction + "}" : "null") +
               ",isHeaderField:" + (this.isHeaderField ? "true" : "false") +
               ",submitField:" + (this.submitField ? "true" : "false") +
               ",formFieldName:'" + JSUtil.EnquoteJS(this.formFieldName) + "'" +

               ((extraPropertyName != null) ? "," + extraPropertyName + ":" + javaEscapedExtraPropertyValue : "") +
               "}";
        }
        public string toJS()
        {
            return this.toJS(null, null);
        }
    }

}
