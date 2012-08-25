using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace IntelWeb.IntelWebObjects
{
    public class IntelWebDataConverter
    {
        public static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
        public static string GenericArgType(Type theType)
        {
            Type[] t = theType.GetGenericArguments();
            if (t.Length > 0)
            {
                return t[0].Name;
            }
            return "";
        }
        public static object toType(string val, Type ty,string contextErrorInformation)
        {
            string myTypeName = ty.Name.ToUpper();
            Type ovType = ty;
            if (IsNullableType(ty))
            {
                myTypeName = GenericArgType(ty).ToUpper();
            }
            if (IsNullableType(ty))
            {
                if (String.IsNullOrEmpty(val))
                {
                    return null;
                }
                NullableConverter nc = new NullableConverter(ty);
                ovType = nc.UnderlyingType;
            }
            switch (myTypeName)
            {
                case "BOOLEAN":
                    switch (val.ToUpper())
                    {

                        case "1":
                        case "-1":
                        case "T":
                        case "TRUE":
                        case "Y":
                        case "YES":
                            return true;
                        default:
                            return false;
                    }
                case "DATETIME":
                    if (val.Equals(""))
                    {
                        return DateTime.MinValue;
                    }
                    break;
                case "BYTE":
                case "DECIMAL":
                case "INT32":
                case "INT16":
                case "SBYTE":
                case "SHORT":
                case "USHORT":
                case "FLOAT":
                case "DOUBLE":
                case "LONG":
                case "ULONG":
                    if (val.Equals(""))
                    {
                        return Convert.ChangeType("0", ovType);
                    }
                    break;
                case "GUID":
                    return new Guid(val);
            }
            try
            {
                return Convert.ChangeType(val, ovType);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + " - " + contextErrorInformation);
            }
        }
    }
}
