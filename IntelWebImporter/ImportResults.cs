using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace IntelWebImporter
{
    public class ImportedItem
    {
        private int _lineNumber;
        private bool _isNew;
        private string _objectname;
        private string _importedDetails;

        public static string toTableRowHeader()
        {
            return "<tr><th>Line #</th><th>Item Type</th><th>Action</th><th>Details</th></tr>";
        }

        public string toTableRow(string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr class=\"" + className + "\"><td>" + HttpUtility.HtmlEncode(((int)(this.lineNumber + 1)).ToString()) + "</td>");
            sb.Append("<td>" + HttpUtility.HtmlEncode(this.objectName) + "</td>");
            sb.Append("<td>" + HttpUtility.HtmlEncode(this.isNew ? "Added" : "Updated") + "</td>");
            sb.Append("<td>" + HttpUtility.HtmlEncode(this.importedDetails).Replace("\r\n", "<br />") + "</td>");
            sb.Append("</tr>");
            return sb.ToString();

        }

        public ImportedItem(int mylinenumber, bool myisnew, string myobjectname, string mydetails)
        {
            _lineNumber = mylinenumber;
            _isNew = myisnew;
            _objectname = myobjectname;
            _importedDetails = mydetails;
        }
        public int lineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }
        public bool isNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }
        public string objectName
        {
            get { return _objectname; }
            set { _objectname = value; }
        }
        public string importedDetails
        {
            get { return _importedDetails; }
            set { _importedDetails = value; }
        }
    }

    public class ImportError
    {
        public static string sanitizeMessage(string unsanitizedMessage)
        {
            string resultString = Regex.Replace(unsanitizedMessage, @"[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ]*\.dbo\.", "");

            return resultString;
        }

        public enum Severity : int
        {
            warning = 1, critical = 2
        }

        private int _lineNumber;
        private string _errorMessage = "";
        private Severity _severity = Severity.warning;
        private string _extraInfo = "";
        private string _fieldFQR = "";

        public static string toTableRowHeader()
        {
            return "<tr><th>Line #</th><th>Item Type</th><th>Severity</th><th>Description</th><th>Extra Information</th></tr>";
        }
        public string toTableRow(string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr class=\"" + className + "\"><td>" + HttpUtility.HtmlEncode((this.lineNumber + 1).ToString()) + "</td>");
            sb.Append("<td>" + HttpUtility.HtmlEncode(this.fieldFQR ?? "") + "</td>");
            sb.Append("<td>" + (this.severity.Equals(Severity.warning) ? "Warning" : "Error") + "</td>");
            sb.Append("<td>" + HttpUtility.HtmlEncode(this.errorMessage ?? "") + "</td>");
            sb.Append("<td>" + HttpUtility.HtmlEncode(this.extraInfo ?? "") + "</td></tr>");
            return sb.ToString();
        }
        public int lineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }
        public string fieldFQR
        {
            get { return _fieldFQR; }
            set { _fieldFQR = value; }
        }
        public string errorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = sanitizeMessage(value); }
        }
        public Severity severity
        {
            get { return _severity; }
            set { _severity = value; }
        }
        public string extraInfo
        {
            get { return _extraInfo; }
            set { _extraInfo = sanitizeMessage(value); }
        }

    }
}
