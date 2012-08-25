using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using IntelWeb;

namespace IntelWebSite
{
    public static class JSUtil
    {
        public static string EnquoteJS(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }
            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>'))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        //t = "000" + Integer.toHexString(c); 
                        string tmp = new string(c, 1);
                        t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }

        public static string SerializeJSProperty(object FieldOrOther)
        {

            if (FieldOrOther is IntelWebField)
            {
                IntelWebField tField = (IntelWebField)FieldOrOther;

                if (tField.fieldValue == null)
                {
                    return "null";
                }
                switch (tField.type)
                {
                    case System.Data.OleDb.OleDbType.DBDate:
                    case System.Data.OleDb.OleDbType.Date:
                        var mydate = (DateTime)tField.fieldValue;
                        return "'" + mydate.Month.ToString() + "/" + mydate.Day.ToString() + "/" + mydate.Year.ToString() + "'";
                    case System.Data.OleDb.OleDbType.DBTime:
                    case System.Data.OleDb.OleDbType.DBTimeStamp:
                        var mydate2 = (DateTime)tField.fieldValue;
                        return "new Date(" + new TimeSpan(mydate2.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks).TotalMilliseconds.ToString() + ")";
                    case System.Data.OleDb.OleDbType.Boolean:
                        return ((Boolean)tField.fieldValue) ? "true" : "false";
                    case System.Data.OleDb.OleDbType.BigInt:
                    case System.Data.OleDb.OleDbType.Currency:
                    case System.Data.OleDb.OleDbType.Decimal:
                    case System.Data.OleDb.OleDbType.Double:
                    case System.Data.OleDb.OleDbType.Integer:
                    case System.Data.OleDb.OleDbType.Numeric:
                    case System.Data.OleDb.OleDbType.Single:
                    case System.Data.OleDb.OleDbType.SmallInt:
                    case System.Data.OleDb.OleDbType.TinyInt:
                    case System.Data.OleDb.OleDbType.UnsignedBigInt:
                    case System.Data.OleDb.OleDbType.UnsignedInt:
                    case System.Data.OleDb.OleDbType.UnsignedSmallInt:
                    case System.Data.OleDb.OleDbType.UnsignedTinyInt:
                    case System.Data.OleDb.OleDbType.VarNumeric:
                        return JSUtil.EnquoteJS(tField.fieldValue.ToString());
                    default:
                        return "'" + JSUtil.EnquoteJS(tField.fieldValue.ToString()) + "'";
                }
            }
            else
            {
                if (FieldOrOther == null)
                {
                    return "null";
                }
                if (FieldOrOther is Boolean) { return ((Boolean)FieldOrOther) ? "true" : "false"; }
                if (FieldOrOther is Decimal || FieldOrOther is int || FieldOrOther is Int16 || FieldOrOther is Int32 || FieldOrOther is Int64 || FieldOrOther is Double) { return FieldOrOther.ToString(); }
                if (FieldOrOther is Boolean?)
                {
                    if (((Boolean?)FieldOrOther).HasValue)
                    {
                        return ((Boolean?)FieldOrOther).Value ? "true" : "false";
                    }
                    else
                    {
                        return "null";
                    }
                }

                return "'" + JSUtil.EnquoteJS(FieldOrOther.ToString()) + "'";
            }
        }

    }
}
