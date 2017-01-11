using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public static class XSolverHelpers
    {

        public static bool AsBool(object data)
        {
            string s = data as string;

            if (data is bool)
            {
                return (bool)data;
            }
            else if (data is double)
            {
                return (double)data != 0.0;
            }
            else if (data is int)
            {
                return (int)data != 0;
            }
            else if (s != null)
            {
                return s.Length != 0;
            }
            else
            {
                return false;
            }
        }


        public static double AsDouble(object data)
        {
            string s = data as string;

            if (data is bool)
            {
                return (bool)data ? 1.0 : 0.0;
            }
            else if (data is double)
            {
                return (double)data;
            }
            else if (data is int)
            {
                return 1.0 * (int)data;
            }
            else if (s != null)
            {
                double v;
                double.TryParse(s, out v);
                return v;
            }
            else
            {
                return default(double);
            }
        }


        public static bool Match(object da, object db)
        {
            string sa = da as string;
            string sb = db as string;

            if (da == null || db == null)
            {
                return da == null && db == null;
            }
            else if (da is bool && db is bool)
            {
                return (bool)da == (bool)db;
            }
            else if (da is double && db is double)
            {
                return (double)da == (double)db;
            }
            else if (da is int && db is int)
            {
                return (int)da == (int)db;
            }
            else if (da is string && db is string)
            {
                return (string)da == (string)db;
            }
            else
            {
                double na = AsDouble(da);
                double nb = AsDouble(db);
                return na == nb;
            }
        }


        public static int Compare(object da, object db)
        {
            string sa = da as string;
            string sb = db as string;

            if (da == null || db == null)
            {
                if (da == null)
                {
                    return db == null ? 0 : -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (da is bool && db is bool)
            {
                return ((bool)da).CompareTo((bool)db);
            }
            else if (da is double && db is double)
            {
                return ((double)da).CompareTo((double)db);
            }
            else if (da is int && db is int)
            {
                return ((int)da).CompareTo((int)db);
            }
            else if (da is string && db is string)
            {
                return ((string)da).CompareTo((string)db);
            }
            else
            {
                double na = AsDouble(da);
                double nb = AsDouble(db);
                return na.CompareTo(nb);
            }
        }

    }
}
