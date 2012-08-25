using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace IntelWeb
{
    public class SimpleSQLParser
    {
        private List<SQLPart> sParts = new List<SQLPart>();
        public enum SQLPartType : byte
        {
            fields = 1, from = 2, where = 3, groupby = 4, having = 5, orderby = 6
        }

        public struct parseInstruction
        {
            public string startkeyword;
            public string endkeyword;
        }
        public class SQLField
        {
            private string _entirefield = "";
            private string _expression = "";
            private string _alias = "";
            public SQLField(string fieldtext)
            {
                this.whole = fieldtext;
            }
            public string whole
            {
                get
                {
                    if (_alias == "")
                    {
                        return _expression;
                    }
                    else
                    {
                        return _expression + " as " + _alias;
                    }
                }
                set
                {
                    _entirefield = value;
                    _parse();
                }
            }
            private void _parse()
            {
                //parses the entire field into expression and alias
                int posas = InstrUn(1, _entirefield, " as ");
                if (posas > -1)
                {
                    _expression = _entirefield.Substring(0, posas);
                    _alias = _entirefield.Substring(posas + 4, _entirefield.Length - posas - 4);
                }
                else
                {
                    _alias = "";
                    _expression = _entirefield;
                }
            }
            public string expression
            {
                get
                {
                    return _expression;
                }
                set
                {
                    expression = value;
                }
            }
            public string alias
            {
                get
                {
                    return _alias;
                }
                set
                {
                    _alias = value;
                }
            }
        }
        public class SQLPart
        {
            public static SQLPart create(SQLPartType type, string partValue)
            {
                SQLPart i = new SQLPart();
                i.value = partValue;
                i.partType = type;
                return i;

            }
            public SQLPartType partType;
            public string value;

        }
        public SimpleSQLParser()
        {

        }
        public SimpleSQLParser(string SQL)
        {
            this.SQL = SQL;
        }
        public static string partFromSQL(string sql, SQLPartType part)
        {
            parseInstruction pi = SQLPartKeyword(part);
            int startpos = sql.IndexOf(pi.startkeyword, 0, StringComparison.InvariantCultureIgnoreCase);
            int endpos = -1;
            if (startpos == -1)
            {
                return "";
            }

            //Find the ending spot -- loop from the current sql part to the end looking for the next end keyword match

            for (byte i = (byte)part; i <= Enum.GetNames(typeof(SQLPartType)).Length; i++)
            {
                SQLPartType tpart = (SQLPartType)i;
                pi = SQLPartKeyword(tpart);
                if (pi.endkeyword == "")
                {
                    return sql.Substring(startpos, sql.Length - startpos); //return from the startpos to the end

                }
                else
                {
                    endpos = sql.IndexOf(pi.endkeyword, startpos + 1, StringComparison.InvariantCultureIgnoreCase);
                    if (endpos > -1)
                    {
                        return sql.Substring(startpos, endpos - startpos);
                    }
                }
            }
            return "";
        }
        public static parseInstruction SQLPartKeyword(SQLPartType part)
        {
            parseInstruction res = new parseInstruction();

            res.startkeyword = "";
            res.endkeyword = "";
            switch (part)
            {
                case SQLPartType.fields:
                    res.startkeyword = "Select ";
                    res.endkeyword = " From ";
                    break;
                case SQLPartType.from:
                    res.startkeyword = " From ";
                    res.endkeyword = " Where ";
                    break;
                case SQLPartType.where:
                    res.startkeyword = " Where ";
                    res.endkeyword = " Group By ";
                    break;
                case SQLPartType.groupby:
                    res.startkeyword = " Group By ";
                    res.endkeyword = " Having ";
                    break;
                case SQLPartType.having:
                    res.startkeyword = " Having ";
                    res.endkeyword = " Order By ";
                    break;
                case SQLPartType.orderby:
                    res.startkeyword = " Order By ";
                    res.endkeyword = "";
                    break;
            }
            return res;
        }
        public void deletePart(SQLPartType deletePartType)
        {
            SQLPart sp = this.sParts.Find(delegate(SQLPart opart)
            {
                return (opart.partType == deletePartType);
            });
            if (sp == null) { return; }
            sp.value = "";
        }
        public void addPart(string addItem, SQLPartType addPartType)
        {

            SQLPart sp = this.sParts.Find(delegate(SQLPart opart)
            {
                return (opart.partType == addPartType);
            });
            if (sp == null)
            {
                sp = new SQLPart();
                sp.partType = addPartType;
                sParts.Add(sp);
            }
            if (sp.value == null || sp.value == "")
            {
                sp.value += SQLPartKeyword(addPartType).startkeyword;

            }
            else if (addPartType == SQLPartType.where)
            {
                if (sp.value != null && sp.value.Trim() != "" && sp.value.Trim().ToUpper() != "WHERE")
                {
                    sp.value += " and "; // +addItem;
                }
            }
            else if (addPartType == SQLPartType.fields)
            {
                sp.value += ", ";
            }
            if (addPartType == SQLPartType.where && !(String.IsNullOrEmpty(addItem)))
            {
                sp.value += "(" + addItem + ")";
            }
            else
            {
                sp.value += addItem;
            }
        }
        public List<SimpleSQLParser.SQLField> Fields
        {
            get
            {
                List<SimpleSQLParser.SQLField> result = new List<SimpleSQLParser.SQLField>();
                string SelectPart = this.getPart(SQLPartType.fields);
                SelectPart = SelectPart.Trim();
                if (SelectPart.Substring(0, 6).Equals("select", StringComparison.InvariantCultureIgnoreCase))
                {
                    SelectPart = SelectPart.Substring(7, SelectPart.Length - 7).Trim();
                }
                int ppos = -1;
                int cpos = InstrUn(0, SelectPart, ",");
                while (cpos > -1)
                {

                    result.Add(new SQLField(SelectPart.Substring(ppos + 1, cpos - ppos - 1)));
                    ppos = cpos;
                    cpos = InstrUn(cpos + 1, SelectPart, ",");
                }
                if (SelectPart.Length > ppos)
                {
                    result.Add(new SQLField(SelectPart.Substring(ppos + 1, SelectPart.Length - ppos - 1)));
                }
                return result;
            }
        }
        public string getPart(SQLPartType part)
        {
            foreach (SQLPart tpart in sParts)
            {
                if (tpart.partType == part)
                {
                    return tpart.value;
                }

            }
            return "";
        }
        public string SQL
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (byte i = 1; i <= Enum.GetNames(typeof(SQLPartType)).Length; i++)
                {
                    sb.Append(this.getPart((SQLPartType)i) + " ");
                }
                return sb.ToString();
            }
            set
            {
                this.sParts.Clear();
                List<string> a = new List<string>();
                foreach (SQLPartType part in Enum.GetValues(typeof(SQLPartType)))
                {
                    SQLPart sp = new SQLPart();
                    sp.partType = part;
                    sp.value = partFromSQL(value, part);
                    if (sp.value != "")
                    {
                        this.sParts.Add(sp);
                    }
                }
            }
        }

        public static int InstrUn(int startingPosition, string searchIn, string searchFor)
        {
            bool isEven = true;
            List<char> Even = new List<char>();
            Even.Add('\'');
            List<char> Additive = new List<char>();
            Additive.Add('(');
            List<char> Subtractive = new List<char>();
            Subtractive.Add(')');
            int shouldEqualZero = 0;
            int curpos = 0;
            foreach (char a in searchIn.ToCharArray())
            {
                curpos += 1;
                if (isEven && Additive.Contains(a))
                {
                    shouldEqualZero += 1;
                }
                else if (Subtractive.Contains(a))
                {
                    shouldEqualZero -= 1;
                }
                else if (Even.Contains(a))
                {
                    isEven = !isEven;
                }

                if (curpos >= startingPosition && isEven && shouldEqualZero == 0 && curpos < searchIn.Length - searchFor.Length && searchIn.Substring(curpos, searchFor.Length).Equals(searchFor, StringComparison.InvariantCultureIgnoreCase))
                {
                    return curpos;
                }

            }
            return -1;
        }
        public static int IntrUnquoted(int startingPosition, string searchIn, string searchFor)
        {
            /*
             * 
             * Public Function InstrUnQuoted(startingposition, searchin, searchfor)
    'Returns the unwrapped position of searchfor in searchin
    InstrUnQuoted = 0: iseven = True: QuoteChar = "'"
    For counter = startingposition To Len(searchin)
        If InStr(1, QuoteChar, Mid(searchin, counter, 1), vbTextCompare) > 0 Then
            iseven = Not iseven
        End If
        If iseven And UCase(Mid(searchin, counter, Len(searchfor))) = UCase(searchfor) Then
            InstrUnQuoted = counter
            Exit Function
        End If
    Next
End Function
             */
            bool isEven = true;
            char quoteChar = '\'';
            char[] chars = searchIn.ToCharArray();
            for (int n = startingPosition; n < chars.Length; n++)
            {
                char c = chars[n];
                if (c.Equals(quoteChar))
                {
                    isEven = !isEven;
                }
                if (isEven)
                {
                    if (searchIn.ToUpper().IndexOf(searchFor.ToUpper(), n).Equals(n))
                    {
                        return n;
                    }
                }
            }
            return -1;
        }
        public static bool isBalancedUnquoted(string text, char additiveChar, char subtractiveChar)
        {
            bool isEven = true;
            char quoteChar = '\'';
            int balancer = 0;
            char[] chars = text.ToCharArray();
            foreach (char c in chars)
            {
                if (c.Equals(quoteChar))
                {
                    isEven = !isEven;
                }
                if (isEven)
                {
                    if (c.Equals(additiveChar))
                    {
                        balancer = balancer + 1;
                    }
                    if (c.Equals(subtractiveChar))
                    {
                        balancer = balancer - 1;
                    }
                }
                if (balancer < 0)
                {
                    return false;
                }
            }
            return balancer.Equals(0);
        }
        public class injectionResult
        {
            public bool safe = false;
            public string reason = "";
            public injectionResult(bool isSafe, string rejectionReason)
            {
                this.safe = isSafe;
                this.reason = rejectionReason;
            }
        }
        public static List<string> splitUn(string text, string delimiter)
        {
            /*
             Public Function SplitUN(thestring, delimiter)
    Dim local_ta()
    pos = 1: counter = -1: prevpos = 1

    While pos < Len(thestring) + 1
        pos = InstrUn(pos, thestring, delimiter)
        counter = counter + 1

        If pos = 0 Then pos = Len(thestring) + 1
        ReDim Preserve local_ta(counter)
        local_ta(counter) = Mid(thestring, prevpos, pos - prevpos)
        If Left(local_ta(counter), Len(delimiter)) = delimiter Then local_ta(counter) = Right(local_ta(counter), Len(local_ta(counter)) - Len(delimiter))
        If Right(local_ta(counter), Len(delimiter)) = delimiter Then local_ta(counter) = Left(local_ta(counter), Len(local_ta(counter)) - Len(delimiter))
        If pos > 0 Then pos = pos + 1
        prevpos = pos
    Wend
    SplitUN = local_ta
End Function 
             */
            List<string> result = new List<string>();
            int pos = 0;
            int prevPos = 0;
            while (pos < text.Length)
            {
                pos = SimpleSQLParser.InstrUn(pos, text, delimiter);
                if (pos.Equals(-1))
                {
                    pos = text.Length;
                }
                result.Add(text.Substring(prevPos, pos - prevPos));
                pos += 1;
                prevPos = pos;

            }
            return result;
        }
        public static injectionResult isInjectionSafe(string text)
        {

            /*
                1 The field must not contain any unenclosed  in single quotes characters considered unsafe
                2 The field must not contain any unenclosed in single quotes words considered unsafe
                3 The field must contain an even number of single quotes, many injection techniques rely on unbalanced number of quotes
                4 The field must not contain comma outside of single quotes and parenthesis
                5 The field must have balanced number of ({[ and }]) outside of single quotation marks

                //1 ----------------------------
                        */

            List<string> badChars = new List<string>() { ";", "@" };
            foreach (string bad in badChars)
            {
                if (SimpleSQLParser.IntrUnquoted(0, text, bad) > -1)
                {
                    return new injectionResult(false, "Contains an unquoted '" + bad + "' character.");
                }
            }

            //2 --------------------------

            List<string> badWords = new List<string>() { "--", "select", "update", "exec", "insert", "dbcc", "union", "openrowset", "from", "create", "delete", "sp_", "xp_", "sql", "open", "pwd", "hra", "waitfor", "0x", "bulk", "bcp", "wscript", "shell", "cmd", "call", "cross", "join", "declare", "cursor", "drop", "kill", "open", "fetch", "lock", "shutdown", "raiseerror", "writetext", "procedure", "break", "while", "function", "current_user", "opendatasource", "openquery", "openxml", "grant", "revoke", "escape", @"*\", @"\*", @"/*", @"*/" };
            foreach (string bad in badWords)
            {
                if (SimpleSQLParser.IntrUnquoted(0, text, bad) > -1)
                {
                    return new injectionResult(false, "Contains the unquoted word '" + bad + "' which is considered to be unsafe.");
                }
            }

            //3 ---------------------------

            bool isEven = true;

            char[] chars = text.ToCharArray();
            foreach (char c in chars)
            {
                if (c.Equals('\''))
                {
                    isEven = !isEven;
                }
            }
            if (!isEven)
            {
                return new injectionResult(false, "Contains an uneven number of apostrophes in the text");
            }

            //4 ----------------------------

            if (SimpleSQLParser.InstrUn(0, text, ",") > -1)
            {
                return new injectionResult(false, "Contains an unenclosed comma");
            }
            if (!SimpleSQLParser.isBalancedUnquoted(text, '(', ')'))
            {
                return new injectionResult(false, "Contains an uneven number of open and close parenthesis.");
            }
            if (!SimpleSQLParser.isBalancedUnquoted(text, '[', ']'))
            {
                return new injectionResult(false, "Contains an uneven number of open and close square brackets.");
            }
            if (!SimpleSQLParser.isBalancedUnquoted(text, '{', '}'))
            {
                return new injectionResult(false, "Contains an uneven number of open and close curly braces.");
            }

            return new injectionResult(true, "");
        }
    }
}
