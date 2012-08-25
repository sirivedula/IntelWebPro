using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using IntelWeb;
using IntelWeb.IntelWebObjects;

namespace IntelWebSite
{
    public class WebGridTable
    {
        public enum EditType { modal, inline }
        private WebGrid _grid;
        private string _tableCSSClass = "webgrid";
        private string _tableId = "webdatatable";
        private List<extraColumn> _extraColumns = new List<extraColumn>();
        public string UIHeaderText { get; set; }
        public EditType formEditType { get; set; }

        public WebGridTable()
        {
            this.formEditType = EditType.inline;
        }
        public string tableId
        {
            get{return _tableId;}
            set{_tableId = value;}
        }
        public string tableCSSClass
            {
                get { return _tableCSSClass; }
                set { _tableCSSClass = value; }
            }

        public WebGrid Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }
        
        public List<extraColumn> extraColumns
        {
            get { return _extraColumns; }
            set { _extraColumns = value; }
        }
        private string replaceGridAndRecordParams(string subject,string gridname, int recordIndex)
        {
            string result = subject;
            result = Regex.Replace(result,@"\[gridName\]","'" + JSUtil.EnquoteJS(gridname) + "'",RegexOptions.IgnoreCase | RegexOptions.Multiline);
            result = Regex.Replace(result,@"\[recordIndex\]", recordIndex.ToString() ,RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return result;
        }

        public string tableUI()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this.UIHeaderText))
            {
                sb.Append(@"<table align=""center"" class=""titlegrid""><thead class=""gridtitle""><tr><td>" + HttpUtility.HtmlEncode(this.UIHeaderText) + "</td></tr></table>");
            }
            sb.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\" id=\"" + HttpUtility.HtmlEncode(this.tableId) + "\" class=\"" + HttpUtility.HtmlEncode(this.tableCSSClass) + "\">");
            int allRowId = -1;
            int displayedRowCount = -1;

            sb.AppendLine("<thead class=\"gridheader\"><tr>");
            foreach (var headerField in this.Grid.headerFields())
            {
                sb.Append("<th>" + HttpUtility.HtmlEncode(headerField.displayname) + "</th>");
            }
            sb.Append("<th>Actions</th>");
            sb.AppendLine("</tr></thead><tbody class=\"gridbody\">");

            int rowID = -1;
            if (this.Grid.data != null)
            {
                foreach (IntelWebObject record in this.Grid.data)
                {
                    rowID++;
                    //if (rowID == 100) { break; }    //load upto 100 records for now
                    allRowId++;
                    displayedRowCount++;
                    string hideRecordStyle = "";
                    sb.AppendLine("<tr id=\"" + this.Grid.gridName + "_row_" + rowID.ToString() + "\"" + (((displayedRowCount % 2) == 0) ? " class=\"gridodd\"" : "") + hideRecordStyle + ">");
                    foreach (var headerField in this.Grid.headerFields())
                    {
                        sb.Append("<td>");
                        string fieldValue;
                        if (headerField.formatFunction != null)
                        {
                            fieldValue = headerField.formatFunction.Invoke(record).ToString();
                        }
                        else
                        {
                            var apfield = record.Field(headerField.fieldName);
                            fieldValue = (apfield.fieldValue ?? "").ToString();
                        }
                        sb.Append(HttpUtility.HtmlEncode(fieldValue));
                        sb.Append("</td>");
                    }
                    foreach (extraColumn ec in this.extraColumns)
                    {
                        sb.Append("<td>" + replaceGridAndRecordParams((ec.fieldTemplate.formatFunction.Invoke(record) ?? "").ToString(), this.Grid.gridName, rowID) + "</td>");
                    }
                    sb.AppendLine("</tr>");
                }
            }
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<div style=\"margin-top:10px;\"><a href=\"javascript:void(0);\" onclick=\"addNew();\"><img style=\"vertical-align:middle;\" src=\"Images/add.png\" alt=\"addNew\" />Add New</a></div>");
            return sb.ToString();
        }

        public string tableScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$().ready(function(){var myGrid = webGrid.findFirst(webGrid.grids, function(obj) { return obj.name == '" + JSUtil.EnquoteJS(this.Grid.gridName) + "';});");
            sb.AppendLine("myGrid.tableId = '" + JSUtil.EnquoteJS(this.tableId) + "' ;");
            StringBuilder ecBuilder = new StringBuilder();
            foreach (extraColumn ec in this.extraColumns)
            {
                ecBuilder.Append(ec.fieldTemplate.toJS("atBeginning", ec.atBeginning ? "true" : "false"));
                ecBuilder.Append(",");
            }
            if (ecBuilder.Length > 0) { ecBuilder.Remove(ecBuilder.Length - 1, 1); }
            sb.AppendLine("myGrid.extraColumns = [" + ecBuilder.ToString() + "];");
            sb.AppendLine("})</script>");
            return sb.ToString();
        }

        public class extraColumn
        {
            public string name { get; set; }
            public bool atBeginning { get; set; }
            public DisplayField fieldTemplate {get;set; }

            public extraColumn()
            {
                this.atBeginning = false;
            }
        }

    }
}
