using System;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace IntelWeb
{
    public class IntelWebDataTypeMapper
    {
        public static MapInfo map(string sqltype)
        {
            switch (sqltype)
            {
                case "image":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.LongVarBinary, "byte[]");
                case "text":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.LongVarChar, "string");
                case "uniqueidentifier":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Guid, "Guid");
                case "tinyint":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.TinyInt, "byte");
                case "smallint":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.SmallInt, "Int16");
                case "int":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Integer, "Int32");
                case "smalldatetime":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.DBDate, "DateTime");
                case "real":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Double, "double");
                case "money":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Currency, "decimal");
                case "datetime":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.DBDate, "DateTime");
                case "float":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.VarNumeric, "decimal");
                case "sql_variant":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Variant, "object");
                case "ntext":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.LongVarWChar, "string");
                case "bit":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Boolean, "bool");
                case "decimal":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Decimal, "decimal");
                case "numeric":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Decimal, "decimal");
                case "smallmoney":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Currency, "decimal");
                case "bigint":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.BigInt, "Int64");
                case "varbinary":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.VarBinary, "byte");
                case "varchar":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.VarChar, "string");
                case "binary":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Binary, "byte");
                case "char":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Char, "string");
                case "timestamp":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.DBTimeStamp, "DateTime");
                case "nvarchar":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.VarWChar, "string");
                case "nchar":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.WChar, "string");
                case "xml":
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.LongVarChar, "string");
                default:
                    return MapInfo.make(sqltype, System.Data.OleDb.OleDbType.Empty, "UNKNOWN!!!");
            }
        }
    }
    public class MapInfo
    {
        public string sqlType;
        public OleDbType oleDBType;
        public string declareType;

        public static MapInfo make(string ssqltype, System.Data.OleDb.OleDbType soletype, string sdeclaretype)
        {
            MapInfo m = new MapInfo();
            m.sqlType = ssqltype;
            m.oleDBType = soletype;
            m.declareType = sdeclaretype;
            return m;
        }

    }
}
