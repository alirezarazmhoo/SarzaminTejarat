using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace System
{
    public class ShamsiDate
    {
        public static string ToShamsi(DateTime dt)
        {
            string res = "";
            PersianCalendar p = new PersianCalendar();
            res += p.GetYear(dt);
            res += "/";
            res += p.GetMonth(dt);
            res += "/";
            res += p.GetDayOfMonth(dt);
            return res;
        }
    }
}