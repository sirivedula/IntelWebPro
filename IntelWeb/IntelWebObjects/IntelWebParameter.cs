using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace IntelWeb
{

    public class IntelWebParameter
    {
        public IntelWebParameter()
        {

        }
        public static List<IntelWebField> List(IntelWebField p1)
        {
            List<IntelWebField> templist = new List<IntelWebField>();
            templist.Add(p1);
            return templist;
        }
        public static List<IntelWebField> List(IntelWebField p1, IntelWebField p2)
        {
            List<IntelWebField> templist = new List<IntelWebField>();
            templist.Add(p1);
            templist.Add(p2);
            return templist;
        }
        public static List<IntelWebField> List(IntelWebField p1, IntelWebField p2, IntelWebField p3)
        {
            List<IntelWebField> templist = new List<IntelWebField>();
            templist.Add(p1);
            templist.Add(p2);
            templist.Add(p3);
            return templist;
        }
        public static List<IntelWebField> List(IntelWebField p1, IntelWebField p2, IntelWebField p3, IntelWebField p4)
        {
            List<IntelWebField> templist = new List<IntelWebField>();
            templist.Add(p1);
            templist.Add(p2);
            templist.Add(p3);
            templist.Add(p4);
            return templist;
        }

        public static IntelWebField GetParam(string name, OleDbType type, int size, object value)
        {

            return new IntelWebField(name, type, size, 0, 0, DataRowVersion.Current, false, value);


        }
        public object[] List2_4(object p1, object p2,object p3,object p4)
        {
            List<IntelWebField> l = IntelWebParameter.List((IntelWebField)p1, (IntelWebField)p2, (IntelWebField)p3, (IntelWebField)p4);
            return IntelWebParameter.ParamListToObjectArray(l);
        }
        public object[] List2_3(object p1, object p2,object p3)
        {
            List<IntelWebField> l = IntelWebParameter.List((IntelWebField)p1, (IntelWebField)p2, (IntelWebField)p3);
            return IntelWebParameter.ParamListToObjectArray(l);
        }
        public object[] List2_2(object p1,object p2)
        {
            List<IntelWebField> l = IntelWebParameter.List((IntelWebField)p1, (IntelWebField)p2);
            return IntelWebParameter.ParamListToObjectArray(l);
        }
        public object[] List2_1(object p1)
        {
            List<IntelWebField> l = IntelWebParameter.List((IntelWebField)p1);
            return IntelWebParameter.ParamListToObjectArray(l);
        }
        public static List<IntelWebField> ObjectArrayToParamList(object la)
        {

            List<IntelWebField> r = new List<IntelWebField>();
            if (la == null || DBNull.Equals(DBNull.Value,la))
            {
                return r;
            }
            Array l = (Array)la;
            foreach (object n in l)
            {
                r.Add((IntelWebField)n);
            }

            return r;
        }
        public static object[] ParamListToObjectArray(List<IntelWebField> l)
        {
            object[] a = new object[l.Count];
            int n = -1;
            foreach (IntelWebField i in l)
            {
                n++;
                a[n] = i;
            }
            return a;
        }
        public object GetParam2(string name, string type, object size, object value)
        {
            return IntelWebParameter.GetParam(name, IntelWebDataTypeMapper.map(type).oleDBType, int.Parse(size.ToString()), value);
        }
    }
}